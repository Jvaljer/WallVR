using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Operator : MonoBehaviourPun {
    private GameObject circle;
    private Vector3 prev_pos;
    private bool shape_moving = false;
    private bool init = true;
    private bool run = false;

    private GameObject upleft;
    private GameObject upright;
    private GameObject downleft;
    private GameObject downright;

    public void Update(){
        //un-comment to test drag drop
        /* circle = GameObject.Find("Circle");
        prev_pos = circle.transform.position;
        DragDrop();
        if(circle.transform.position != prev_pos){
        Debug.Log("Moving dah shape");
        } */

        if(photonView.IsMine){
            Debug.Log("I am Operator");
            if(init){
                Debug.Log("must init");
                circle = GameObject.Find("Circle");
                init = circle==null;
            } else if(!run){
                //before we wanna start the program we wanna check if the 4 participants are well there
                CheckForParticipant();
            } else {
                Debug.Log("Program Is Running");
                //now the program can run
                prev_pos = circle.transform.position;
                DragDrop();
                if(circle.transform.position != prev_pos){
                    Vector3 nxt_pos = circle.transform.position;
                    shape_moving = true;
                } else {
                    shape_moving = false;
                }
            }
        }
    }
    
    public void DragDrop(){
        Debug.Log("DragDrop running");
        ShapeCircle shape = circle.GetComponent<ShapeCircle>();
        Vector3 click_start = Vector3.zero;
        Vector3 obj_start = Vector3.zero;

        if(Input.GetMouseButtonDown(0)){
            //if the left click is performed
            //then checks if it's inside the circle
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click.z = 0f;
            if(ClickInsideCircle(click)){
                click_start = Input.mousePosition;
                obj_start = circle.transform.position;
            }
        }
        if(Input.GetMouseButton(0)){
            if(!shape.IsDragged()){
                shape.Pick();
            }

            Vector3 mouse_delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - click_start;
            mouse_delta.z = 0f;
            Vector3 new_pos = obj_start + mouse_delta;
            circle.transform.position = new_pos;
            //maybe we wanna move the shape here ?
            photonView.RPC("MoveShapeOnScreen", RpcTarget.AllBuffered, new_pos);
        }
        if(Input.GetMouseButtonUp(0)){
            shape.Drop();
        }
    }

    public void CheckForParticipant(){
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

        run = upleft!=null && upright!=null && downleft!=null && downright!=null;
    }

    public bool ClickInsideCircle(Vector3 coord){
        float dist = (float)Math.Sqrt(Math.Pow(coord.x - circle.transform.position.x, 2) + Math.Pow(coord.y - circle.transform.position.y, 2));
        float rad = circle.transform.localScale.x/2;

        return dist <= rad;
    }

    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }

    [PunRPC]
    public void MoveShapeOnScreen(Vector3 pos){
        Debug.Log("moving the shape on screen : "+pos);
        GameObject.Find("Circle").transform.position = pos;
    }
}
