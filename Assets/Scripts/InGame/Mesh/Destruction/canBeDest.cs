using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canBeDest : MonoBehaviour 
{	
	public float ForcePropulse = 5;
	public float DelayDestruc = 1;

	void OnCollisionEnter ( Collision collision )
	{
		if ( collision.collider.tag == Constants._PlayerTag )
		{
			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, collision.transform, ForcePropulse, DelayDestruc ) );
            int randomSong = UnityEngine.Random.Range(0, 5);
            GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "Wood_" + (randomSong + 1), false);
        }
	}
}
