using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DownLeft : MonoBehaviourPun {
    // Start is called before the first frame update
    void Start(){
        //nothing to initialize yet
    }

    // Update is called once per frame
    void Update(){
        //nothing to update yet
    }

    [PunRPC]
    public void SetName(string str){
        gameObject.name = str;
    }
}
