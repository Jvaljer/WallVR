using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Setup : MonoBehaviourPun {
    //player role & condition into the program
    public bool is_master { get; private set; }
    public int part_cnt { get; private set; }
    public bool is_vr { get; private set; }

    //wall attributes
    public float wall_width { get; private set; } 
    public float wall_height { get; private set; }
    private string wall_str;
    public Wall wall { get; private set; }

    //screen attributes (operator screen)
    public int screen_width { get; set; }
    public int screen_height { get; set; }
    public Camera own_cam { get; set; }

    //Screen attributes (participant screen ?)
    public bool full_screen { get; private set; }
    public float zoom_ratio { get; private set; }

    //positionning attributes (client screens)
    public float wall_pos_x { get; private set; }
    public float wall_pos_y { get; private set; }
    public float x_pos { get; private set; }
    public float y_pos { get; private set; }

    public void Awake(){
        //default values with (1 ope) + (10 2DP) + (1 VRP)
        is_master = false;
        full_screen = false;
        is_vr = false;
        part_cnt = 11; //not counting ope in these

        string[] args = System.Environment.GetCommandLineArgs();
        for(int i=0; i<args.Length; i++){
            switch (args[i]){
                //wall & screen settings
                case "-wall":
                    wall_str = args[i+1];
                    break;
                case "-sw": //override by 'screen-width'
                    screen_width = int.Parse(args[i+1]);
                    break;
                case "-sh": //override by 'screen-height'
                    screen_height = int.Parse(args[i+1]);
                    break;
                case "-fs": //override by 'fullscreen'
                    int fs = int.Parse(args[i+1]);
                    full_screen = (fs!=0);
                    break;
                
                //user role & condition
                case "-r":
                    switch (args[i+1]){
                        case "m":
                            is_master = true;
                            is_vr = false;
                            break;
                        case "p2d":
                            is_master = false;
                            is_vr = false;
                            break;
                        case "pvr":
                            is_master = false;
                            is_vr = true;
                            break;
                    }
                    break;
                case "-x":
                    x_pos = float.Parse(args[i+1]);
                    break;
                case "-y":
                    y_pos = float.Parse(args[i+1]);
                    break;
                
                //program predicates
                case "-pa":
                    part_cnt = int.Parse(args[i+1]);
                    break;
                case "-mo":
                    int v = int.Parse(args[i+1]);
                    if(v==1){
                        part_cnt = v;
                    }
                    break;
                
                default:
                    break;
            }
        }

        //setting used wall
        switch (wall_str){
            case "WILDER":
                wall = new Wilder();
                break;
            case "DESKTOP":
                wall = new Desktop(1,2); //Desktop(2,2)
                break;
            default:
                wall = new Wilder();
                break;
        }
        wall_height = wall.Height();
        wall_width = wall.Width();

        zoom_ratio = 1f; //must accurate

        ConnectToServer();
    }

    public void ConnectToServer(){
        GameObject.Find("ScriptManager").GetComponent<NetworkHandler>().Connect();
    }
}
