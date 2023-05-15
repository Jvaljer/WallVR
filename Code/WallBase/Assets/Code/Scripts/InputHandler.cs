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
    public Dictionary<int, PartCursor> p_cursors { get; set; }
    public Dictionary<int, MasterCursor> m_cursors { get; set; }

    //Devices's dictionnary
    public Dictionnary<object, MDevice> m_devices { get; set; }

    public void Awake(){
        Debug.Log("InputHandler Awakes");
        devices = new Dictionary<object, MDevice>();
        p_cursors = new Dictionary<int, PCursor>();
    }
    public void Start(){
        Debug.Log("InputHandler Starts");
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();

        //supposing we don't have any right/left camera, only the main one (WILDER wall)
        if(PhotonNetwork.IsMasterClient){
            Cursor.visible = true; 
            RegisterDevice("Mouse", this);
            CreateMCursor(this, 0, 0.5f, 0.5f, Color.red);
        } else {
            Cursor.visible = false;
            cursor_HW = 16*4;
        }
    }

    /******************************************************************************/
    /*                 MASTER CURSORS CLASS AND METHODS                           */
    /******************************************************************************/
    public class MCursor {
        //all Master Cursor's attributes
        public PCursor p_cursor { get; set; }
        public int id { get; private set; }
        public int uid { get; set; }
        public Color c { get; set; }
        public float x { get; private set; }
        public float y { get; private set; }
        public int button { get; private set; }
        public bool hidden { get; private set; }

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

        public void Move(float x_, float y_){
            x = x_;
            y = y_;
        }

    };


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

    //RPC to create a PCursor
    [PunRPC]
    public void CreatePCursorRPC(int uid, float x_, float y_, Color c_){
        Debug.Log("Creating a participant cursor -> ("+uid+":"+"("+x_+","+y_+")");
        part_cursors.Add(uid, new PCursor(x_, y_, c_));
    }

    //RPC to remove a PCursor
    [PunRPC]
    public void RemovePCursorRPC(int uid){
        Debug.Log("Removing the participant cursor : "+uid);
        p_cursors.Remove(uid);
    }

    //RPC to move a PCursor
    [PunRPC]
    public void MoveOrCreatePCursorRPC(int uid, float x_, float y_, Color c_){
        if(!part_cursors.ContainsKey(uid)){
            Debug.Log("(from Move/Create):: Creating a participant cursor -> ("+uid+":"+"("+x_+","+y_+")");
            p_cursors.Add(uid, new PCursor(x_, y_, c_));
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
            MCursor cursor = new MCursor(id_, x_, y_);
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
}
