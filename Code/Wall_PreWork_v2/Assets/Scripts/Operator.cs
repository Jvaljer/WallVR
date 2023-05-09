using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Operator : MonoBehaviourPun {
    //shapes attributes
    private GameObject circle;

    //photon participants entity
    private GameObject upleft;
    private GameObject upright;
    private GameObject downleft;
    private GameObject downright;

    //ready to start predicate
    private bool ready = false;

    // Start is called before the first frame update
    void Start(){
        //nothing to initialize yet
    }

    // Update is called once per frame
    void Update(){
        //we wanna fetch all the screen parts before we allow the program to start
        if(!ready){
            FetchForParticipants();
        }
    }

    public void FetchForParticipants(){
        if(upleft==null){
            upleft = GameObject.Find("UpLeft");
        }
        if(upright==null){
            upright = GameObject.Find("UpRight");
        }
        if(downleft==null){
            downleft = GameObject.Find("DownLeft");
        }
        if(downright==null){
            downright = GameObject.Find("DownRight");
        }
    }
    public void FetchForShapes(){
        if(circle==null){
            circle = GameObject.Find("Circle");
        }
    }

    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }
}
