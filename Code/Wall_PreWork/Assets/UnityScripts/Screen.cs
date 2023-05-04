using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour {

    //parent attributes 
    private GameObject scene;
    private Scene scene_script;

    // Start is called before the first frame update
    void Start(){
        scene = GameObject.Find("Scene");
        scene_script = scene.GetComponent<Scene>();
    }

    // Update is called once per frame
    void Update(){
        
    }
}
