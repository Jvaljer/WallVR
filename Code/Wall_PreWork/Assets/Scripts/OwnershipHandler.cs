using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OwnershipHandler : MonoBehaviourPun {

    public GameObject indicator;
    private bool got_shape = false;
    public GameObject circle;
    public GameObject square;

    private GameObject up_left;
    private GameObject up_right;
    private GameObject down_right;
    private GameObject down_left;

    private bool all_got = false;
    private bool circle_in_up_left = true;
    private bool circle_in_up_right = true;
    private bool circle_in_down_left = true;
    private bool circle_in_down_right = true;

    private bool square_in_up_left = true;
    private bool square_in_up_right = false;
    private bool square_in_down_left = false;
    private bool square_in_down_right = false;

    // Update is called once per frame
    void Update(){
        if(!got_shape){
            FetchForShape();
        }
        if(!all_got){
            Debug.Log("Searching em screen parts");
            FetchScreenPart();
        } else if(got_shape){
            CircleLocalisation();
            SetCircleOwnership();

            SquareLocalisation();
            SetSquareOwnership();
            //now just for the information, we wanna show wo are the owners
            indicator.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = circle.transform.GetChild(0).gameObject.GetComponent<Shape>().OwnersToStr() + "\n"
                                                                                        + square.transform.GetChild(0).gameObject.GetComponent<Shape>().OwnersToStr();
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
        if(circle==null){
            circle = GameObject.Find("ShapeCircle(Clone)");
        }
        if(square==null){
            square = GameObject.Find("ShapeSquare(Clone)");
        }
        got_shape = circle!=null && square!=null;
    }

    private void CircleLocalisation(){
        //all true by default (worst case predicted)
        circle_in_down_left = true;
        circle_in_down_right = true;
        circle_in_up_left = true;
        circle_in_up_right = true;
        //first we wanna get the shape's center & diameter
        Vector2 coord = new Vector2(circle.transform.GetChild(0).gameObject.transform.position.x, circle.transform.GetChild(0).gameObject.transform.position.y);
        float x = coord.x;
        float y = coord.y;

        float d = circle.transform.GetChild(0).gameObject.transform.localScale.x/2;
        float mid_x = -4.5f;
        float mid_y = 0.0f;

        //float mid_d = Mathf.sqrt(2)/2 * d;

        //BDT to define the region of the screen in which is located the shape
        if(x-d <=mid_x && x+d >mid_x && y-d<=mid_y && y+d>mid_y){
            //UPRIGHT + UPLEFT + DOWNRIGHT + DOWNLEFT
            //must increase accuracy HERE 
            return;
        } else if(x+d <=mid_x){
            if(y-d <=mid_y){
                if(y+d <=mid_y){
                    //DOWNLEFT
                    circle_in_down_right = false;
                    circle_in_up_left = false;
                    circle_in_up_right = false;
                    return;
                } else {
                    //DOWNLEFT + UPLEFT
                    circle_in_down_right = false;
                    circle_in_up_right = false;
                    return;
                }
            } else {
                //UPLEFT
                circle_in_down_right = false;
                circle_in_down_left = false;
                circle_in_up_right = false;
                return;
            }

        } else {
            if(x-d <=mid_x){
                if(y+d <=mid_y){
                    //DOWNRIGHT + DOWNLEFT
                    circle_in_up_left = false;
                    circle_in_up_right = false;
                    return;
                } else {
                    //UPLEFT + UPRIGHT
                    circle_in_down_left = false;
                    circle_in_down_right = false;
                    return;
                }
            } else {
                if(y+d <=mid_y){
                    //DOWNRIGHT
                    circle_in_down_left = false;
                    circle_in_up_right = false;
                    circle_in_up_left = false;
                    return;
                } else {
                    if(y-d <=mid_y){
                        //DOWNRIGHT + UPRIGHT
                        circle_in_down_left = false;
                        circle_in_up_left = false;
                        return;
                    } else {
                        //UPRIGHT
                        circle_in_down_left = false;
                        circle_in_up_left = false;
                        circle_in_down_right = false;
                        return;
                    }
                }
            }
        }
    }

    public void SetCircleOwnership(){
        Shape shp = circle.transform.GetChild(0).gameObject.GetComponent<Shape>();
        shp.ResetOwners();
        if(circle_in_up_left){
            shp.SetOwner("UpLeft");
        }
        if(circle_in_up_right){
            shp.SetOwner("UpRight");
        }
        if(circle_in_down_left){
            shp.SetOwner("DownLeft");
        }
        if(circle_in_down_right){
            shp.SetOwner("DownRight");
        }
    }

    public void SquareLocalisation(){
        //all true by default
        square_in_up_left = true;
        square_in_up_right = true;
        square_in_down_left = true;
        square_in_down_right = true;

        //center coords : 
        Vector2 coord = new Vector2(square.transform.GetChild(0).gameObject.transform.position.x, square.transform.GetChild(0).gameObject.transform.position.x);

        float x = coord.x;
        float y = coord.y;

        float d = square.transform.GetChild(0).gameObject.transform.localScale.x/2;
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
                    square_in_down_right = false;
                    square_in_up_left = false;
                    square_in_up_right = false;
                    return;
                } else {
                    //DOWNLEFT + UPLEFT
                    square_in_down_right = false;
                    square_in_up_right = false;
                    return;
                }
            } else {
                //UPLEFT
                square_in_down_right = false;
                square_in_down_left = false;
                square_in_up_right = false;
                return;
            }

        } else {
            if(x-d <=mid_x){
                if(y+d <=mid_y){
                    //DOWNRIGHT + DOWNLEFT
                    square_in_up_left = false;
                    square_in_up_right = false;
                    return;
                } else {
                    //UPLEFT + UPRIGHT
                    square_in_down_left = false;
                    square_in_down_right = false;
                    return;
                }
            } else {
                if(y+d <=mid_y){
                    //DOWNRIGHT
                    square_in_down_left = false;
                    square_in_up_right = false;
                    square_in_up_left = false;
                    return;
                } else {
                    if(y-d <=mid_y){
                        //DOWNRIGHT + UPRIGHT
                        square_in_down_left = false;
                        square_in_up_left = false;
                        return;
                    } else {
                        //UPRIGHT
                        square_in_down_left = false;
                        square_in_up_left = false;
                        square_in_down_right = false;
                        return;
                    }
                }
            }
        }
    }

    public void SetSquareOwnership(){
        Shape shp = square.transform.GetChild(0).gameObject.GetComponent<Shape>();
        shp.ResetOwners();
        if(square_in_up_left){
            shp.SetOwner("UpLeft");
        }
        if(square_in_up_right){
            shp.SetOwner("UpRight");
        }
        if(square_in_down_left){
            shp.SetOwner("DownLeft");
        }
        if(square_in_down_right){
            shp.SetOwner("DownRight");
        }
    }
}
