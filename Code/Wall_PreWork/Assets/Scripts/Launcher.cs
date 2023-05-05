using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    //unity attributes
    public GameObject ctrl_pane;
    public GameObject screen;

    //phton attributes
    private byte max_in_room = 5;
    private RoomOptions room_options;

    //Awake method from unity (called before anything else)
    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
        ctrl_pane.SetActive(false);
        screen.SetActive(false);
    }

    //Connect method called whenever the user starts the app
    public void Connect(){
        Debug.Log("Connecting ...");
        PhotonNetwork.ConnectUsingSettings();
    }

    //Connection callbacks
    public override void OnConnectedToMaster(){
        Debug.Log("Connect to Master (launcher)");
        base.OnConnectedToMaster();

        room_options = new RoomOptions{MaxPlayers=max_in_room, IsVisible=true, IsOpen=true};
        PhotonNetwork.JoinOrCreateRoom("Room", room_options, TypedLobby.Default);
    }

    public override void OnJoinedRoom(){
        Debug.Log("Room has been joined (launcher)");
        base.OnJoinedRoom();

        transform.gameObject.SetActive(false);
        ctrl_pane.SetActive(true);
        screen.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.Log("Player entered room (launcher)");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
