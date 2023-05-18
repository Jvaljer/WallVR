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

    //some predicates
    public bool prog_run { get; set; } = false;
    public bool ready_to_init_part { get; set; } = false;

    [PunRPC]
    public void Initialize(){
        Debug.Log("Operator is Initializing");
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        if(photonView.IsMine){
            //setting the screen resolution only for myself
            Screen.fullScreen = setup.full_screen;
            if(Screen.fullScreen){
                setup.screen_width = Screen.width;
                setup.screen_height = Screen.height;
            } else {
                Screen.SetResolution( (int)setup.screen_width, (int)setup.screen_height, false );
            }

            //initializing a new shape
            circle_prefab = PhotonNetwork.InstantiateRoomObject("Circle", Vector3.zero, Quaternion.identity);
        }
        input_handler = gameObject.GetComponent<InputHandler>();
        Debug.Log("Input Handler initializing from Ope RPC");
        Debug.LogError("got IH -> "+(input_handler!=null));
        input_handler.InitalizeIH();
        Debug.Log("Initializing the actual participant");
        ready_to_init_part = true;
    }
}
