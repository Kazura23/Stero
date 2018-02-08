using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerNbLine : MonoBehaviour {

	public int NbrLineLeft;
	public int NbrLineRigh;
	public int CurrLine;

	bool checkTrigger = false;

	void OnEnable ( )
	{
		checkTrigger = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Constants._PlayerTag &&!checkTrigger) 
		{
			Debug.Log ( "ChangeLine" );
			checkTrigger = true;

			if ( !GetComponent<SpawnNewLvl> ( ) )
			{
				PlayerController getPlayer = other.gameObject.GetComponent<PlayerController> ( );
				getPlayer.NbrLineLeft = NbrLineLeft - CurrLine;
				getPlayer.NbrLineRight = NbrLineRigh + CurrLine;
				getPlayer.currLine = CurrLine;
			}

//			Debug.Log ( getPlayer.currLine + " / " + getPlayer.NbrLineLeft + " / " + getPlayer.NbrLineRight );
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == Constants._PlayerTag && !checkTrigger) 
		{
			Debug.Log ( "ChangeLine" );
			checkTrigger = true;
			if ( !GetComponent<SpawnNewLvl> ( ) )
			{
				PlayerController getPlayer = other.gameObject.GetComponent<PlayerController> ( );
				getPlayer.NbrLineLeft = NbrLineLeft - CurrLine;
				getPlayer.NbrLineRight = NbrLineRigh + CurrLine;
				getPlayer.currLine = CurrLine;
			}
			//Debug.Log ( getPlayer.currLine + " / " + getPlayer.NbrLineLeft + " / " + getPlayer.NbrLineRight );
		}
	}

	public void UpdateManually ( PlayerController getPlayer )
	{
		getPlayer.NbrLineLeft = NbrLineLeft - CurrLine;
		getPlayer.NbrLineRight = NbrLineRigh + CurrLine;
		getPlayer.currLine = CurrLine;
	}
}
