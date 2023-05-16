using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class Rendering : MonoBehaviourPun {
    //unity attributes
    private Setup setup;

    //geometry attibutes
    private float screen_ratio = 1f;
    private float pix_to_unit = 1f;
    private float sw;
    private float sh;
    private float abs = 1.0f;
    private float ih_scale = 1.5f;
    private float ortho_size = 5f;
    private float pixel_in_mm = 0.264275256f; //abel's laptop

    public void Awake(){
        Debug.Log("Rendering Awakes");
    }

    public void Start(){
        Debug.Log("Rendering Starts "+photonView.IsMine+" || "+PhotonNetwork.IsMasterClient);
        setup = GameObject.Find("ScriptManager").GetComponent<Setup>();

        if(photonView.IsMine){
            sw = Screen.width;
            sh = Screen.height;
            screen_ratio = sh/sw;
            pix_to_unit = Camera.main.orthographicSize /(sh/2.0f);
            abs = 0.1f;
            ih_scale = ih_scale*abs;
        } else {
            sw = setup.wall_width;
            sh = setup.wall_height;
            screen_ratio = sh/sw;
            ortho_size = (float)Camera.main.orthographicSize / (float)setup.wall.RowsAmount();
            pix_to_unit = (float)setup.wall.RowsAmount() * (float)Camera.main.orthographicSize / (sh/2.0f);
            abs = 1.0f;
        }
    }
}