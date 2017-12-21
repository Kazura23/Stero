using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canBeDest : MonoBehaviour 
{	
	public float ForcePropulse = 5;
	public float DelayDestruc = 1;
    public int nbPunchDestroy = 5;
	public bool DeadByPlayer = true;


	void OnCollisionEnter ( Collision collision )
	{
		string getTag = collision.collider.tag;
		if ( getTag == Constants._PlayerTag && DeadByPlayer || getTag == Constants._EnnemisTag || getTag == Constants._ObsEnn)
		{
			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, collision.transform, ForcePropulse, DelayDestruc, 20 ) );
			int randomSong = UnityEngine.Random.Range ( 0, 5 );
			GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );
		}
	}

    private void OnTriggerEnter ( Collider other )
    {
        if ( other.tag == Constants._PunchTag && tag == Constants._Intro )
        {
            nbPunchDestroy--;
            if(nbPunchDestroy == 0)
            {
				GlobalManager.GameCont.ActiveGame ( );
            }
        }
    }
}
