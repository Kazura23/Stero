using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerNbLine : MonoBehaviour {

	public int NbrLineLeft;
	public int NbrLineRigh;
	public int CurrLine;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			/*PlayerController getPlayer = other.gameObject.GetComponent<PlayerController> ( );
			getPlayer.NbrLineLeft = NbrLineLeft;
			getPlayer.NbrLineRight = NbrLineRigh;
			getPlayer.currLine -= CurrLine;*/
		}
	}
}
