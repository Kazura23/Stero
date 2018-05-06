using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canBeDest : MonoBehaviour 
{	
	public float ForcePropulse = 25;
	public float DelayDestruc = 3;
    public int nbPunchDestroy = 5;
	public bool DeadByPlayer = true;
	public bool UseThis = true;
	bool onColl = false;

	void OnCollisionEnter ( Collision collision )
	{
		if ( !UseThis )
		{
			return;
		}

		string getTag = collision.collider.tag;
		if ( !onColl && getTag == Constants._PlayerTag && !GlobalManager.GameCont.Player.GetComponent<PlayerController> ( ).Dash && DeadByPlayer && gameObject.tag != Constants._ElemDash || getTag == Constants._ObjDeadTag )
		{
			AbstractObject getAbs = GetComponent <AbstractObject> ( );
			if ( getAbs != null )
			{
				if ( getTag == Constants._PlayerTag )
				{
					getAbs.ForceProp ( Vector2.zero, DeathType.Walk );
				}
				else
				{
					getAbs.ForceProp ( Vector2.zero, DeathType.Enemy );
				}
			}

			onColl = true;
			if ( gameObject.activeSelf )
			{
				Debug.Log("Destroy");
				StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, collision.transform, ForcePropulse, DelayDestruc ) );
				int randomSong = UnityEngine.Random.Range ( 0, 5 );
				GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );

				VibrationManager.Singleton.ObsVibration();
			}
        }
	}

    private void OnTriggerEnter ( Collider other )
    {
		if ( UseThis && other.tag == Constants._PunchTag && tag == Constants._Intro )
        {
            nbPunchDestroy--;
            if(nbPunchDestroy == 0)
            {
				GlobalManager.GameCont.ActiveGame ( );
            }
        }
    }
}
