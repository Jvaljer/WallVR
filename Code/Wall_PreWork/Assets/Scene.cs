using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour {

    //all GameObject of the scene
    private GameObject screen; //containing all 4 screens (players)
    private GameObject ctrl_pane; //the control pane where the manipulation happens
    
    // Start is called before the first frame update
    void Start(){
        Debug.Log("Starting the Scene.cs script");

        screen = GameObject.Find("Screen");
        ctrl_pane = GameObject.Find("ControlPane");
    }

    // Update is called once per frame
    void Update(){
        
    }
}
