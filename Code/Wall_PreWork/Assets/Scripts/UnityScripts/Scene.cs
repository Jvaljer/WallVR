using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour {

    //all GameObject of the scene
    private GameObject scene; // the main app scene
    private GameObject screen; //containing all 4 screens (players)
    private GameObject ctrl_pane; //the control pane where the manipulation happens

    //spatial attributes
    public Vector3 screen_center = new Vector3(-4.8f, 0f, 0f); //screen.position; 
    public Vector3 pane_center = new Vector3(5.2f, -3f, 0f);   //ctrl_pane.position;
    
    //predicates
    private bool accessible;

    // Start is called before the first frame update
    void Start(){
        scene = GameObject.Find("Scene");
        screen = GameObject.Find("Screen");
        ctrl_pane = GameObject.Find("ControlPane");
    }

    // Update is called once per frame
    void Update(){
        
    }

    //Specific Getters
    public bool IsAccessible(){
        return accessible;
    }

    //Specific Setters
    public void NotAccessible(){
        accessible = false;
    }
    public void Accessible(){
        accessible = true;
    }

    //All other methods 
    public void Initialize(int[] parameters){
        //must implement
        return;
    }
}
