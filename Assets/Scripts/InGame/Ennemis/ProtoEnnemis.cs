using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProtoEnnemis : AbstractObject
{
    #region Variables

    [Space]
	public Color NewColor;
	public bool CanRun = false;
	public float MoveSpeed = 10;

	Color saveCol;
	Material parMat;

    bool detected = false;
	#endregion

	#region Mono
	protected override void Awake ( )
	{
		//parMat = getTrans.GetComponent<MeshRenderer> ( ).material;
		//saveCol = parMat.color;
		base.Awake();
	}

	void Update () 
	{
		if ( playerTrans == null )
		{
			return;
		}

		if ( Vector3.Distance ( getTrans.position, playerTrans.position ) > 7.5f )
		{
			if ( CanRun && detected )
			{
				getTrans.localPosition += playerTrans.forward * Time.deltaTime * MoveSpeed;
			}
		}
		else if ( detected )
		{
			detected = false;
			attack ( );
		}
	}
	#endregion

	#region Public Methods
    public override void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		//base.PlayerDetected ( thisObj, isDetected );

        if (isDetected && !isDead && !detected)
        {
            detected = true;

			if ( CanRun )
			{
				int a;
				foreach ( SkinnedMeshRenderer thisSkin in GetComponentsInChildren<SkinnedMeshRenderer> ( ))
				{
					for ( a = 0; a < thisSkin.materials.Length; a++ )
					{
						thisSkin.materials [ a ] = new Material ( thisSkin.materials [ a ] );
						thisSkin.materials [ a ].SetFloat ( "_highlight", 1.0f );
					}	
				}
			}
            //parMat.color = NewColor;
        }
		else
		{
			//parMat.color = saveCol;
		}
		//parMat.color = NewColor;

       
	}

	void attack ( )
	{
		if ( !CanRun )
		{
			int a;
			foreach ( SkinnedMeshRenderer thisSkin in GetComponentsInChildren<SkinnedMeshRenderer> ( ))
			{
				for ( a = 0; a < thisSkin.materials.Length; a++ )
				{
					thisSkin.materials [ a ] = new Material ( thisSkin.materials [ a ] );
					thisSkin.materials [ a ].SetFloat ( "_highlight", 1.0f );
				}	
			}
		}

		GameObject txt = GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), "TextEnemy", transform.parent, 3);
		txt.transform.DOScale(Vector3.one * .15f, 0);
		string rdmText = GlobalManager.DialMa.dial[0].quotes[UnityEngine.Random.Range(0, GlobalManager.DialMa.dial[0].quotes.Length)];
		// Debug.Log(rdmText);
		txt.GetComponent<TextMesh>().text = rdmText;

		GlobalManager.AudioMa.OpenAudio(AudioType.OtherSound, "Charlotte_Attack", false);

		try {
			GetComponentInChildren<Animator>().SetTrigger("Attack");
		}
		catch{
		}
	}

	public override void Dead ( bool enemy = false ) 
	{
        int randomSong = UnityEngine.Random.Range(0, 2);

        GlobalManager.AudioMa.OpenAudio(AudioType.OtherSound, "Charlotte_Death" + (randomSong + 1), false);

        //GlobalManager.Ui.BloodHit();

        base.Dead ( enemy );
        //mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
    }
	#endregion

	#region Private Methods
	protected override void OnCollisionEnter ( Collision thisColl )
	{
        base.OnCollisionEnter ( thisColl );

        if ( isDead )
		{

            GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y, transform.localPosition.z + 5f), "EnemyNormalDeath", transform.parent, .35f);
		}
	}

	protected override void CollDetect ( )
	{
		base.CollDetect ( );

        GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
	}
	#endregion
}
