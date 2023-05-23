using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Participant : MonoBehaviourPun {
    //referenced setup
    private Setup setup;

    public void NetworkStart(Setup stp){
        //Debug.LogError("Network Start for Participant");
        setup = stp;
        if(photonView.IsMine){
            //Debug.Log("I am P"+PhotonNetwork.LocalPlayer.ActorNumber);
            InitMyAttributes();
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

        //setting the camera
        Vector3 old_pos = Camera.main.transform.position;
        Vector3 scale = Camera.main.transform.localScale;

        float center_x = setup.x_pos + (setup.screen_width/2) - (setup.wall.Width()/2) + (setup.screen_width/2);
        float center_y = (setup.wall.Height()/2) - setup.y_pos + (setup.screen_height/2) - (setup.screen_height/2);
        Vector3 screen_pos = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
        Vector3 world_pos = Camera.main.ScreenToWorldPoint(new Vector3(center_x, center_y, screen_pos.z));
        Camera.main.transform.position = world_pos;
        //Debug.Log("setup.x_pos-> "+setup.x_pos+"  setup.screen_width/2-> "+(setup.screen_width/2)+"  setup.wall.Width()/2-> "+(setup.wall.Width()/2)+"  := center_x-> "+center_x+"    screen_pos.x-> "+screen_pos.x+"   world_pos.x-> "+world_pos);

        //Debug.Log("P"+PhotonNetwork.LocalPlayer.ActorNumber+" has cam center "+Camera.main.transform.position+"::"+world_pos+" instead of "+old_pos+"  and scale was "+scale);
    }

    [PunRPC]
    public void OperatorStartedRPC(){
        string log = "Operator called Start RPC ";
        if(photonView.IsMine || PhotonNetwork.IsMasterClient){
            log += "for myself";
            GameObject.Find("Operator(Clone)").GetComponent<InputHandler>().ParticipantReady();
            //Debug.LogError("shape is null : "+(GameObject.Find("Circle(Clone)")==null));
        } else {
            log += "for another one";
        }
        //Debug.LogError(log);
    }

    [PunRPC]
    public void Operatorleft(){
        //Debug.Log("I see operator has left -> I QUIT");
        Application.Quit();
    }

    [PunRPC]
    public void SomeoneLeft(int pv){
        //Debug.Log("I see that someone left the program : "+pv);
        if(pv==1){ //in this program logic, 1 is the master client (operator)
            Application.Quit();
        }
    }
}
