using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerTest : MonoBehaviour {

    Player inputPlayer;
    Rigidbody rigi;

	// Use this for initialization
	void Start () {
        inputPlayer = ReInput.players.GetPlayer(0);
        rigi = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float axis = 0;
		if((axis = inputPlayer.GetAxis("Horizontal")) != 0)
        {
            transform.Translate(axis * Time.deltaTime * 10, 0, 0);
        }else if (inputPlayer.GetButton("CoupSimple"))
        {
            rigi.AddForce(Vector3.up * 3, ForceMode.Impulse);
        }
	}
}
