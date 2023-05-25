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

    //possible shapes prefab
    private GameObject circle_prefab;
    private GameObject square_prefab;

    //some predicates
    public bool prog_run { get; set; } = false;
    public bool ready_to_init_part { get; set; } = false;

    [PunRPC]
    public void InitializeRPC(){
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        network = GameObject.Find("ScriptManager").GetComponent<NetworkHandler>();
        if(photonView.IsMine){
            Log("operator InitializeRPC for "+PhotonNetwork.LocalPlayer.ActorNumber+" (myself)");
            //setting the screen resolution only for myself
            Screen.fullScreen = setup.full_screen;
            if(Screen.fullScreen){
                setup.screen_width = Screen.width;
                setup.screen_height = Screen.height;
            } else {
                Screen.SetResolution( (int)setup.screen_width, (int)setup.screen_height, false );
            }
        } else {
            Log("operator InitializeRPC for "+PhotonNetwork.LocalPlayer.ActorNumber+" (other)"); 
        }
        input_handler = gameObject.GetComponent<InputHandler>();
        Log("starting program initialization for "+PhotonNetwork.LocalPlayer.ActorNumber);
        input_handler.InitalizeIH();
        network.OperatorInitialized();
    }

    [PunRPC]
    public void InstantiateShape(string category, Vector3 pos){
        switch (category){
            case "circle":
                break;
            case "square":
                square_prefab = PhotonNetwork.InstantiateRoomObject("Square", pos, Quaternion.identity);
                square_prefab.GetComponent<PhotonView>().RPC("MoveRPC", RpcTarget.AllBuffered, pos);
                break;
            default:
                break;
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