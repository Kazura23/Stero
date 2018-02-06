using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallctrl : MonoBehaviour {
    public GameObject Wall;
    public GameObject Playerc;
	// Use this for initialization

 
    private void OnTriggerEnter(Collider Playerc)
    {
        Wall.SetActive(false);
    }
}
