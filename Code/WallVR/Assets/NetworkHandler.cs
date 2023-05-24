using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkHandler : MonoBehaviourPunCallbacks {
    //referenced Setup
    public Setup setup;

    //photon room
    private RoomOptions room_opt;
    private byte max_in_room = 11; //max with Pun FREE (but we need only 11 without VR)
    public int current_in_room { get; private set; }
    public bool ope_joined { get; private set; } = false;

    //all used prefabs
    public GameObject ope_prefab;
    public GameObject part_2d_prefab;
    public GameObject part_vr_prefab;
    public GameObject shape1_prefab;
    public GameObject shape2_prefab;

    //LocalParticipant & Cameras
    private GameObject cur_part;

    public void Awake(){
        //syncing the scenes
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster(){
        base.OnConnectedToMaster();
        //creating the room
        room_opt = new RoomOptions{MaxPlayers=max_in_room, IsVisible=true, IsOpen=true};
        PhotonNetwork.JoinOrCreateRoom("Room", room_opt, TypedLobby.Default);
    }

    public override void OnCreatedRoom(){
        base.OnCreatedRoom();
        //user creates -> MC & init cpt
        current_in_room = 0;
        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
    }

    public override void OnJoinedRoom(){
        //triggered for the brand newcomer only
        base.OnJoinedRoom();
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        if(setup.is_master){
            //if master then instantiate operator
            ope_prefab = PhotonNetwork.Instantiate("Operator", transform.position, transform.rotation);

            shape1_prefab = PhotonNetwork.InstantiateRoomObject("Circle", Vector3.zero, Quaternion.identity);
            Shape sh1_ctrl = shape1_prefab.GetComponent<Shape>();
            sh1_ctrl.Categorize("circle");
            sh1_ctrl.SetSize(shape1_prefab.transform.localScale.x);
            sh1_ctrl.PositionOn(Vector3.zero);
        } else {
            //else instantiate participant
            if(setup.is_vr){
                part_vr_prefab = PhotonNetwork.Instantiate("Participant 2D", transform.position, transform.rotation);
                setup.own_cam = part_vr_prefab.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Camera>();
                GameObject.Find("2D Camera").SetActive(false);
                part_2d_prefab.GetComponent<Participant>().NetworkStart(setup);
                cur_part = part_vr_prefab;
            } else {
                part_2d_prefab = PhotonNetwork.Instantiate("Participant VR", transform.position, transform.rotation);
                setup.own_cam = GameObject.Find("2D Camera").GetComponent<Camera>();
                part_2d_prefab.GetComponent<Participant>().NetworkStart(setup);
                cur_part = part_2d_prefab;
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        base.OnPlayerLeftRoom(otherPlayer);
        cur_part.GetComponent<PhotonView>().RPC("SomeoneLeft", RpcTarget.AllBuffered, otherPlayer.ActorNumber);
    }

    public void OperatorInitialized(){
        if(!PhotonNetwork.IsMasterClient){
            cur_part.GetComponent<PhotonView>().RPC("OperatorStartedRPC", RpcTarget.AllBuffered);
        }
    }

    public void Connect(){
        PhotonNetwork.NickName = System.DateTime.Now.Ticks.ToString();
        PhotonNetwork.ConnectUsingSettings();
    }
}
