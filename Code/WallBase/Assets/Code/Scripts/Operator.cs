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
        //input_handler = GameObject.Find("Operator").GetComponent<InputHandler>();
        if(photonView.IsMine){
            if(setup!=null){
                Init();
            }   
        }
    }

    public void Update(){
        if(photonView.IsMine){ //if everything's okay only the 'PhotonNetwork.IsMaterClient==true' computer will launch this
            if(setup!=null){
                Debug.Log("all joined : "+(setup.part_cnt==network.current_in_room));
                prog_run = (setup.part_cnt==network.current_in_room);
                if(prog_run){
                    //nothing to do ???
                    //wait for test n stuff -> ope will do so ?
                    //input_handler.Start();
                }
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
}
