using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    //move method from small's coordinates in world
    public void MoveOnScreen(Vector3 small_pos){
        //here we wanna translate these coords to the screen's world position
        Debug.Log(small_pos);
        Debug.Log(transform.position);
    }
}
