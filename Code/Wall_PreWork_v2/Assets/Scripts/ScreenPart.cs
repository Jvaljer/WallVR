using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScreenPart : MonoBehaviourPun {
    //we want each screen to have its id which will allow the program to decide where to locate the shape
    private string loc_id;

    //owning the shape predicates
    private bool is_owner;

    // Update is called once per frame
    void Update(){
        //nothing to update there
    }
    
    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }

    [PunRPC]
    public void Initialize(string id){
        loc_id = id;
    }
    [PunRPC]
    public void ShowCircle(GameObject shape){
        Debug.Log("showing the shape on "+loc_id);
        //must implement
        return;
    }
}
