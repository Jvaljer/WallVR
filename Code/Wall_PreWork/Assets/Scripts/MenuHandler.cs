using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

    public GameObject ctrl_pane;
    public GameObject screen;
    public GameObject indicator;
    public GameObject menu;

    private GameObject up_left;
    private GameObject up_right;
    private GameObject down_left;
    private GameObject down_right;
    private GameObject circle;
    private GameObject square;
    private GameObject user;

    private bool all_got = false;
    
    // Start is called before the first frame update
    void Start(){
        //nothing to start
    }

    // Update is called once per frame
    void Update(){
        //nothing to update
    }

    public void Leave(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Resume(){
        menu.SetActive(false);
        ctrl_pane.SetActive(true);
        screen.SetActive(true);
        indicator.SetActive(true);
        //we also wanna enable all the photon entities
        up_left.SetActive(true);
        up_right.SetActive(true);
        down_left.SetActive(true);
        down_right.SetActive(true);
        circle.SetActive(true);
        square.SetActive(true);
        user.SetActive(true);
    }

    public void EnterMenu(){
        menu.SetActive(true);
        ctrl_pane.SetActive(false);
        screen.SetActive(false);
        indicator.SetActive(false);
        //we also wanna disable all the photon entities
        if(!all_got){
            FetchForEntities();
        }
        up_left.SetActive(false);
        up_right.SetActive(false);
        down_left.SetActive(false);
        down_right.SetActive(false);
        circle.SetActive(false);
        square.SetActive(false);
        user.SetActive(false);
    }

    public void FetchForEntities(){
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

        if(circle==null){
            circle = GameObject.Find("ShapeCircle(Clone)");
        }
        if(square==null){
            square = GameObject.Find("ShapeSquare(Clone)");
        }
        if(user==null){
            user = GameObject.Find("User(Clone)");
        }

        all_got = (up_left!=null && up_right!=null && down_right!=null && down_left!=null && circle!=null && square!=null && user!=null);
    }
}
