using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoObs : AbstractObject 
{
	#region Variables
	bool checkThis = false;
	#endregion

	#region Mono
	protected override void Awake ()
	{
		base.Awake ( );
		isObject = true;
	}

	protected override void OnEnable ( )
	{
		GetComponentInChildren<MeshRenderer>().enabled = true;
		base.OnEnable ( );
	}
	#endregion

	#region Public Methods
	public override void Dead ( bool enemy = false, DeathType thisDeath = DeathType.Punch ) 
	{
		base.Dead ( enemy, thisDeath );
	}

	public override void ForceProp( Vector3 forceProp, DeathType thisDeath, bool checkConst = true, bool forceDead = false )
	{
		if ( !checkThis )
		{
			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, playerCont.transform, 30, 3 ) );
			int randomSong = UnityEngine.Random.Range ( 0, 5 );
			GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );

			base.ForceProp ( forceProp, thisDeath, checkConst, forceDead );
			checkThis = true;
		}
	}
	public override void Degat(Vector3 p_damage, int p_technic)
	{
		if ( !checkThis )
		{
			if ( p_technic == 0 )
			{
				StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, playerCont.transform, 30, 3 ) );
				int randomSong = UnityEngine.Random.Range ( 0, 5 );
				GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );
				base.Degat ( p_damage, p_technic );
			}
			else if ( p_technic == 1 ) 
			{
				base.ForceProp( p_damage, DeathType.Punch, true, true );
			}
			checkThis = true;
		}
	}

	public override void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		base.PlayerDetected ( thisObj, isDetected );
	}
	#endregion

	#region Private Methods
	protected override void OnCollisionEnter ( Collision thisColl )
	{
		base.OnCollisionEnter ( thisColl );

		if ( isDead )
		{
			
		}
	}

	/*protected override void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._PunchTag )
		{
			
		}
	}*/
	#endregion
}
