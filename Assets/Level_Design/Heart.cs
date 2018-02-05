using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			// On gagne un coeur si on n'est pas déjà au max

			PlayerController getP = GetComponent<PlayerController> ( );

			if ( getP.Life < 3 )
			{
				getP.Life++;
				GlobalManager.Ui.StartBonusLife ( getP.Life );
			}

			Destroy (this.gameObject);
		}
	}
}
