using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Circle : MonoBehaviourPun {

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
    public void NormName(int n){
        gameObject.name = "Circle "+ (n.ToString());
    }
}