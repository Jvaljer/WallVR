using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Participant : MonoBehaviourPun {
    //referenced setup
    private Setup setup;

    public bool initialized { get; set; } = false;

    public void Start(){
        Debug.Log("Participant "+PhotonNetwork.LocalPlayer.ActorNumber+" is starting");
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        if(photonView.IsMine){
            Debug.Log("I am P"+PhotonNetwork.LocalPlayer.ActorNumber);
            if(setup!=null){
                Debug.Log("initializing my attributes");
                InitAttributes();
            } else {
                //program can run if all is good
                //might have nothing to do there
            }
        }
    }

    public void InitAttributes(){
        //setting the camera
        Vector3 old_pos = Camera.main.transform.position;
        Vector3 scale = Camera.main.transform.localScale;
        float center_x = setup.wall_pos_x + (setup.screen_width/2) - (setup.wall_width/2) + (setup.screen_width/2);
        float center_y = (setup.wall_height/2) - setup.wall_pos_y; //+ (setup.screen_height/2) - (setup.screen_height/2)
        Vector3 screen_pos = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
        Vector3 world_pos = Camera.main.ScreenToWorldPoint(new Vector3(center_x, center_y, screen_pos.z));
        Camera.main.transform.position = world_pos;
        Debug.Log("P"+PhotonNetwork.LocalPlayer.ActorNumber+" has cam center "+Camera.main.transform.position+"::"+world_pos+" instead of "+old_pos+"  and scale was "+scale);
        //fixing screen resolution
        Screen.fullScreen = setup.full_screen;
        if(Screen.fullScreen){
            setup.screen_width = Screen.width;
            setup.screen_height = Screen.height;
        } else {
            Screen.SetResolution( (int)setup.screen_width, (int)setup.screen_height, false );
        }

        //now setting the possible name
        gameObject.name = "Participant";
        Debug.Log(gameObject.name+"has well been initialized");
        initialized = true;
    }
}
