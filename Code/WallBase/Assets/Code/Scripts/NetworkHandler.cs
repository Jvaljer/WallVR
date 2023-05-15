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
    public int current_in_room { get; private set; }

    //prefabs
    public GameObject ope_prefab;
    public GameObject part_prefab;

    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Update(){
        if(PhotonNetwork.IsMasterClient){ //setup.is_master
            if(PhotonNetwork.PlayerList.Length - 1 > current_in_room){
                Debug.Log("someone joined the room");
                current_in_room++;
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

        if(setup.is_master){ //PhotonNetwork.IsMasterClient
            Debug.Log("User is master");
            ope_prefab = PhotonNetwork.Instantiate("Operator", transform.position, transform.rotation);
        } else {
            Debug.Log("User is participant");
            part_prefab = PhotonNetwork.Instantiate("Participant", transform.position, transform.rotation);
        }
    }
    
}
