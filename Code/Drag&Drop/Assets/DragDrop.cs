using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    /*private bool dragging = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dragging && Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
    }
    
    public void OnMouseDown(){
    	dragging = true;
    }
    public void OnMouseDrag(){
    	if(dragging){
    		Vector3 mouse_pos = Input.mousePosition;
        	Camera cam = Camera.main;
        	Vector3 view_port = cam.ScreenToViewportPoint(mouse_pos);
        	Vector3 world_pos = cam.ViewportToWorldPoint(new Vector3(view_port.x, view_port.y, cam.nearClipPlane + (view_port.z * (cam.farClipPlane - cam.nearClipPlane))));
            
     		//transform.position = mouse_pos;
        	transform.position = world_pos;
        }
    }
    public void OnMouseUp()
    {
        dragging = false;
    }*/
    private bool dragging = false;
    private Vector3 offset;

    public void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    public void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 cur_screen_point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset.z);
            Vector3 cur_position = Camera.main.ScreenToWorldPoint(cur_screen_point) + offset;
            transform.position = cur_position;
        }
    }

    public void OnMouseUp()
    {
        dragging = false;
    }
}
