using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkHandler : MonoBehaviourPunCallbacks {
    //referenced setup
    public Setup setup;

    //photon room 
    private RoomOptions room;
    private byte max_in_room = 20; //max with Pun FREE (but we need only 11)
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

    public void Update(){
        if(PhotonNetwork.IsMasterClient && !all_there){ //setup.is_master
            if(PhotonNetwork.PlayerList.Length - 1 > current_in_room){
                Debug.Log("someone joined the room");
                current_in_room++;
                all_there = (current_in_room==setup.part_cnt);
            }
            if(all_there){
                photonView.RPC("ReadyToStart", RpcTarget.MasterClient);
            }
        }
    }

    public override void OnConnectedToMaster(){
        Debug.Log("Connected to Master");
        base.OnConnectedToMaster();
        current_in_room = 0;
        //creating the room
        room = new RoomOptions{MaxPlayers=max_in_room, IsVisible=true, IsOpen=true};
        PhotonNetwork.JoinOrCreateRoom("Room", room, TypedLobby.Default);
    }

    public override void OnJoinedRoom(){
        Debug.Log("Room Joined");
        base.OnJoinedRoom();

        if(PhotonNetwork.IsMasterClient){ //setup.is_master
            Debug.Log("User is master");
            ope_prefab = PhotonNetwork.Instantiate("Operator", transform.position, transform.rotation);
            ope_joined = true;
            shape1_prefab = PhotonNetwork.Instantiate("Circle", transform.position, transform.rotation);
            shape1_prefab.GetComponent<PhotonView>().RPC("NormName", RpcTarget.AllBuffered, 0);
        } else {
            Debug.Log("User is participant");
            part_prefab = PhotonNetwork.Instantiate("Participant", transform.position, transform.rotation);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        base.OnPlayerLeftRoom(otherPlayer);
        part_prefab.GetComponent<PhotonView>().RPC("SomeoneLeft", RpcTarget.AllBuffered, otherPlayer.ActorNumber);
        if(otherPlayer.ActorNumber==PhotonNetwork.MasterClient.ActorNumber){
            Debug.Log("The operator left -> Quit this bro");
            part_prefab.GetComponent<PhotonView>().RPC("Operatorleft", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void ReadyToStart(){
        Debug.Log("ready so...");
    }
    

}
