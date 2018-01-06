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
	public override void Dead ( bool enemy = false ) 
	{
		base.Dead ( enemy );
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
	#endregion
}
