using UnityEngine;

public class DeadBall : MonoBehaviour 
{
	#region Variable
	public float ForcePropulse = 5;
	public float DelayDestruc = 1;
	public float Acceleration = 50;

	Rigidbody getRirig;
	Transform getTrans;
	Vector3 getForward;
	#endregion
	
	#region Mono
	void Awake ( )
	{
		getForward = GlobalManager.GameCont.Player.transform.forward;
		getRirig = GetComponent<Rigidbody> ( );
		getTrans = transform;
		getRirig.AddForce ( getForward * Acceleration, ForceMode.VelocityChange );
		Destroy ( gameObject, 10 );
	}

	void Update ( )
	{
		getRirig.AddForce ( Acceleration * getForward * Time.deltaTime, ForceMode.VelocityChange );
	}

	void OnCollisionEnter ( Collision collision )
	{
		if ( collision.collider.tag == Constants._EnnemisTag || collision.collider.tag == Constants._ElemDash || collision.collider.tag == Constants._PlayerTag || collision.collider.tag == Constants._ObsTag )
		{
			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, collision.transform, ForcePropulse, DelayDestruc, 100, false, true ) );
			int randomSong = UnityEngine.Random.Range ( 0, 5 );
			GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );
		}
	}

	private void OnTriggerEnter ( Collider other )
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
