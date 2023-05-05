using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//photon necessities
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    //unity attributes
    private GameObject scene;
    private GameObject launch_interface;
    private Scene scene_script;
    private int[] scene_parameters;

    //photon attributes
    private byte max_in_room = 5; //we want to have the user & the 4 part of the screen
    private RoomOptions room_options;

    //Awake Method from Unity 
    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //Unity's Start method
    public void Start(){
        //we first need to instantiate all the dependencies
        launch_interface = GameObject.Find("LauncherCanvas");
        scene =  GameObject.Find("Scene");
        scene_script = GameObject.Find("Scene").GetComponent<Scene>();
        scene.SetActive(false);
        scene_script.NotAccessible();
    }

    //Connect function for the connection to the server
    public void Connect(){
        Debug.Log("Connecting ...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void InitializeSceneParameters(){
        //must implement 
        return;
    }

    //override Photon CALLBACKS (below)

    //this method is called whenever there's a connection onto the master client of the server (first user to join by default)
    public override void OnConnectedToMaster(){
        Debug.Log("There is a connection to the master client");
        base.OnConnectedToMaster();

        //if we're the master we wanna create the room
        room_options = new RoomOptions { MaxPlayers = max_in_room, IsVisible = true, IsOpen = true };
        //then we wanna Join it
        PhotonNetwork.JoinOrCreateRoom("App Room", room_options, TypedLobby.Default);
    }

    //this method is called whenever there's someone who joins the room
    public override void OnJoinedRoom(){
        Debug.Log("Someone has joined the room");
        base.OnJoinedRoom();

        scene.SetActive(true);
        scene_script.Accessible();

        //here we wanna initialize the scene parameters
        InitializeSceneParameters();
        scene_script.Initialize(scene_parameters);
        launch_interface.SetActive(false);
    }
    
    //this method is called whenever there's a player that enters the room
    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.Log("A player entered the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}