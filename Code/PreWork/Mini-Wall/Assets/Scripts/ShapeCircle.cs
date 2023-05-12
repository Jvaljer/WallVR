using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShapeCircle : MonoBehaviourPun {

    private bool dragged = false;

    //setters
    public void Pick(){
        dragged = true;
    }
    public void Drop(){
        dragged = false;
    }
    
    //getters
    public bool IsDragged(){
        return dragged;
    }

    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }
}
