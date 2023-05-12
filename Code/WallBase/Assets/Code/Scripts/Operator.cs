using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 

public class Operator : MonoBehaviourPun {

    //referenced setup & net manag
    private Setup setup;
    private NetworkManager network_manager;

    //some predicates
    private bool run;

    void Start(){
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        network_manager = GameObject.Find("ScriptManager").GetComponent<NetworkManager>();
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
            run = (setup.part_cnt==network_manager.current_in_room);
            if(run){
                Debug.Log("Program can run !!!");
                photonView.RPC("RunRPC", RpcTarget.AllBuffered);
            }
        }
    }

    public void Init(){
        //nothing to really set up, as the camera is just the whole scene without any zoom or stuff
        gameObject.name = "Operator";
    }

    [PunRPC]
    public void RunRPC(){
        Debug.Log("Operator said run it");
        run = true;
    }
}
