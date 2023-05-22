using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InputHandler : MonoBehaviourPun {
    private bool up_run = true;
    private bool up_init = true;
    //Referenced setup & wall
    private Setup setup;
    public bool initialized { get; set; } = false;

    //Referenced rendering
    private Renderer render;

    //Some cursors attributes
    private static int cursor_HW = 16;
    private static int cursor_T = 1;
    private static int cursor_L = 1;

    //creator attributes
    private int uid_creator = 0;

    //Cursors's dictionnary
    public Dictionary<int, PCursor> p_cursors { get; set; }
    public Dictionary<int, MCursor> m_cursors { get; set; }

    //Devices's dictionnary
    public Dictionary<object, MDevice> m_devices { get; set; }

    //must delete list
    private List<int> to_delete_ids;

    public void Awake(){
        //Debug.Log("InputHandler Awakes");
        m_devices = new Dictionary<object, MDevice>();
        p_cursors = new Dictionary<int, PCursor>();
        to_delete_ids = new List<int>();
    }

    public void InitalizeIH(){
        //Debug.Log("InputHandler Initialize "+photonView.IsMine);
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        render = GameObject.Find("ScriptManager").GetComponent<Renderer>();
        
        //supposing we don't have any right/left camera, only the main one (WILDER wall)
        if(photonView.IsMine){
            Cursor.visible = true; 
            RegisterDevice("Mouse", this);
            CreateMCursor(this, 0, 0.5f, 0.5f, Color.red);
        } else {
            Cursor.visible = false;
            cursor_HW = 16*4;
        }
        GameObject.Find("Circle(Clone)").GetComponent<Shape>().AddOwner(0);
        //GameObject.Find("Square(Clone)").GetComponent<Shape>().AddOwner(0);
    }

    public void Update(){
        /* if(up_run){
            Debug.LogError("ih update running");
            up_run = false;
        } */
        if(photonView.IsMine && initialized){
            /*if(up_init){
                Debug.LogError("Ih -> Update is good");
                up_init = false;
            } */
            //Debug.Log("we do be running");
            float mouse_x = Input.mousePosition.x/Screen.width;
            float mouse_y = (Screen.height - Input.mousePosition.y)/Screen.height;

            //handling drag & drop
            if(Input.GetMouseButtonDown(0)){
                //Debug.Log("Cursor Start Move");
                StartMoveMCursor(this, 0, mouse_x, mouse_y, true);
                photonView.RPC("InputRPC", RpcTarget.AllBuffered, "Down", mouse_x, mouse_y, 0);
            } else if(Input.GetMouseButtonUp(0)){
                //Debug.Log("Cursor Stop Move");
                StopMoveMCursor(this, 0, mouse_x, mouse_y);
                photonView.RPC("InputRPC", RpcTarget.AllBuffered, "Up", mouse_x, mouse_y, 0);
            } else {
                //Debug.Log("Cursor Move");
                MoveMCursor(this, 0, mouse_x, mouse_y);
                if(GetMCursor(this,0).drag){
                    photonView.RPC("InputRPC", RpcTarget.AllBuffered, "Move", mouse_x, mouse_y, 0);
                }
            }

            //handling shape creation ? 
            if(Input.GetMouseButtonDown(1)){
                Debug.LogError("right click buddy");
            }

            //handling cursors
            to_delete_ids.Clear();
            foreach(MDevice dev in m_devices.Values){
                foreach(MCursor mc in dev.cursors.Values){
                    //if not related PCursor then create it
                    if(mc.p_cursor==null){
                        mc.AddPCursor(new PCursor(mc.x, mc.y, mc.c));
                        //Debug.Log("Master -> CreatePCursorRPC");
                        photonView.RPC("CreatePCursorRPC", RpcTarget.AllBuffered, uid_creator, mc.x, mc.y, mc.c.ToString());
                        mc.uid = uid_creator;
                        uid_creator++;
                    }

                    //if MCursor to delete then delete it 
                    if(mc.to_delete){
                        to_delete_ids.Add(mc.id);
                        mc.RemovePCursor();
                        p_cursors.Remove(mc.uid);
                        if(!mc.hidden){
                            //Debug.Log("Master -> RemovePCursorRPC");
                            photonView.RPC("RemovePCursorRPC", RpcTarget.AllBuffered, mc.uid);
                        }
                    } else if(mc.x != mc.p_cursor.x || mc.y != mc.p_cursor.y){
                        //Debug.Log("Master -> MoveOrCreatePCursorRPC");
                        photonView.RPC("MoveOrCreatePCursorRPC", RpcTarget.AllBuffered, mc.uid, mc.x, mc.y, mc.c.ToString());
                        mc.p_cursor.Move(mc.x, mc.y);
                    }

                    //drag predicates handling
                    if(mc.clicked){
                        mc.clicked = false;
                    }
                }

                foreach(int id in to_delete_ids){
                    dev.RemoveCursor(id);
                }
                to_delete_ids.Clear();
            }
        }
    }

    /******************************************************************************/
    /*                 MASTER CURSORS CLASS AND METHODS                           */
    /******************************************************************************/
    public class MCursor {
        //all Master Cursor's attributes
        public PCursor p_cursor { get; private set; }
        public int id { get; private set; }
        public int uid { get; set; }
        public Color c { get; set; }
        public float x { get; private set; }
        public float y { get; private set; }
        public int button { get; set; }
        public bool hidden { get; set; }
        public bool drag { get; set; }
        public bool clicked { get; set; }
        public bool to_delete { get; set; }

        //Constructor
        public MCursor(int id_, float x_, float y_, Color c_){
            id = id_;
            x = x_;
            y = y_;
            c = c_;
            p_cursor = null;
            drag = false;
            clicked = false;
        }

        //setters
        public void Move(float x_, float y_){
            x = x_;
            y = y_;
        }

        public void AddPCursor(PCursor pc){
            p_cursor = pc;
        }
        public void RemovePCursor(){
            p_cursor = null;
        }

        public void Pick(){
            drag = true;
        }
        public void Drop(){
            drag = false;
        }
    };

    //all related methods
    public MCursor GetMCursor(object obj, int id_){
        if(m_devices.ContainsKey(obj)){
            return m_devices[obj].GetCursor(id_);
        }
        return null;
    }

    public void CreateMCursor(object obj, int id_, float x_, float y_, Color c_, bool hid_=false){
        //Debug.Log("first Create Cursor");
        MDevice device = GetDevice(obj);
        if(device==null){
            return;
        }
        MCursor cursor = device.GetCursor(id_);
        if(cursor!=null){
            return;
        }

        cursor = device.CreateCursor(id_, x_, y_, c_);
        cursor.hidden = hid_;
    }

    public void RemoveCursor(object obj, int id_){
        MDevice device = GetDevice(obj);
        if(device==null){
            return;
        }
        MCursor cursor = device.GetCursor(id_);
        if(cursor==null){
            return;
        }
        cursor.to_delete = true;
    }

    //moving methods
    public void StartMoveMCursor(object obj, int id_, float x_, float y_, bool d_){
        MCursor cursor = GetMCursor(obj, id_);
        if(cursor==null){
            return;
        }
        cursor.Pick();
    }
    public void MoveMCursor(object obj, int id_, float x_, float y_){
        MCursor cursor = GetMCursor(obj, id_);
        if(cursor==null){
            return;
        }
        cursor.Move(x_, y_);
    }
    public void StopMoveMCursor(object obj, int id_, float x_, float y_){
        MCursor cursor = GetMCursor(obj, id_);
        if(cursor==null){
            return;
        }
        cursor.Drop();
    }

    public void CursorClick(object obj, int id_, float x_, float y_, int btn=0){
        MCursor cursor = GetMCursor(obj, id_);
        if(cursor==null){
            return;
        }
        cursor.clicked = true;
        cursor.button = btn;
    }
    /******************************************************************************/
    /*            PARTICIPANT CURSORS CLASS AND METHODS                           */
    /******************************************************************************/
    public class PCursor{
        public float x { get; private set; }
        public float y { get; private set; }
        public Color c { get; set; }
        public Texture2D tex { get; set; }

        public PCursor(float x_, float y_, Color c_){
            x = x_;
            y = y_;
            c = c_;
            tex = CursorsTex.SimpleCursor(c, Color.black, cursor_HW, cursor_T, cursor_L);
        }

        public void Move(float x_, float y_){
            x = x_;
            y = y_;
        }
    };

    //rendering cursors
    public void OnGUI(){
        if(initialized){
            //Debug.Log("OnGUI "+p_cursors.Count+" master : "+PhotonNetwork.IsMasterClient);
            foreach(PCursor pc in p_cursors.Values){
                float x, y;
                if(PhotonNetwork.IsMasterClient){
                    x = pc.x*Screen.width;
                    y = pc.y*Screen.height;
                    //Debug.Log("master shows cursor on : "+(new Vector2(x,y)));
                } else {
                    x = -setup.x_pos + pc.x * setup.wall_width;
                    y = -setup.y_pos + pc.y * setup.wall_height;
                    //Debug.Log("participant shows cursor on : "+(new Vector2(x,y)));
                }
    
                GUI.DrawTexture(new Rect(x - cursor_HW, y - cursor_HW, 2*cursor_HW, 2*cursor_HW), pc.tex);
            }
        }
    }

    //related methods
    public PCursor GetPCursor(object obj, int id_){
        if(GetMCursor(obj, id_)!=null){
            return GetMCursor(obj, id_).p_cursor;
        }
        return null;
    }

    //RPC to create a PCursor
    [PunRPC]
    public void CreatePCursorRPC(int uid, float x_, float y_, string str){
        //Debug.Log("Creating a participant cursor -> ("+uid+":"+"("+x_+","+y_+")");
        Color color;
        ColorUtility.TryParseHtmlString(str, out color);
        p_cursors.Add(uid, new PCursor(x_, y_, color));
    }

    //RPC to remove a PCursor
    [PunRPC]
    public void RemovePCursorRPC(int uid){
        //Debug.Log("Removing the participant cursor : "+uid);
        p_cursors.Remove(uid);
    }

    //RPC to move a PCursor
    [PunRPC]
    public void MoveOrCreatePCursorRPC(int uid, float x_, float y_, string str){
        if(!p_cursors.ContainsKey(uid)){
            //Debug.Log("(from Move/Create):: Creating a participant cursor -> ("+uid+":"+"("+x_+","+y_+")");
            Color color;
            ColorUtility.TryParseHtmlString(str, out color);
            p_cursors.Add(uid, new PCursor(x_, y_, color));
        } else {
            //Debug.Log("Moving a Participant cursor -> ("+x_+","+y_+")");
            p_cursors[uid].Move(x_, y_);
        }
    }

    /******************************************************************************/
    /*                 MASTER DEVICES CLASS AND METHODS                           */
    /******************************************************************************/
    public class MDevice {
        public string name { get; private set; }
        public Dictionary<int, MCursor> cursors { get; set; }

        //Constructor
        public MDevice(string str){
            name = str;
            cursors = new Dictionary<int, MCursor>();
        }

        //specific cursor getter
        public MCursor GetCursor(int id_){
            if(cursors.ContainsKey(id_)){
                return cursors[id_];
            }
            return null;
        }

        //cursor creating method (adds & returns a new cursor);
        public MCursor CreateCursor(int id_, float x_, float y_, Color c_){
            //Debug.Log("Create new MCursor "+id_);
            MCursor cursor = new MCursor(id_, x_, y_, c_);
            cursors.Add(id_, cursor);
            return cursor;
        }

        //cursor removing method
        public void RemoveCursor(int id_){
            //Debug.Log("Removing a MCursor "+id_);
            cursors.Remove(id_);
        }
    };

    public MDevice GetDevice(object obj){
        if(m_devices.ContainsKey(obj)){
            return m_devices[obj];
        }
        return null;
    }

    public void RegisterDevice(string str, object obj){
        //Debug.Log("Registering new device "+str);
        //if the object is already referrring a device -> nothing to register
        if(GetDevice(obj)!=null){
            return;
        }
        m_devices.Add(obj, new MDevice(str));
    }

    /******************************************************************************/
    /*                      ALL OTHER STUFF HANDLING METHODS                      */
    /******************************************************************************/

    public void ParticipantReady(){
        initialized = true;
        //Debug.LogError("has init IH -> init render");
        render.Initialize();
    }

    [PunRPC]
    public void InputRPC(string str, float x_, float y_, int id_){
        //Debug.Log("InputRPC -> "+str);
        Vector3 input; 
        if(PhotonNetwork.IsMasterClient){
            input = Camera.main.ScreenToWorldPoint(new Vector3(x_*Screen.width, y_*Screen.height, 0f));
        } else {
            input = Camera.main.ScreenToWorldPoint(new Vector3(-setup.x_pos + x_ * setup.wall_width, -setup.y_pos + y_ * setup.wall_height, 0f));
        }
        input.y *= -1;
        input.z = 0f;
        //Debug.Log("Input Sent to render via RPC");
        //Debug.Log("input is on : "+input);
        render.Input(str, input, id_); //turn this into RPC ??
    }
}
