using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class Renderer : MonoBehaviourPun {
    //unity attributes
    private Setup setup;
    private NetworkHandler network_handler;
    private GameObject ope;

    //geometry attibutes
    private float screen_ratio = 1f;
    private float pix_to_unit = 1f;
    private float sw;
    private float sh;
    private float abs = 1.0f;
    private float ih_scale = 1.5f;
    private float ortho_size = 5f;
    //private float pixel_in_mm = 0.264275256f; //abel's laptop

    //shapes attributes
    //all shapes
    private Dictionary<string, GameObject> shapes;

    public void Start(){
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        network_handler = GameObject.Find("ScriptManager").GetComponent<NetworkHandler>();
        shapes = new Dictionary<string, GameObject>();
        //Debug.Log("Renderer Starts "+PhotonNetwork.IsMasterClient+"  ope_joined-> "+network_handler.ope_joined+" |=| "+GameObject.Find("Operator")!=null);
    }

    public void Input(string name, Vector3 coord, int id){
        //Debug.Log("Receiving an input : "+name+" from "+id);
        //first we wanna check which one of the shape we are tryna move
        foreach(GameObject obj in shapes.Values){
            //Debug.Log("rooting the shapes : "+obj.name);
            Shape obj_ctrl = obj.GetComponent<Shape>();
            //Debug.Log("is owned by "+id+ " -> "+obj_ctrl.IsOwnedBy(id));
            if(obj_ctrl.IsOwnedBy(id)){ //later on we'll like to add more scripts + abstract class
                //Debug.LogError("our shape is well related to "+id);
                switch (name){
                    case "Down":
                        Debug.Log("testing down for "+obj.name);
                        if(obj_ctrl.CoordsInside(coord)){
                            obj.GetComponent<PhotonView>().RPC("PickRPC", RpcTarget.AllBuffered);
                        }
                        break;
                    case "Move":
                        //already tested if dragging ? test it again ?
                        if(obj_ctrl.IsDragged()){
                            //then move shape depending on role
                            obj.GetComponent<PhotonView>().RPC("MoveRPC", RpcTarget.AllBuffered, coord, setup.part_zoom);
                        }
                        break;
                    case "Up":
                        obj.GetComponent<PhotonView>().RPC("DropRPC", RpcTarget.AllBuffered);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void Initialize(){
        //Debug.Log("initializing renderer");
        ope = GameObject.Find("Operator(Clone)");
        if(!shapes.ContainsKey("Circle(Clone)")){
            shapes.Add(GameObject.Find("Circle(Clone)").name,GameObject.Find("Circle(Clone)"));
        }

        if(PhotonNetwork.IsMasterClient){
            //Debug.Log("Entering Rendering ope_joined section as OPERATOR "+(shape!=null));
            sw = Screen.width;
            sh = Screen.height;
            screen_ratio = sh/sw;
            pix_to_unit = Camera.main.orthographicSize /(sh/2.0f);
            abs = 0.1f;
            ih_scale = ih_scale*abs;
        } else {
            //Debug.Log("Entering Rendering ope_joined section as PARTICIPANT "+(shape!=null));
            sw = setup.wall_width;
            sh = setup.wall_height;
            screen_ratio = sh/sw;
            ortho_size = (float)Camera.main.orthographicSize / (float)setup.wall.RowsAmount();
            pix_to_unit = (float)setup.wall.RowsAmount() * (float)Camera.main.orthographicSize / (sh/2.0f);
            abs = 1.0f;
            foreach(GameObject shape in shapes.Values){
                //zoom value = amount of division ?
                //shape.transform.localScale *= 2f;
                shape.transform.localScale *= setup.part_zoom;
            }
        }
    }
}
