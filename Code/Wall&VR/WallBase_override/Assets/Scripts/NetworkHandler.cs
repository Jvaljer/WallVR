using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkHandler : MonoBehaviourPunCallbacks {
    //referenced setup
    public Setup setup;

    //photon room
    private RoomOptions room_opt;
    private byte max_in_room = 11; //max with Pun FREE (but we need only 11 without VR)
    public int current_in_room { get; private set; }
    public bool ope_joined { get; private set; } = false;

    //prefabs
    public GameObject ope_prefab;
    public GameObject part_prefab;
    public GameObject vr_prefab;
    public GameObject shape1_prefab;
    public GameObject shape2_prefab;

    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster(){
        base.OnConnectedToMaster();
        current_in_room = 0;
        //creating the room
        room_opt = new RoomOptions{MaxPlayers=max_in_room, IsVisible=true, IsOpen=true};
        PhotonNetwork.JoinOrCreateRoom("Room", room_opt, TypedLobby.Default);
    }

    public override void OnJoinedRoom(){
        //triggered for the just joining entity only
        base.OnJoinedRoom();
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        if(PhotonNetwork.IsMasterClient){
            //if master then instantiate operator
            ope_prefab = PhotonNetwork.Instantiate("Operator", transform.position, transform.rotation);
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            shape1_prefab = PhotonNetwork.InstantiateRoomObject("Circle", Vector3.zero, Quaternion.identity);
            Shape sh1_ctrl = shape1_prefab.GetComponent<Shape>();
            sh1_ctrl.Categorize("circle");
            sh1_ctrl.SetSize(shape1_prefab.transform.localScale.x);
            sh1_ctrl.PositionOn(Vector3.zero);
            if(setup.master_only){
                ope_prefab.GetComponent<PhotonView>().RPC("InitializeRPC", RpcTarget.AllBuffered);
            }
        } else {
            //else instantiate participant
            part_prefab = PhotonNetwork.Instantiate("Participant", transform.position, transform.rotation);
            part_prefab.GetComponent<Participant>().NetworkStart(setup);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        base.OnPlayerEnteredRoom(newPlayer);
        if(PhotonNetwork.IsMasterClient){
            //if i'm master then test some stuff
            if(PhotonNetwork.CurrentRoom.PlayerCount==(setup.part_cnt +1)){ //all parts + master
                ope_prefab.GetComponent<PhotonView>().RPC("InitializeRPC", RpcTarget.AllBuffered);
            }
        }
        //else I don't give a fuck
    }
    public override void OnPlayerLeftRoom(Player otherPlayer){
        base.OnPlayerLeftRoom(otherPlayer);
        part_prefab.GetComponent<PhotonView>().RPC("SomeoneLeft", RpcTarget.AllBuffered, otherPlayer.ActorNumber);
    }

    public void Connect(){
        PhotonNetwork.NickName = System.DateTime.Now.Ticks.ToString();
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OperatorInitialized(){
        if(!PhotonNetwork.IsMasterClient){
            part_prefab.GetComponent<PhotonView>().RPC("OperatorStartedRPC", RpcTarget.AllBuffered);
        }
    }
}
