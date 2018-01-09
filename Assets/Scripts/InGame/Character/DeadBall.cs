using UnityEngine;
using System.Collections;

public class DeadBall : MonoBehaviour 
{
	#region Variable
	public float ForceProp = 10;
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
    /*
	void OnCollisionEnter ( Collision collision )
	{
		if ( collision.collider.tag != Constants._UnTagg && collision.collider.tag != Constants._DebrisEnv && collision.collider.tag != Constants._ObjDeadTag )
		{
			WaitForSeconds thisSec = new WaitForSeconds ( 0.1f );
			foreach ( Rigidbody thisRig in getTrans.GetComponentsInChildren<Rigidbody>())
			{
				thisRig.constraints = RigidbodyConstraints.None;
				thisRig.useGravity = true;
				thisRig.AddForce ( new Vector3 ( Random.Range ( -Acceleration, Acceleration ), Random.Range ( -Acceleration, Acceleration ), Random.Range ( -Acceleration, Acceleration ) ), ForceMode.VelocityChange );
				StartCoroutine ( waitCol ( thisRig.GetComponent<Collider> ( ), thisSec ) );
			}

			AbstractObject thisObj = collision.gameObject.GetComponentInChildren<AbstractObject> ( );
			if ( thisObj != null )
			{
				thisObj.Dead ( true );
			}
			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, collision.transform, ForceProp, DelayDestruc, 100, false, true, true ) );
			int randomSong = UnityEngine.Random.Range ( 0, 5 );
			GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );
		}
	}*/

	IEnumerator waitCol ( Collider thisColl, WaitForSeconds thisSec )
	{
		yield return thisSec;
		thisColl.enabled = true;
	}
	#endregion
		
	#region Public
	#endregion
	
	#region Private
	#endregion
}
