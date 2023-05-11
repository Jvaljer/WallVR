using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    //parameters
    private string param_r;
    private float param_x;
    private float param_y;

    //main (and only) room camera
    public Camera cam;

    //launch menu from Unity IDE
    public GameObject launch_interface;
    public GameObject join_btn;

    //Photon Room
    private RoomOptions room;
    private byte max_users = 5;

    //All Prefabs
    public GameObject ope_prefab;
    public GameObject part_prefab;
    public GameObject shape_prefab;

    

    //Awake method from unity (which is called before anything else)
    public void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //must upgrade to be more like the actual wall
    /*public void Start(){
        string[] args = System.Environment.GetCommandLineArgs();
        for(int i=0; i<args.Length; i++){
            if(args[i]=="-r"){
                param_r = args[i+1];
            } else if(args[i]=="-x"){
                param_x = int.Parse(args[i+1]);
            } else if(args[i]=="-y"){
                param_y = int.Parse(args[i+1]);
            }
        }
        //Connect();
    } */

    //all Photon Callbacks
    public override void OnConnectedToMaster(){
        base.OnConnectedToMaster();

        room = new RoomOptions{MaxPlayers=max_users, IsVisible=true, IsOpen=true};
        PhotonNetwork.JoinOrCreateRoom("Room", room, TypedLobby.Default);
    }

    public override void OnJoinedRoom(){
        base.OnJoinedRoom();

        launch_interface.SetActive(false);
        if(PhotonNetwork.LocalPlayer.IsMasterClient){ //param_r=="m"
            //if the current user is MasterClient
            Debug.Log("user is master");

            ope_prefab = PhotonNetwork.Instantiate("Operator", transform.position, transform.rotation);
            ope_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "Operator");

            shape_prefab = PhotonNetwork.Instantiate("Circle", transform.position, transform.rotation);
            shape_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, "Circle");
        } else { //param_r=="c"
            Debug.Log("user is client");
            //if he's not then we'll give him a specific role
            /*if(param_x >=0f && param_x <= 1f && param_y >=0 && param_y <=1f){
                part_prefab = PhotonNetwork.Instantiate("Participant", transform.position, transform.rotation);
                part_prefab.GetComponent<PhotonView>().RPC("RpcInitialize", RpcTarget.AllBuffered, param_x, param_y);
            } */
            string part_id = "";
            switch (PhotonNetwork.LocalPlayer.ActorNumber){
                case 2:
                    part_id = "UpLeft";
                    break;
                case 3:
                    part_id = "UpRight";
                    break;
                case 4:
                    part_id = "DownLeft";
                    break;
                case 5:
                    part_id = "DownRight";
                    break;
            }
            Debug.Log("User is "+part_id);
            part_prefab = PhotonNetwork.Instantiate("Participant", transform.position, transform.rotation);
            part_prefab.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, part_id);
        }
    }

    //public override void OnPlayerEnteredRoom(Player newPlayer){
    //    base.OnPlayerEnteredRoom(newPlayer);
    //}

    //Connect method 
    public void Connect(){
        join_btn.GetComponent<Button>().interactable = false;
        PhotonNetwork.NickName = System.DateTime.Now.Ticks.ToString();
        PhotonNetwork.ConnectUsingSettings();
    }
}
