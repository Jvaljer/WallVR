using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScreenPart : MonoBehaviourPun {
    //unity attributes
    private GameObject ope;
    private GameObject circle;

    //we want each screen to have its id which will allow the program to decide where to locate the shape
    private string loc_id;

    //owning the shape predicates
    private bool is_owner;

    // Update is called once per frame
    void Update(){
        if(circle==null){
            circle = GameObject.Find("Circle");
        }
    }
    
    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }

    [PunRPC]
    public void MoveCircle(Vector3 new_pos){
        circle.transform.position = new_pos;
    }
}
