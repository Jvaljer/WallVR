using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapes : MonoBehaviour {
    //parent attributes
    private GameObject scene;
    private Scene scene_script;
    private GameObject shapes;

    //shapes objects
    private GameObject circle;

    // Start is called before the first frame update
    void Start(){
        scene = GameObject.Find("Scene");
        scene_script = scene.GetComponent<Scene>();
        shapes = GameObject.Find("Shapes");
        //getting the existing shapes 
            //each shape shall have 2 children 'small & big versions of themselves
        circle = GameObject.Find("Shapes/Circle");
    }

    // Update is called once per frame
    void Update(){
        
    }
}