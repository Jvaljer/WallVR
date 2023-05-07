using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OwnershipHandler : MonoBehaviourPun {

    public GameObject indicator;
    private bool got_shape = false;
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

    // Update is called once per frame
    void Update(){
        if(!got_shape){
            FetchForShape();
        }
        if(!all_got){
            Debug.Log("Searching em screen parts");
            FetchScreenPart();
        } else if(got_shape){
            ShapeLocalisation();
            SetOwnership();
            //now just for the information, we wanna show wo are the owners
            indicator.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = shape.transform.GetChild(0).gameObject.GetComponent<Shape>().OwnersToStr();
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

    private void FetchForShape(){
        if(shape==null){
            shape = GameObject.Find("ShapeCircle(Clone)");
        }

        got_shape = shape!=null;
    }

    private void ShapeLocalisation(){
        //all true by default (worst case predicted)
        shape_in_down_left = true;
        shape_in_down_right = true;
        shape_in_up_left = true;
        shape_in_up_right = true;
        //first we wanna get the shape's center & diameter
        Vector2 coord = new Vector2(shape.transform.GetChild(0).gameObject.transform.position.x, shape.transform.GetChild(0).gameObject.transform.position.y);
        float x = coord.x;
        float y = coord.y;

        float d = shape.transform.GetChild(0).gameObject.transform.localScale.x/2;
        float mid_x = -4.5f;
        float mid_y = 0.0f;

        //BDT to define the region of the screen in which is located the shape
        if(x-d <=mid_x && x+d >mid_x && y-d<=mid_y && y+d>mid_y){
            //UPRIGHT + UPLEFT + DOWNRIGHT + DOWNLEFT
            return;
        } else if(x+d <=mid_x){
            if(y-d <=mid_y){
                if(y+d <=mid_y){
                    //DOWNLEFT
                    shape_in_down_right = false;
                    shape_in_up_left = false;
                    shape_in_up_right = false;
                    return;
                } else {
                    //DOWNLEFT + UPLEFT
                    shape_in_down_right = false;
                    shape_in_up_right = false;
                    return;
                }
            } else {
                //UPLEFT
                shape_in_down_right = false;
                shape_in_down_left = false;
                shape_in_up_right = false;
                return;
            }

        } else {
            if(x-d <=mid_x){
                if(y+d <=mid_y){
                    //DOWNRIGHT + DOWNLEFT
                    shape_in_up_left = false;
                    shape_in_up_right = false;
                    return;
                } else {
                    //UPLEFT + UPRIGHT
                    shape_in_down_left = false;
                    shape_in_down_right = false;
                    return;
                }
            } else {
                if(y+d <=mid_y){
                    //DOWNRIGHT
                    shape_in_down_left = false;
                    shape_in_up_right = false;
                    shape_in_up_left = false;
                    return;
                } else {
                    if(y-d <=mid_y){
                        //DOWNRIGHT + UPRIGHT
                        shape_in_down_left = false;
                        shape_in_up_left = false;
                        return;
                    } else {
                        //UPRIGHT
                        shape_in_down_left = false;
                        shape_in_up_left = false;
                        shape_in_down_right = false;
                        return;
                    }
                }
            }
        }
    }

    public void SetOwnership(){
        Shape shp = shape.transform.GetChild(0).gameObject.GetComponent<Shape>();
        shp.ResetOwners();
        if(shape_in_up_left){
            shp.SetOwner("UpLeft");
        }
        if(shape_in_up_right){
            shp.SetOwner("UpRight");
        }
        if(shape_in_down_left){
            shp.SetOwner("DownLeft");
        }
        if(shape_in_down_right){
            shp.SetOwner("DownRight");
        }
    }
}
