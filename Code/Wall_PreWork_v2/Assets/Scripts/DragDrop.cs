using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragDrop : MonoBehaviourPun {
    //program operator
    private GameObject ope;
    //currently being dragged predicate
    private bool dragging = false;
    private Vector3 offset;
    //object can or not be dragged dropped 
    private bool enabled = false;

    public void Initialize(){
        ope = GameObject.Find("Operator");
    }
    public void Disable(){
        enabled = false;
    }
    public void Enable(){
        enabled = true;
    }

    public void OnMouseDown(){
        if(enabled){
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
        }
    }

    public void OnMouseUp(){
        if(enabled){
            dragging = false;
        }
    }

    public void OnMouseDrag(){
        if (dragging && enabled){
            Vector3 cur_screen_point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset.z);
            Vector3 cur_position = Camera.main.ScreenToWorldPoint(cur_screen_point) + offset;
            transform.position = cur_position;
            ope.GetComponent<PhotonView>().RPC("MoveShape", RpcTarget.AllBuffered, cur_position);
        }
    }
}
