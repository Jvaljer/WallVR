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
    private GameObject square_prefab;

    //some predicates
    public bool prog_run { get; set; } = false;
    public bool ready_to_init_part { get; set; } = false;

    [PunRPC]
    public void Initialize(){
        //Debug.LogError("Operator is Initializing");
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        network = GameObject.Find("ScriptManager").GetComponent<NetworkHandler>();
        if(photonView.IsMine){
            //setting the screen resolution only for myself
            Screen.fullScreen = setup.full_screen;
            if(Screen.fullScreen){
                setup.screen_width = Screen.width;
                setup.screen_height = Screen.height;
            } else {
                Screen.SetResolution( (int)setup.screen_width, (int)setup.screen_height, false );
            }
        }
        input_handler = gameObject.GetComponent<InputHandler>();
        //Debug.Log("Input Handler initializing from Ope RPC");
        input_handler.InitalizeIH();
        //Debug.LogError("Calling the OpeInit Statement : "+PhotonNetwork.LocalPlayer.ActorNumber);
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
}
