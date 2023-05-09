using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Operator : MonoBehaviourPun {
    //menu attributes
    private GameObject menu;

    //shapes attributes
    private GameObject circle;

    //photon participants entity
    private GameObject upleft;
    private GameObject upright;
    private GameObject downleft;
    private GameObject downright;

    //ready to start predicate
    private bool ready = false;
    private bool part_ready = false;
    private bool shape_ready = false;

    //owners list
    private List<GameObject> owners = new List<GameObject>();
    private List<string> owners_id = new List<string>();

    public void Start(){
        menu = GameObject.Find("OperatorView/MenuCanva");
    }
    // Update is called once per frame
    public void Update(){
        FetchForShapes();
        DetermineOwners();
        /*foreach(GameObject owner in owners){
            owner.GetComponent<PhotonView>().RPC("ShowCircle", RpcTarget.AllBuffered, circle);
        }*/
        SetOwnerInfo();
        //we wanna fetch all the screen parts before we allow the program to start
        if(!ready){
            if(!part_ready){
                FetchForParticipants();
            }
            if(!shape_ready){
                FetchForShapes();
            }
            ready = part_ready && shape_ready;
        } else {
            Debug.Log("determining owners");
            DetermineOwners();
            foreach(GameObject owner in owners){
                owner.GetComponent<PhotonView>().RPC("ShowCircle", RpcTarget.AllBuffered, circle);
            }
            SetOwnerInfo();
        }
    }

    public void FetchForParticipants(){
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
        part_ready = upleft!=null && upright!=null && downright!=null && downleft!=null;
    }
    public void FetchForShapes(){
        if(circle==null){
            circle = GameObject.Find("Circle");
        }
        shape_ready = circle!=null;
    }

    public void DetermineOwners(){
        //first of all we clean the owners list
        if(owners!=null){
            owners.Clear();
        }
        if(owners_id!=null){
            owners_id.Clear();
        }
        //then we initiate the needed variables 
        float x0 = 0f;
        float y0 = 0f;
        float r = circle.transform.GetChild(0).gameObject.transform.localScale.x/2;
        Vector2 coord = new Vector2(circle.transform.position.x, circle.transform.position.y);
        float x = coord.x;
        float y = coord.y;

        //now we use a BDT to locate the circle (yet not with the best accuracy) 
        if(x+r <=x0){
            if(y-r <=y0){
                if(y+r <=y0){
                    //DownLeft
                    owners.Add(upleft);
                    owners_id.Add("UpLeft");
                } else {
                    //DownLeft + UpLeft
                    owners.Add(downleft);
                    owners.Add(upleft);
                    owners_id.Add("DownLeft");
                    owners_id.Add("UpLeft");
                }
            } else {
                //UpLeft
                owners.Add(upleft);
                owners_id.Add("UpLeft");
            }
        } else {
            if(x-r <= x0){
                if(y+r <=y0){
                    //DownRight + DownLeft
                    owners.Add(downright);
                    owners.Add(downleft);
                    owners_id.Add("DownRight");
                    owners_id.Add("DownLeft");
                } else {
                    if(y-r <=y0){
                        //(worst case) 
                        //UpLeft + UpRight + DownLeft + DownRight
                        owners.Add(upleft);
                        owners.Add(upright);
                        owners.Add(downleft);
                        owners.Add(downright);
                        owners_id.Add("UpLeft");
                        owners_id.Add("UpRight");
                        owners_id.Add("DownLeft");
                        owners_id.Add("DownRight");
                    } else {
                        //UpLeft + UpRight
                        owners.Add(upleft);
                        owners.Add(upright);
                        owners_id.Add("UpLeft");
                        owners_id.Add("UpRight");
                    }
                }
            } else {
                if(y+r <=y0){
                    //DownRight
                    owners.Add(downright);
                    owners_id.Add("DownRight");
                } else {
                    if(y-r <=y0){
                        //DownRight + UpRight
                        owners.Add(downright);
                        owners.Add(upright);
                        owners_id.Add("DownRight");
                        owners_id.Add("UpRight");
                    } else {
                        //UpRight
                        owners.Add(upright);
                        owners_id.Add("UpRight");
                    }
                }
            }
        }
    }

    public void SetOwnerInfo(){
        string str = "";
        foreach(string id in owners_id){
            str += id + "\n";
        }
        menu.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = str;
    }

    //RPC methods
    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }
}
