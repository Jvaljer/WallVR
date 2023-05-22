using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Circle : MonoBehaviourPun {

    private bool dragged = false;
    public List<int> owners { get; private set; } = new List<int>();
    //setters
    public void Pick(){
        dragged = true;
    }
    public void Drop(){
        dragged = false;
    }
    
    //getters
    public bool IsDragged(){
        return dragged;
    }

    public bool IsOwnedBy(int id){
        return owners.Contains(id);
    }

    //setters
    public void AddOwner(int id){
        owners.Add(id);
    }
    public void RemoveOwner(int id){
        owners.Remove(id);
    }
    public void ClearOwners(){
        owners.Clear();
    }
}
