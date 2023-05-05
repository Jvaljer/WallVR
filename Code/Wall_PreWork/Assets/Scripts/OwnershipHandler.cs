using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OwnershipHandler : MonoBehaviourPun {

    public GameObject shape;

    private GameObject up_left;
    private GameObject up_right;
    private GameObject down_right;
    private GameObject down_left;

    private bool all_got = false;
    private bool shape_in_up_left = true;
    private bool shape_in_up_right = true;
    private bool shape_in_down_left = true;
    private bool shape_in_down_right = true;

    // Start is called before the first frame update
    void Start(){
        //nothing to initialize there
    }

    // Update is called once per frame
    void Update(){
        if(!all_got){
            Debug.Log("Searching em screen parts");
            FetchScreenPart();
        } else {
            ShapeLocalisation();
            Debug.Log("up_left : "+shape_in_up_left+" && up_right : "+shape_in_up_right+" && down_right : "+shape_in_down_right+" && down_left : "+shape_in_down_left);
        }
    }

    private void FetchScreenPart(){
        if(up_left==null){
            up_left = GameObject.Find("UpLeft(Clone)");
        }
        if(up_right==null){
            up_right = GameObject.Find("UpRight(Clone)");
        }
        if(down_right==null){
            down_right = GameObject.Find("DownRight(Clone)");
        }
        if(down_left==null){
            down_left = GameObject.Find("DownLeft(Clone)");
        }

        all_got = up_left!=null && up_right!=null && down_right!=null && down_left!=null;
    }

    private void ShapeLocalisation(){
        //all true by default (worst case predicted)
        shape_in_down_left = true;
        shape_in_down_right = true;
        shape_in_up_left = true;
        shape_in_up_right = true;
        //first we wanna get the shape's center & diameter
        Vector2 coord = new Vector2(shape.transform.position.x, shape.transform.position.y);
        float x = coord.x;
        float y = coord.y;

        float d = shape.transform.localScale.x;
        float mid_x = -4.5f;

        if(x-d <=mid_x && x+d >mid_x && y-d<=0 && y+d>0){
            Debug.Log("CENTER");
            return;
        } else if(x+d <=mid_x){
            if(y-d <=0){
                if(y+d <=0){
                    //DOWNLEFT
                    shape_in_down_right = false;
                    shape_in_up_left = false;
                    shape_in_up_right = false;
                    Debug.Log("DOWNLEFT");
                    return;
                } else {
                    //DOWNLEFT + UPLEFT
                    shape_in_down_right = false;
                    shape_in_up_right = false;
                    Debug.Log("DOWNLEFT + UPLEFT");
                    return;
                }
            } else {
                //UPLEFT
                shape_in_down_right = false;
                shape_in_down_left = false;
                shape_in_up_right = false;
                Debug.Log("UPLEFT");
                return;
            }

        } else {
            if(x-d <=mid_x){
                if(y+d <=0){
                    //DOWNRIGHT + DOWNLEFT
                    shape_in_up_left = false;
                    shape_in_up_right = false;
                    Debug.Log("DOWNRIGHT + DOWNLEFT");
                    return;
                } else {
                    //UPLEFT + UPRIGHT
                    shape_in_down_left = false;
                    shape_in_down_right = false;
                    Debug.Log("UPLEFT + UPRIGHT");
                    return;
                }
            } else {
                if(y+d <=0){
                    //DOWNRIGHT
                    shape_in_down_left = false;
                    shape_in_up_right = false;
                    shape_in_up_left = false;
                    Debug.Log("DOWNRIGHT");
                    return;
                } else {
                    if(y-d <=0){
                        //DOWNRIGHT + UPRIGHT
                        shape_in_down_left = false;
                        shape_in_up_left = false;
                        Debug.Log("DOWNRIGHT + UPRIGHT");
                        return;
                    } else {
                        //UPRIGHT
                        shape_in_down_left = false;
                        shape_in_up_left = false;
                        shape_in_down_right = false;
                        Debug.Log("UPRIGHT");
                        return;
                    }
                }
            }
        }
    }
}
