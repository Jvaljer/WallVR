using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {
    private Vector3 screen_center = new Vector3(-4.5f, 0f, 0f);
    private Vector3 pane_center = new Vector3(6.5f, 0f, 10f);

    private List<string> owners = new List<string>();

    //move method from small's coordinates in world
    public void MoveOnScreen(Vector3 small_pos){
        //here we wanna translate these coords to the screen's world position
        //first we wanna get the distance between pane_center & small_pos 
        float dist_x = (small_pos.x - pane_center.x)/3;
        float dist_y = (small_pos.y - pane_center.y)/3;

        Vector3 new_vec = new Vector3(dist_x*5, dist_y*5, small_pos.z);
        transform.position = screen_center+new_vec;
    }

    public void SetOwner(string id){
        owners.Add(id);
    }
    public void ResetOwners(){
        owners.Clear();
    }
}
