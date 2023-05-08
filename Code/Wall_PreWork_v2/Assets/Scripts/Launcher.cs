using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    //menu attributes (Unity IDE)
    public GameObject menu;
    //all different views (instantiated on Unity IDE)
    public GameObject ope_view;
    public GameObject upleft_view;
    public GameObject upright_view;
    public GameObject downleft_view;
    public GameObject downright_view;

    //photon room attributes
    private byte max_in_room = 5;
    private RoomOptions room;

    //server's state predicates
    private bool master_joined;

    //Awake method from Unity (called before ANYTHING else)
    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //Connect method which tells depending on the situations what role the new user shall have
    public void Connect(){
        Debug.Log("Connecting ...");
        PhotonNetwork.ConnectUsingSettings();
    }

    //Photon Unity Networking CallBacks (called whenever the related event happens)

    public override void OnConnectedToMaster(){
        //this is called whenever the server is connected to the master (the first user to join)
        Debug.Log("Connecting to Master ... (launcher)");
        base.OnConnectedToMaster();

        //and so we wanna instantiate the room, as there's no room created yet
        room = new RoomOptions{MaxPlayers=max_in_room, IsVisible=true, IsOpen=true};
        PhotonNetwork.JoinOrCreateRoom("Room", room, TypedLobby.Default);
        master_joined = true;
    }

    public override void OnJoinedRoom(){
        //this is called whenever an user enters the room
        Debug.Log("Joining the Room ... (launcher)");
        base.OnJoinedRoom();

        //when an user joins the room, we wanna instantiate the corresponding room
        menu.SetActive(false);
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            Debug.Log("User is master");
            ope_view.SetActive(true);
        } else {
            switch (PhotonNetwork.LocalPlayer.ActorNumber){
                case 2:
                    Debug.Log("User is up left");
                    upleft_view.SetActive(true);
                    break;
                case 3:
                    Debug.Log("User is up right");
                    upright_view.SetActive(true);
                    break;
                case 4:
                    Debug.Log("User is down left");
                    downleft_view.SetActive(true);
                    break;
                case 5:
                    Debug.Log("User is down right");
                    downright_view.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
