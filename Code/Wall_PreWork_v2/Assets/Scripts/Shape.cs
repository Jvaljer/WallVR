using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//this gameobject is located in the Operator view
public class Shape : MonoBehaviourPun {
    //unity gameobjects
    private GameObject ope;

    // Start is called before the first frame update
    void Start(){
        //nothing to initialize yet
    }

    // Update is called once per frame
    void Update(){
        //nothing to update yet
    }

    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }
}
