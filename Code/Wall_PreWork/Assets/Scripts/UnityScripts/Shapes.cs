using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapes : MonoBehaviour {
    //parent attributes
    private GameObject scene;
    private Scene scene_script;
    private GameObject shapes;

    //shapes objects
    public GameObject circle;
    private ShapeCtrl circle_ctrl;

    // Start is called before the first frame update
    void Start(){
        scene = GameObject.Find("Scene");
        scene_script = scene.GetComponent<Scene>();
        shapes = GameObject.Find("Shapes");
        //getting the existing shapes 
            //each shape shall have 2 children 'small & big versions of themselves
        circle_ctrl = circle.transform.GetChild(1).gameObject.GetComponent<ShapeCtrl>();
    }

    // Update is called once per frame
    void Update(){
        //here we just wanna make the link between the small & big versiosn of the circle 
        if(circle_ctrl.moving){
            MoveBigCircle();
        }
    }

    //other methods
    public void MoveBigCircle(){
        Vector3 small_pos = circle.transform.GetChild(0).transform.position;
        Vector3 translate_pos = new Vector3(0,0,0); 
        circle.transform.GetChild(1).gameObject.transform.position = translate_pos;
    }
}