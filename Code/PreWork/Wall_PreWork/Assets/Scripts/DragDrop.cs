using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour {
    private bool dragging = false;
    private Vector3 offset;

    public GameObject big;
    private Shape big_version;

    public void OnMouseDown(){
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    public void OnMouseDrag(){
        if (dragging){
            big_version = big.GetComponent<Shape>();
            Vector3 cur_screen_point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset.z);
            Vector3 cur_position = Camera.main.ScreenToWorldPoint(cur_screen_point) + offset;
            transform.position = cur_position;
            big_version.MoveOnScreen(cur_position);
        }
    }

    public void OnMouseUp(){
        dragging = false;
    }

}
