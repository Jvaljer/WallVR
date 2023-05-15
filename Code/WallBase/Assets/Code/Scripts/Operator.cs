using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 

public class Operator : MonoBehaviourPun {

    //referenced setup & net manag
    private Setup setup;
    private NetworkHandler network;

    //some predicates
    private bool run;

    void Start(){
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        network = GameObject.Find("ScriptManager").GetComponent<NetworkHandler>();
        Debug.Log("initalizing operator");
        if(photonView.IsMine){
            Debug.Log("And it's me");
            if(setup!=null){
                Debug.Log("initializing my attributes");
                Init();
            }   
        }
    }

    public void Update(){
        if(photonView.IsMine){ //if everything's okay only the 'PhotonNetwork.IsMaterClient==true' computer will launch this
            if(setup!=null){
                run = (setup.part_cnt==network.current_in_room);
                if(run){
                    Debug.Log("Program can run !!!");
                    photonView.RPC("RunRPC", RpcTarget.AllBuffered);
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

    [PunRPC]
    public void RunRPC(){
        Debug.Log("Operator said run it");
        run = true;
    }
}
