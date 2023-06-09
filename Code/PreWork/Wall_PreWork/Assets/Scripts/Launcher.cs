using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    //unity attributes
    public GameObject menu;
    public GameObject ctrl_pane;
    public GameObject screen;
    public GameObject indicator;
    private Vector3 screen_center = new Vector3(-4.5f, 0f, 0f);
    private Vector3 pane_center = new Vector3(6.5f, 0f, 10f);

    //phton attributes
    private byte max_in_room = 5;
    private RoomOptions room_options;

    //prefabs
    private GameObject up_right;
    private GameObject up_left;
    private GameObject down_right;
    private GameObject down_left;
    private GameObject user;

    //shapes attributes
    private GameObject circle;
    private GameObject square;


    //Awake method from unity (called before anything else)
    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
        ctrl_pane.SetActive(false);
        screen.SetActive(false);
        indicator.SetActive(false);
        menu.SetActive(false);
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

        //setting active or not all main gameobjects
        transform.gameObject.SetActive(false);
        ctrl_pane.SetActive(true);
        screen.SetActive(true);
        indicator.SetActive(true);

        //now instantiating all of the photon entities we'll need
        user = PhotonNetwork.Instantiate("User", ctrl_pane.transform.position, ctrl_pane.transform.rotation);
        up_left = PhotonNetwork.Instantiate("UpLeft", screen.transform.GetChild(0).position, screen.transform.GetChild(0).rotation);
        up_right = PhotonNetwork.Instantiate("UpRight", screen.transform.GetChild(1).position, screen.transform.GetChild(1).rotation);
        down_left = PhotonNetwork.Instantiate("DownLeft", screen.transform.GetChild(3).position, screen.transform.GetChild(3).rotation);
        down_right = PhotonNetwork.Instantiate("DownRight", screen.transform.GetChild(2).position, screen.transform.GetChild(2).rotation);
 
        //instantiating a circle
        circle = PhotonNetwork.InstantiateRoomObject("ShapeCircle", new Vector3(0,0,0), new Quaternion(0,0,0,0));
        circle.transform.GetChild(0).gameObject.transform.position = screen_center;
        circle.transform.GetChild(1).gameObject.transform.position = pane_center;

        //instantiating a square 
        square = PhotonNetwork.InstantiateRoomObject("ShapeSquare", new Vector3(0,0,0), new Quaternion(0,0,0,0));
        square.transform.GetChild(0).gameObject.transform.position = new Vector3(screen_center.x-2.5f, screen_center.y+2.5f,0f);
        square.transform.GetChild(1).gameObject.transform.position = new Vector3(pane_center.x-1.5f, pane_center.y+1.5f, 10f);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.Log("Player entered room (launcher)");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
