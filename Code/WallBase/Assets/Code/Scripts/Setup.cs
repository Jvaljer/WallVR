using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Setup : MonoBehaviourPun {
    //public type arg { get; private set; } is read by compiler as :
    /*
        private type arg;
        public type Arg(){
            return arg;
        }
        private void SetArg(type val){
            arg = val;
        }
    */
    public bool is_master { get; private set; } //client if no specifications
    public int part_cnt { get; private set; } //amount of participant we want in the program

    //wall attributes (usign Wilder's as basic)
    public float wall_width { get; private set; } 
    public float wall_height { get; private set; }
    private string wall_str;
    private Wall wall;

    //screen attributes (operator screen)
    public int screen_width { get; set; }
    public int screen_height { get; set; }

    //Screen attributes (participant screen ?)
    public bool full_screen { get; private set; }

    //positionning attributes (client screens)
    private string column;
    private string row;
    public float wall_pos_x { get; private set; }
    public float wall_pos_y { get; private set; }

    public void Awake(){
        //parsing all the given args
        Debug.Log("Setup -> starting to parse arguments");
        
        //all defautl values
        is_master = false;
        full_screen = false; //-popupwindow used by default ?
        part_cnt = 10;

        string[] args = System.Environment.GetCommandLineArgs ();
        for(int i=0; i<args.Length; i++){
            //switch faster than if()if()... -> jump table by compiler
            switch (args[i]){
                //all render datas
                case "-wall":
                    wall_str = args[i+1];
                    break;
                case "-sw":
                    screen_width = int.Parse(args[i+1]);
                    break;
                case "-sh":
                    screen_height = int.Parse(args[i+1]);
                    break;
                case "-fs":
                    int fs = int.Parse(args[i+1]);
                    full_screen = fs != 0; //if fs=0 -> false, else true 
                    break;
                //all 'hierarchy' datas 
                case "-r": 
                    //looking at user's role
                    if(args[i+1]=="m"){
                        //is master 
                        Debug.Log("user is master");
                        is_master = true; //quite useless -> PhotonNetwork.IsMasterClient defining this
                    } else if(args[i+1]=="p"){
                        //is  client
                        Debug.Log("user is participant");
                        is_master = false;
                    }
                    break;
                case "-x":
                    column = args[i+1];
                    break;
                case "-y":
                    row = args[i+1];
                    break;
                
                //program predicate datas
                case "-pa":
                    //getting the participants amount (not including operator)
                    part_cnt = int.Parse(args[i+1]);
                    break;
            }
        }

        //setting wall attributes
        switch (wall_str){
            case "WILDER":
                wall = new Wilder();
                break;
            case "WILDEST":
                //wall = new Wildest();
                break;
            case "DESKTOP":
                wall = new Desktop(2,1);
                break;
            default:
                //using wilder
                wall = new Wilder();
                break;
        }
        wall_height = wall.Height();
        wall_width = wall.Width();

        //creating setup thx to parsing
        if(is_master){
            
        } else {
            //specific position values 
            if(column=="a"){
                wall_pos_x = 0;
            } else {
                wall_pos_x = wall_width/2 + wall.SingleScreenWidth();
            }

            wall_pos_y = float.Parse(row)*wall.SingleScreenHeight();
        }

        //now that all this has been initialized we wanna connect to the server !
        ConnectToServer();
    }

    public void ConnectToServer(){
        Debug.Log("user attempts a connexion to server");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = System.DateTime.Now.Ticks.ToString();
    }
}
