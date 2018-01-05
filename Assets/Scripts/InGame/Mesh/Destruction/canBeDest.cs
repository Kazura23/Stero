using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canBeDest : MonoBehaviour 
{	
	public float ForcePropulse = 25;
	public float DelayDestruc = 3;
    public int nbPunchDestroy = 5;
	public bool DeadByPlayer = true;
	bool onColl = false;

	void OnCollisionEnter ( Collision collision )
	{
		string getTag = collision.collider.tag;
		if ( !onColl && getTag == Constants._PlayerTag && DeadByPlayer || getTag == Constants._EnnemisTag || getTag == Constants._ObsEnn)
		{
			AbstractObject getAbs = GetComponent <AbstractObject> ( );
			if ( getAbs != null )
			{
				getAbs.ForceProp ( Vector2.zero );
			}

			onColl = true;
			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, collision.transform, ForcePropulse, DelayDestruc, 20 ) );
			int randomSong = UnityEngine.Random.Range ( 0, 5 );
			GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );


            VibrationManager.Singleton.ObsVibration();
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
