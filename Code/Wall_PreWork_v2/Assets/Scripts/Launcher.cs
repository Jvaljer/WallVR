using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    //main camera
    public Camera camera;
    //other cameras
    public Camera UL_cam;
    public Camera UR_cam;
    public Camera DL_cam;
    public Camera DR_cam;

    //menu attributes (Unity IDE)
    public GameObject menu;
    public GameObject connect_btn;

    //all different views (instantiated on Unity IDE)
    public GameObject ope_view;
    public GameObject part_view;

    //all different prefabs (participant & shapes)
    public GameObject ope_prefab;
    public GameObject part_prefab;
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
        //menu.GetComponent<Animation>().Start();
        connect_btn.GetComponent<Button>().interactable = false;
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
        //menu.GetComponent<Animation>().Stop();

        //when an user joins the room, we wanna instantiate the corresponding view & the photon entity that comes along
        menu.SetActive(false);
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            Debug.Log("User is master");
            ope_view.SetActive(true);
            ope_prefab = PhotonNetwork.Instantiate("Operator", ope_view.transform.position, ope_view.transform.rotation);
            ope_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "Operator");
            //in this case we also wanna instantiate the shape
            circle_prefab = PhotonNetwork.InstantiateRoomObject("Circle", new Vector3(camera.transform.position.x, camera.transform.position.y, 0f), camera.transform.rotation);
            circle_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "Circle");
        } else {
            //Screen.SetResolution(560, 560, false);
            //camera.orthographicSize = 280f;
            part_view.SetActive(true);
            string part_id = "";
            switch (PhotonNetwork.LocalPlayer.ActorNumber){
                case 2:
                    Debug.Log("User is up left");
                    part_id = "UpLeft";
                    break;
                case 3:
                    Debug.Log("User is up right");
                    part_id = "UpRight";
                    break;
                case 4:
                    Debug.Log("User is down left");
                    part_id = "DownLeft";
                    break;
                case 5:
                    Debug.Log("User is down right");
                    part_id = "DownRight";
                    break;
                default:
                    break;
            }
            part_prefab = PhotonNetwork.Instantiate("ScreenPart", new Vector3(0,0,0), new Quaternion(0,0,0,0));
            part_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, part_id);
            EnablePartCam(part_id);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.Log("Player entered room (launcher)"+PhotonNetwork.LocalPlayer.ActorNumber);
        base.OnPlayerEnteredRoom(newPlayer);
    }

    //other methods

    public void EnablePartCam(string part){
        switch (part){
            case "UpLeft":
                UL_cam.gameObject.SetActive(true);
                camera.gameObject.SetActive(false);
                break;
            case "UpRight":
                UR_cam.gameObject.SetActive(true);
                camera.gameObject.SetActive(false);
                break;
            case "DownLeft":
                DL_cam.gameObject.SetActive(true);
                camera.gameObject.SetActive(false);
                break;
            case "DownRight":
                DR_cam.gameObject.SetActive(true);
                camera.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
