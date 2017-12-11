using UnityEngine;

public class DeadBall : MonoBehaviour 
{
	#region Variable
	public float ForcePropulse = 5;
	public float DelayDestruc = 1;
	#endregion
	
	#region Mono
	void OnCollisionEnter ( Collision collision )
	{
		if ( collision.collider.tag != Constants._UnTagg && collision.collider.tag != Constants._DebrisEnv )
		{
			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, collision.transform, ForcePropulse, DelayDestruc, 100, false, true ) );
			int randomSong = UnityEngine.Random.Range(0, 5);
			GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "Wood_" + (randomSong + 1), false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ( other.tag == Constants._PunchTag && tag == Constants._Intro )
		{
			GlobalManager.GameCont.ActiveGame ( );
		}
	}
	#endregion
		
	#region Public
	#endregion
	
	#region Private
	#endregion
}
