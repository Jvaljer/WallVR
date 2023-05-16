using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 

public class Operator : MonoBehaviourPun {

    //referenced setup & net manag
    private Setup setup;
    private NetworkHandler network;
    private InputHandler input_handler;

    //some predicates
    public bool prog_run { get; set; } = false;

    void Start(){
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        network = GameObject.Find("ScriptManager").GetComponent<NetworkHandler>();
        input_handler = gameObject.GetComponent<InputHandler>();

        if(photonView.IsMine){
            if(setup!=null){
                Init();
            }
        }
    }

    public void Init(){
        //nothing to really set up, as the camera is just the whole scene without any zoom or stuff
        gameObject.name = "Operator";
        //fixing screen resolution
        Screen.fullScreen = setup.full_screen;
        if(Screen.fullScreen){
            setup.screen_width = Screen.width;
            setup.screen_height = Screen.height;
        } else {
            Screen.SetResolution( (int)setup.screen_width, (int)setup.screen_height, false );
        }
    }

    [PunRPC]
    public void ReadyToStart(){
        Debug.Log("operator is ready");
    }
}
