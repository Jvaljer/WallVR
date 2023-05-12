using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Participant : MonoBehaviourPun {
    private float[] screen_coord;
    private string id;
    private GameObject circle;
    private bool init = true;

    public void Start(){
        Debug.Log("I am a participant who's starting duh");
    }
    
    public void Update(){
        if(photonView.IsMine){
            Debug.Log("I am Participant");
            Debug.Log("init : "+init);
            if(init){
                Debug.Log("must init");
                //and repositionnate the camera
                Camera.main.transform.position = new Vector3(-5f, 2.5f, -10f);
                //Initialize();
                Init();
            } else {
                Debug.Log("now initialized let's do some stuff");
            }
        }
    }
    
    public void Init(){
        circle = GameObject.Find("Circle");
        //now let's zoom in (Zoom x4)
        Camera.main.orthographicSize = 2.5f;
        //and repositionnate the camera
        float depth = Camera.main.transform.position.z;
        float w, h;
        
        switch (id){
            case "UpLeft":
                //up-left
                w = -4.5f;
                h = 2.5f;
                break;
            case "UpRight":
                //up-right
                w = 4.5f;
                h = 2.5f;
                break;
            case "DownLeft":
                //down-left
                w = -4.5f;
                h = -2.5f;
                break;
            case "DownRight":
                //down-right
                w = 4.5f;
                h = -2.5f;
                break;

            default:
                w = 0f;
                h = 0f;
                break;
        }
        
        Camera.main.transform.position = new Vector3(w, h, depth);
        init = circle==null;

    }
    public void Initialize(){
        circle = GameObject.Find("Circle");
        Camera.main.orthographicSize = 2.5f;

        float depth = Camera.main.transform.position.z;
        float w, h;

        switch (screen_coord[0],screen_coord[1]){
            case (0,0):
                //up-left
                w = -4.5f;
                h = 2.5f;
                break;
            case (1,0):
                //up-right
                w = 4.5f;
                h = 2.5f;
                break;
            case (0,1):
                //down-left
                w = -4.5f;
                h = -2.5f;
                break;
            case (1,1):
                //down-right
                w = 4.5f;
                h = -2.5f;
                break;

            default:
                w = 0f;
                h = 0f;
                break;
        }
        
        Camera.main.transform.position = new Vector3(w, h, depth);
        init = circle==null;
    }

    /*[PunRPC]
    public void RpcInitialize(float x, float y){
        Debug.Log("Initializing the part -> "+x+","+y);
        screen_coord = new float[2];
        screen_coord[0] = x;
        screen_coord[1] = y;

        id = ""+ x.ToString() + y.ToString();
        Debug.Log("Well initialized the part : "+id);
    } */

    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
        id = str;
    }
}
