using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallctrl : MonoBehaviour {
    public GameObject Wall;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerExit(Collider other)
    {
        Wall.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        Wall.SetActive(false);
    }
}
