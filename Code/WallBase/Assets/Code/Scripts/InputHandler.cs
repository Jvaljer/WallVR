using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InputHandler : MonoBehaviourPun {
    //Referenced setup & wall
    private Setup setup;

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
        Debug.Log("InputHandler Awakes");
        m_devices = new Dictionary<object, MDevice>();
        p_cursors = new Dictionary<int, PCursor>();
        to_delete_ids = new List<int>();
    }

    public void Start(){
        Debug.Log("InputHandler Starts "+PhotonNetwork.LocalPlayer.ActorNumber);
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();


        //supposing we don't have any right/left camera, only the main one (WILDER wall)
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            Cursor.visible = true; 
            RegisterDevice("Mouse", this);
            CreateMCursor(this, 0, 0.5f, 0.5f, Color.red);
        } else {
            Cursor.visible = false;
            cursor_HW = 16*4;
        }
    }

    public void Update(){
        if(PhotonNetwork.IsMasterClient){
            float mouse_x = Input.mousePosition.x/Screen.width;
            float mouse_y = (Screen.height - Input.mousePosition.y)/Screen.height;

            //handling drag & drop
            if(Input.GetMouseButtonDown(0)){
                StartMoveMCursor(this, 0, mouse_x, mouse_y, false);
            } else if(Input.GetMouseButtonUp(0)){
                StopMoveMCursor(this, 0, mouse_x, mouse_y);
            } else {
                MoveMCursor(this, 0, mouse_x, mouse_y);
            }

            //handling cursors
            to_delete_ids.Clear();
            foreach(MDevice dev in m_devices.Values){
                foreach(MCursor mc in dev.cursors.Values){
                    //if not related PCursor then create it
                    if(mc.p_cursor==null){
                        mc.AddPCursor(new PCursor(mc.x, mc.y, mc.c));
                        //p_cursors.Add(uid_creator, mc.p_cursor);
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
                            photonView.RPC("RemovePCursorRPC", RpcTarget.AllBuffered, mc.uid);
                        }
                    } else if(mc.x != mc.p_cursor.x || mc.y != mc.p_cursor.y){
                        photonView.RPC("MoveOrCreatePCursorRPC", RpcTarget.AllBuffered, mc.uid, mc.x, mc.y, mc.c.ToString());
                        mc.p_cursor.Move(mc.x, mc.y);
                    }

                    //drag predicates handling
                    if(mc.start_drag){
                        mc.start_drag = false;
                    }
                    if(mc.end_drag){
                        mc.end_drag = false;
                    }
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
        public bool start_drag { get; set; }
        public bool end_drag { get; set; }

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

        public void Drag(){
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
        Debug.Log("first Create Cursor");
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
        cursor.Move(x_, y_);
        if(d_){
            cursor.start_drag = true;
        }
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
        if(cursor.drag){
            cursor.end_drag = true;
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
            c_ = c_;
            tex = CursorsTex.SimpleCursor(c, Color.black, cursor_HW, cursor_T, cursor_L);
        }

        public void Move(float x_, float y_){
            x = x_;
            y = y_;
        }
    };

    //rendering of the cursors (guessing we don't have any bezels (yet...))
    public void OnGUI(){
        Debug.Log("OnGUI "+p_cursors.Count);
        foreach(PCursor pc in p_cursors.Values){
            float x, y, x1;
            if(PhotonNetwork.IsMasterClient){
                x = pc.x*Screen.width;
                y = pc.y*Screen.height;
                x1 = x;
            } else {
                x = - setup.wall_pos_x + (pc.x * setup.wall_width);
                y = - setup.wall_pos_y + (pc.y * setup.wall_height);
                x1 = - setup.wall_pos_x + (pc.x * setup.wall_width) - Screen.width/2;
            }

            GUI.DrawTexture(new Rect(x - cursor_HW, y - cursor_HW, 2*cursor_HW, 2*cursor_HW), pc.tex);
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
        Debug.Log("Creating a participant cursor -> ("+uid+":"+"("+x_+","+y_+")");
        Color color;
        ColorUtility.TryParseHtmlString(str, out color);
        p_cursors.Add(uid, new PCursor(x_, y_, color));
    }

    //RPC to remove a PCursor
    [PunRPC]
    public void RemovePCursorRPC(int uid){
        Debug.Log("Removing the participant cursor : "+uid);
        p_cursors.Remove(uid);
    }

    //RPC to move a PCursor
    [PunRPC]
    public void MoveOrCreatePCursorRPC(int uid, float x_, float y_, string str){
        if(!p_cursors.ContainsKey(uid)){
            Debug.Log("(from Move/Create):: Creating a participant cursor -> ("+uid+":"+"("+x_+","+y_+")");
            Color color;
            ColorUtility.TryParseHtmlString(str, out color);
            p_cursors.Add(uid, new PCursor(x_, y_, color));
        } else {
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
            Debug.Log("Create new MCursor "+id_);
            MCursor cursor = new MCursor(id_, x_, y_, c_);
            cursors.Add(id_, cursor);
            return cursor;
        }

        //cursor removing method
        public void RemoveCursor(int id_){
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
        Debug.Log("Registering new device "+str);
        //if the object is already referrring a device -> nothing to register
        if(GetDevice(obj)!=null){
            return;
        }
        m_devices.Add(obj, new MDevice(str));
    }
}
