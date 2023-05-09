using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    //main camera
    public Camera camera;
    //menu attributes (Unity IDE)
    public GameObject menu;

    //all different views (instantiated on Unity IDE)
    public GameObject ope_view;
    public GameObject upleft_view;
    public GameObject upright_view;
    public GameObject downleft_view;
    public GameObject downright_view;

    //all different prefabs (participant & shapes)
    public GameObject ope_prefab;
    public GameObject upleft_prefab;
    public GameObject upright_prefab;
    public GameObject downleft_prefab;
    public GameObject downright_prefab;
    public GameObject circle_prefab;

    //photon room attributes
    private byte max_in_room = 5;
    private RoomOptions room;

    //Awake method from Unity (called before ANYTHING else)
    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //Connect method which tells depending on the situations what role the new user shall have
    public void Connect(){
        Debug.Log("Connecting ...");
        PhotonNetwork.NickName = System.DateTime.Now.Ticks.ToString();
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
    }

    public override void OnJoinedRoom(){
        //this is called whenever an user enters the room
        Debug.Log("Joining the Room ... (launcher)");
        base.OnJoinedRoom();

        //when an user joins the room, we wanna instantiate the corresponding view & the photon entity that comes along
        menu.SetActive(false);
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            Debug.Log("User is master");
            ope_view.SetActive(true);
            ope_prefab = PhotonNetwork.Instantiate("Operator", ope_view.transform.position, ope_view.transform.rotation);
            ope_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "Operator");
            //in this case we also wanna instantiate the shape
            circle_prefab = PhotonNetwork.InstantiateRoomObject("Circle", ope_view.transform.position, ope_view.transform.rotation);
            circle_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "Circle");
        } else {
            Screen.SetResolution(560, 560, false);
            camera.orthographicSize = 280f;
            switch (PhotonNetwork.LocalPlayer.ActorNumber){
                case 2:
                    Debug.Log("User is up left");
                    upleft_view.SetActive(true);
                    upleft_prefab = PhotonNetwork.Instantiate("UpLeft", upleft_view.transform.position, upleft_view.transform.rotation);
                    upleft_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "UpLeft");
                    break;
                case 3:
                    Debug.Log("User is up right");
                    upright_view.SetActive(true);
                    upright_prefab = PhotonNetwork.Instantiate("UpRight", upright_view.transform.position, upright_view.transform.rotation);
                    upright_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "UpRight");
                    break;
                case 4:
                    Debug.Log("User is down left");
                    downleft_view.SetActive(true);
                    downleft_prefab = PhotonNetwork.Instantiate("DownLeft", downleft_view.transform.position, downleft_view.transform.rotation);
                    downleft_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "DownLeft");
                    break;
                case 5:
                    Debug.Log("User is down right");
                    downright_view.SetActive(true);
                    downright_prefab = PhotonNetwork.Instantiate("DownLeft", downright_view.transform.position, downright_view.transform.rotation);
                    downright_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "DownRight");   
                    break;
                default:
                    break;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.Log("Player entered room (launcher)");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
