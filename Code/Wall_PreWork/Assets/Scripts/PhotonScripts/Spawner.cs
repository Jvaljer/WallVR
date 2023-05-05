using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviourPunCallbacks {
    private GameObject up_left;
    private GameObject up_right;
    private GameObject down_left;
    private GameObject down_right;
    private GameObject user_prefab;

    //scene attributes
    public GameObject scene;
    private Scene scene_script;
    public GameObject screens;
    public GameObject ctrl_pane;

    //Unity start method 
    public void Start(){
        Debug.Log("Starting the Spawner class");
        //we first need to instantiate all the dependencies
    }

    //this callbacks is called whenever someone joins the room
    public override void OnJoinedRoom(){
        Debug.Log("Spawner callbacks to 'OnJoinedRoom'");
        base.OnJoinedRoom();
        //so when an user joins the room we then wanna instantiate the 4 screens entities
        user_prefab = PhotonNetwork.Instantiate("Prefabs/User", ctrl_pane.transform.position, transform.rotation);
        up_left = PhotonNetwork.Instantiate("Prefabs/PhotonScreen", screens.transform.GetChild(0).position, screens.transform.GetChild(0).rotation);
        up_right = PhotonNetwork.Instantiate("Prefabs/PhotonScreen", screens.transform.GetChild(1).position, screens.transform.GetChild(1).rotation);
        down_left = PhotonNetwork.Instantiate("Prefabs/PhotonScreen", screens.transform.GetChild(2).position, screens.transform.GetChild(2).rotation);
        down_right = PhotonNetwork.Instantiate("Prefabs/PhotonScreen", screens.transform.GetChild(3).position, screens.transform.GetChild(3).rotation);
    }

    //this other callback is called whenever someone lefts the room
    public override void OnLeftRoom(){
        base.OnLeftRoom();
        PhotonNetwork.Destroy(user_prefab);
        PhotonNetwork.Destroy(up_left);
        PhotonNetwork.Destroy(up_right);
        PhotonNetwork.Destroy(down_left);
        PhotonNetwork.Destroy(down_right);
    }
}