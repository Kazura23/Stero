using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightctrl : MonoBehaviour {
    public GameObject startlight;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerExit(Collider other)
    {
        startlight.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        startlight.SetActive(true);
    }
}
