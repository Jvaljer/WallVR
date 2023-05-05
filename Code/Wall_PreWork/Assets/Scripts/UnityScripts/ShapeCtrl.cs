using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShapeCtrl : MonoBehaviour {
    //unity attrivutes
    private GameObject ctrl_pane;
    private ControlPane pane_script;
    private GameObject scene;
    public GameObject shapes;
    public GameObject big_version;
    public GameObject small_version;


    //predicates 
    public bool moving = false;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("ShapeCtrl is starting");
        scene = GameObject.Find("Scene");
        ctrl_pane = scene.transform.GetChild(0).gameObject;
        pane_script = ctrl_pane.GetComponent<ControlPane>();
    }

    // Update is called once per frame
    void Update() {
        if(!pane_script.mouse_is_inside){
            moving = false;
        }
    }

    //dragging methods 
    public void OnMouseDown(){
        Debug.Log("Drag started");
        moving = true;
    }
    public void OnMouseDrag(){
        Debug.Log("Drag running");
        if(pane_script.mouse_is_inside){
            small_version.transform.position = Input.mousePosition;
            Debug.Log("");
        }
    }
    public void OnMouseUp(){
        Debug.Log("Drag ended");
        moving = false;
    }
}
