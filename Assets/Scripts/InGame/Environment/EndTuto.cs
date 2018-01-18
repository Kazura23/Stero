using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndTuto : MonoBehaviour 
{
	public GameObject DestroyThis;
	void OnCollisionEnter ( Collision collision )
	{
		string getTag = collision.collider.tag;
		if ( getTag == Constants._PlayerTag )
		{
			GlobalManager.GameCont.Player.GetComponent<PlayerController> ( ).ResetPosDo ( );
			Destroy ( DestroyThis );
		}
	}
}
