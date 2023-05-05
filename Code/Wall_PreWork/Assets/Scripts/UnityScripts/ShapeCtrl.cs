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
            Vector3 mouse_pos = Input.mousePosition;
            Camera cam = Camera.main;
            Vector3 view_port = cam.ScreenToViewportPoint(mouse_pos);
            Vector3 world_pos = cam.ViewportToWorldPoint(new Vector3(view_port.x, view_port.y, cam.nearClipPlane + (view_port.z * (cam.farClipPlane - cam.nearClipPlane))));
            
            //here there might be a problem, because after the first drag n drop, it's not possible to access to this feature anymore...
            //might have to do with some coordinates modifications that are moving the collider somewhere we don't want it to be
            small_version.transform.position = world_pos;
        }
    }
    public void OnMouseUp(){
        Debug.Log("Drag ended");
        moving = false;
    }
}
