using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//this gameobject is located in the Operator view
public class Shape : MonoBehaviourPun {
    //unity gameobjects
    private GameObject ope;
    private bool dragdrop_enabled = false;

    // Start is called before the first frame update
    void Start(){
        ope = GameObject.Find("Operator");
    }

    // Update is called once per frame
    void Update(){
        if(ope==null){
            ope = GameObject.Find("Operator");
        } else {
            if(ope.GetComponent<Operator>().IsReady() && GameObject.Find("OperatorView")!=null){
                dragdrop_enabled = true;
            } else {
                dragdrop_enabled = false;
            }
        }
    }

    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }
}
