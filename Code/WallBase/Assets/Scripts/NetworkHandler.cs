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
    private byte max_in_room = 11; //max with Pun FREE (but we need only 11)
    private bool all_there;
    public int current_in_room { get; private set; }
    public bool ope_joined { get; private set; } = false;

    //prefabs
    public GameObject ope_prefab;
    public GameObject part_prefab;
    public GameObject shape1_prefab;

    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster(){
        Debug.Log("Connected to Master");
        base.OnConnectedToMaster();
        current_in_room = 0;
        //creating the room
        room_opt = new RoomOptions{MaxPlayers=max_in_room, IsVisible=true, IsOpen=true};
        PhotonNetwork.JoinOrCreateRoom("Room", room_opt, TypedLobby.Default);
    }

    public override void OnJoinedRoom(){
        //triggered for the just joining entity only
        Debug.Log("OnJoinedRoom");
        base.OnJoinedRoom();
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        if(PhotonNetwork.IsMasterClient){
            Debug.Log("OnJoinedRoom -> IsMasterClient");
            //if master then instantiate operator
            ope_prefab = PhotonNetwork.Instantiate("Operator", transform.position, transform.rotation);
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            shape1_prefab = PhotonNetwork.InstantiateRoomObject("Circle", Vector3.zero, Quaternion.identity);
        } else {
            Debug.Log("OnJoinedRoom -> !IsMasterClient");
            //else instantiate participant
            part_prefab = PhotonNetwork.Instantiate("Participant", transform.position, transform.rotation);
            part_prefab.GetComponent<Participant>().NetworkStart(setup);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.Log("OnPlayerEnteredRoom");
        base.OnPlayerEnteredRoom(newPlayer);
        if(PhotonNetwork.IsMasterClient){
            //if i'm master then test some stuff
            if(PhotonNetwork.CurrentRoom.PlayerCount==(setup.part_cnt +1)){ //all parts + master
                ope_prefab.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.AllBuffered);
            }
        }
        //else I don't give a fuck
    }
    public override void OnPlayerLeftRoom(Player otherPlayer){
        base.OnPlayerLeftRoom(otherPlayer);
        part_prefab.GetComponent<PhotonView>().RPC("SomeoneLeft", RpcTarget.AllBuffered, otherPlayer.ActorNumber);
    }

    public void Connect(){
        Debug.Log("NetworkHandler -> Connect");
        PhotonNetwork.NickName = System.DateTime.Now.Ticks.ToString();
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OperatorInitialized(){
        Debug.LogError("Operator has initialized so now our turn : "+PhotonNetwork.LocalPlayer.ActorNumber);
        if(!PhotonNetwork.IsMasterClient){
            part_prefab.GetComponent<PhotonView>().RPC("OperatorStartedRPC", RpcTarget.AllBuffered);
        }
    }
}
