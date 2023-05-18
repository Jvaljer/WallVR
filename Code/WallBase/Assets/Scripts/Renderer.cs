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
    private float pixel_in_mm = 0.264275256f; //abel's laptop

    //shapes attributes
    //all shapes
    private Dictionary<string, GameObject> shapes;
    private Dictionary<string, Vector3> shapes_pos;
    private GameObject shape; //yet testing with only one shape (the circle)

    //self attributes
    private bool init = false;

    public void Start(){
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();
        network_handler = GameObject.Find("ScriptManager").GetComponent<NetworkHandler>();
        Debug.Log("Renderer Starts "+PhotonNetwork.IsMasterClient+"  ope_joined-> "+network_handler.ope_joined+" |=| "+GameObject.Find("Operator")!=null);

        /*
        //initializing render scales
        if(GameObject.Find("Operator")!=null){
            ope = GameObject.Find("Operator"); 
            shape = GameObject.Find("Circle 0");
            if(PhotonNetwork.IsMasterClient){
                Debug.Log("Entering Rendering ope_joined section as OPERATOR "+(shape!=null));
                sw = Screen.width;
                sh = Screen.height;
                screen_ratio = sh/sw;
                pix_to_unit = Camera.main.orthographicSize /(sh/2.0f);
                abs = 0.1f;
                ih_scale = ih_scale*abs;
            } else if(photonView.IsMine){
                Debug.Log("Entering Rendering ope_joined section as PARTICIPANT "+(shape!=null));
                sw = setup.wall_width;
                sh = setup.wall_height;
                screen_ratio = sh/sw;
                ortho_size = (float)Camera.main.orthographicSize / (float)setup.wall.RowsAmount();
                pix_to_unit = (float)setup.wall.RowsAmount() * (float)Camera.main.orthographicSize / (sh/2.0f);
                abs = 1.0f;
                shape.transform.localScale *= 2f;
            } else {
                Debug.Log("not my photonView nor master client");
            }
        } else {
            Debug.Log("Renderer start -> ope is null");
        } */
    }

    public void Input(string name, Vector3 coord, int id){
        //Debug.Log("Received input : "+name+" with shape : "+(shape!=null));
    }

    public void Initialize(){
        Debug.Log("initializing renderer");
        DisplayScene();
        ope = GameObject.Find("Operator(Clone)"); 
        shape = GameObject.Find("Circle(Clone)");

        if(PhotonNetwork.IsMasterClient){
            Debug.Log("Entering Rendering ope_joined section as OPERATOR "+(shape!=null));
            sw = Screen.width;
            sh = Screen.height;
            screen_ratio = sh/sw;
            pix_to_unit = Camera.main.orthographicSize /(sh/2.0f);
            abs = 0.1f;
            ih_scale = ih_scale*abs;
        } else {
            Debug.Log("Entering Rendering ope_joined section as PARTICIPANT "+(shape!=null));
            sw = setup.wall_width;
            sh = setup.wall_height;
            screen_ratio = sh/sw;
            ortho_size = (float)Camera.main.orthographicSize / (float)setup.wall.RowsAmount();
            pix_to_unit = (float)setup.wall.RowsAmount() * (float)Camera.main.orthographicSize / (sh/2.0f);
            abs = 1.0f;
            shape.transform.localScale *= 2f;
        }

        init = true;
    }

    public void DisplayScene(){
        //getting the active scene 
        Scene scene = SceneManager.GetActiveScene();

        List<GameObject> go_list = new List<GameObject>();

        foreach(GameObject root in scene.GetRootGameObjects()){
            go_list.Add(root);
            GetChildren(root, go_list, 1);
        }

        foreach(GameObject go in go_list){
            string indent = new string('-', go.transform.GetSiblingIndex());
            Debug.Log(indent+go.name);
        }
    }

    public void GetChildren(GameObject root_, List<GameObject> list_, int i_){
        for (int i = 0; i < root_.transform.childCount; i++){

            GameObject child = root_.transform.GetChild(i).gameObject; 
            // Add the child object to the list
            list_.Add(child);   
            // Recursively get the child GameObjects
            GetChildren(child, list_, i_+1);
        }
    }
}
