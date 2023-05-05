using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPane : MonoBehaviour {
    //parent attributes
    private GameObject pane;
    private GameObject scene;
    private Scene scene_script;

    //specific predicates
    public bool mouse_is_inside;

    // Start is called before the first frame update
    void Start(){
        scene = GameObject.Find("Scene");
        scene_script = scene.GetComponent<Scene>();
        pane = scene.transform.GetChild(0).gameObject;
        mouse_is_inside = false;
    }

    // Update is called once per frame
    void Update(){
    }

    //all control pane's methods
    private void OnMouseEnter() {
        mouse_is_inside = true;
    }
    private void OnMouseExit(){
        mouse_is_inside = false;
    }
}
