using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateHub : MonoBehaviour 
{

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Constants._PlayerTag ) 
		{
			GlobalManager.GameCont.Hub.SetActive ( false );
		}
	}
}
