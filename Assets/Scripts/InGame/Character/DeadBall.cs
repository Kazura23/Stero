using UnityEngine;
using System.Collections;

public class DeadBall : MonoBehaviour 
{
	#region Variable
	//public float ForceProp = 10;
	//public float DelayDestruc = 1;
	public float StartForce = 50;
	public float AccelWhenPunch = 50;
	public float AccelWhenDBPunch = 100;
	public float ProjectilForce = 25;

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
		getRirig.AddForce ( getForward * StartForce, ForceMode.VelocityChange );

		StartCoroutine ( projectObj ( ) );
	}

	/*void Update ( )
	{
		getRirig.AddForce ( Acceleration * getForward * Time.deltaTime, ForceMode.VelocityChange );
	}*/
    
	void OnCollisionEnter ( Collision collision )
	{
		if ( collision.collider.tag == Constants._PlayerTag )
		{
			getRirig.AddForce ( AccelWhenPunch * GlobalManager.GameCont.Player.transform.forward, ForceMode.VelocityChange );
		}
		else if ( collision.collider.tag != Constants._UnTagg )
		{
			Physics.IgnoreCollision ( gameObject.GetComponent<Collider> ( ), collision.collider );
		}

		/*if ( collision.collider.tag != Constants._UnTagg && collision.collider.tag != "Debris" && collision.collider.tag != Constants._ObjDeadTag )
		{
			WaitForSeconds thisSec = new WaitForSeconds ( 0.1f );

			foreach ( Rigidbody thisRig in getTrans.GetComponentsInChildren<Rigidbody>())
			{
				if ( thisRig.GetComponent<canBeDest> ( ) )
				{
					thisRig.GetComponent<canBeDest> ( ).UseThis = true;
				}

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

			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, collision.transform, ForceProp, DelayDestruc ) );
			int randomSong = UnityEngine.Random.Range ( 0, 5 );
			GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );
		}*/
	}

	IEnumerator projectObj ( )
	{
		WaitForSeconds thisSec = new WaitForSeconds ( 0.1f );

		yield return new WaitForSeconds ( 5 );

		foreach ( Rigidbody thisRig in getTrans.GetComponentsInChildren<Rigidbody>())
		{
			if ( thisRig.GetComponent<canBeDest> ( ) )
			{
				thisRig.GetComponent<canBeDest> ( ).UseThis = true;
			}

			thisRig.constraints = RigidbodyConstraints.None;
			thisRig.useGravity = true;
			thisRig.AddForce ( new Vector3 ( Random.Range ( -ProjectilForce, ProjectilForce ), Random.Range ( -ProjectilForce, ProjectilForce ), Random.Range ( -ProjectilForce, ProjectilForce ) ), ForceMode.VelocityChange );
			StartCoroutine ( waitCol ( thisRig.GetComponent<Collider> ( ), thisSec ) );
		}

		StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, transform, ProjectilForce, 1, false, true ) );
		int randomSong = UnityEngine.Random.Range ( 0, 5 );
		GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );
	}

	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._ObsSafe || thisColl.tag == Constants._EnnemisTag || thisColl.tag == Constants._ElemDash )
		{
			Physics.IgnoreCollision ( gameObject.GetComponent<Collider> ( ), thisColl );
			AbstractObject getObj = thisColl.gameObject.GetComponent<AbstractObject> ( );

			if ( getObj )
			{
				getObj.startDeadBall ( true, transform );
			}
		}
		else if ( thisColl.tag == Constants._PunchTag )
		{
			Transform getPlayer = GlobalManager.GameCont.Player.transform;
			Punch getPunch = thisColl.GetComponent<Punch> ( );

			if ( getPunch.numTechnic == (int)Technic.basic_punch )
			{
				if ( getPunch.RightPunch )
				{
					getRirig.AddForce ( AccelWhenPunch * ( getPlayer.right * 0.5f + getPlayer.forward ), ForceMode.VelocityChange );
				}
				else
				{
					getRirig.AddForce ( AccelWhenPunch * ( -getPlayer.right * 0.5f  + getPlayer.forward ), ForceMode.VelocityChange );
				}
			}
			else if ( getPunch.numTechnic == (int)Technic.double_punch )
			{
				getRirig.AddForce ( AccelWhenDBPunch * getPlayer.forward, ForceMode.VelocityChange );
			}
		}
	}

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
