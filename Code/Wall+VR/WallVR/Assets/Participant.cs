using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Participant : MonoBehaviourPun {
    //referenced setup
    private Setup setup;
    private Camera camera;

    public void NetworkStart(Setup stp){
        setup = stp;
        camera = setup.own_cam; //!is_vr -> Camera.main still referring to the wanted cam
        if(photonView.IsMine){
            Log("Network Start for Participant "+PhotonNetwork.LocalPlayer.ActorNumber+" (myself)");
            InitMyAttributes();
        } else {
            Log("Network Start for Participant "+PhotonNetwork.LocalPlayer.ActorNumber+" (other)");
        }
    }

    public void InitMyAttributes(){
        //fixing screen resolution
        Screen.fullScreen = setup.full_screen;
        if(Screen.fullScreen){
            setup.screen_width = Screen.width;
            setup.screen_height = Screen.height;
        } else {
            Screen.SetResolution( (int)setup.screen_width, (int)setup.screen_height, false );
        }
        
        if(setup.is_vr){
            Log("I'm VR");
            //must implement
        } else {
            Log("I'm 2D");
            //setting the camera
            Vector3 old_pos = Camera.main.transform.position;
            Vector3 scale = Camera.main.transform.localScale;

            float center_x = setup.x_pos + (setup.screen_width/2) - (setup.wall.Width()/2) + (setup.screen_width/2);
            float center_y = (setup.wall.Height()/2) - setup.y_pos + (setup.screen_height/2) - (setup.screen_height/2);
            Vector3 screen_pos = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
            Vector3 world_pos = Camera.main.ScreenToWorldPoint(new Vector3(center_x, center_y, screen_pos.z));
            Camera.main.transform.position = world_pos;
        }
    }

    [PunRPC]
    public void OperatorStartedRPC(){
        if(photonView.IsMine){
            Log("OperatorStartedRPC for "+PhotonNetwork.LocalPlayer.ActorNumber+" (myself)");
            GameObject.Find("Operator(Clone)").GetComponent<InputHandler>().ParticipantReady();
        }
        if(PhotonNetwork.IsMasterClient){
            Log("OperatorStartedRPC for master");
        }
    }

    [PunRPC]
    public void SomeoneLeft(int pv){
        if(pv==1){ //in this program logic, 1 is the master client (operator)
            Application.Quit();
        }
    }

    public void Log(string str){
        if(setup.is_vr){
            //simple debugging as under editor for tests
            Debug.Log(str);
        } else {
            //Error Log to make it visible from standalone
            Debug.LogError(str);
        }
    }
}