using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDirect : MonoBehaviour 
{
	public bool GoRight;

	bool checkTrigger = false;

	void OnEnable ( )
	{
		checkTrigger = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Constants._PlayerTag && !checkTrigger) 
		{
			checkTrigger = true;
			PlayerController getPlayer = other.gameObject.GetComponent<PlayerController> ( );
			getPlayer.NewRotation ( gameObject, GoRight );
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == Constants._PlayerTag &&!checkTrigger) 
		{
			checkTrigger = true;
			PlayerController getPlayer = other.gameObject.GetComponent<PlayerController> ( );
			getPlayer.NewRotation ( gameObject, GoRight );
		}
	}
}
