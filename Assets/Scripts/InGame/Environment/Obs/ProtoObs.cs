using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoObs : AbstractObject 
{
	#region Variables
	#endregion

	#region Mono
	protected override void Awake ()
	{
		base.Awake ( );
		isObject = true;
	}
	#endregion

	#region Public Methods
	public override void Dead ( bool enemy = false, DeathType thisDeath = DeathType.Punch ) 
	{
		base.Dead ( enemy, thisDeath );
	}

	public override void Degat(Vector3 p_damage, int p_technic)
	{
		if ( p_technic == 0 )
		{
			if ( gameObject.activeSelf )
			{
				StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, playerCont.transform, 75, 3 ) );
				int randomSong = UnityEngine.Random.Range ( 0, 5 );
				GlobalManager.AudioMa.OpenAudio ( AudioType.FxSound, "Wood_" + ( randomSong + 1 ), false );
				base.Degat ( p_damage, p_technic );
			}
		}
		else if ( p_technic == 1 ) 
		{
			ForceProp( p_damage, DeathType.Punch, true, true );
			base.Degat ( p_damage, p_technic );
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
