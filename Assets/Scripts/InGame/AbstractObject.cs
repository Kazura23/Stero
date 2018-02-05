﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AbstractObject : MonoBehaviour 
{
	#region Variables
	[Space]
	[HideInInspector]
	public bool isDead;
	public float delayDead = 2;

    [Header ("Contact avec obs")]
	[Tooltip ("pourcentage de velocité restante en pourcentage lors d'une collision avec un ennmis ( situation ou ce gameobject est en mouvement )")]
	public float VelRestant = 5;

	[Tooltip ("force de direction lorsque en collision contre un Object / ennemis ( situation ou ce gameobject est immobile )")]
	public float onObjForward;

	[Space]
	[Header ("Contrainte axe / rotation ")]
	[Tooltip ("Si différent de 0 alors l'axe est freeze")]
	public Vector3 FreezeAxe = Vector3.zero;

	[Tooltip ("Si différent de 0 alors l'axe de rotation est freeze")]
	public Vector3 FreezeRot = Vector3.zero;
    [Tooltip("Lier au score")]
    public int point = 100;

	public float BonusMultTimer = 1;

    public bool useGravity = true;

	protected GameObject thisObj;
	protected Rigidbody mainCorps;
	protected Transform getTrans;
    protected PlayerController playerCont;
    protected Transform playerTrans;
    protected bool activeSlow = true;
	protected bool isObject = false;
	protected Vector3 startPos;
	Rigidbody meshRigid;
    private int techPunch;

	Vector3 projection;
	float distForDB = 0;
	bool checkDead = false;
	bool destGame;
	#endregion

	#region Mono
	protected virtual void Awake () 
	{
		destGame = true;
		isDead = false;
		getTrans = transform;
		startPos = getTrans.localPosition;

		mainCorps = getTrans.GetComponent<Rigidbody> ( );
		Rigidbody [] allRig = getTrans.GetComponentsInChildren<Rigidbody> ( );

		if ( allRig.Length > 1 )
		{
			meshRigid = allRig [ 1 ];
		}
		else
		{
			meshRigid = mainCorps;
		}

		if ( mainCorps == null )
		{
			Debug.LogWarning ( "There no rigidBody" );
			Destroy ( gameObject );
			return;
		}
			
		mainCorps.constraints = RigidbodyConstraints.FreezeAll;
	}

 /*   protected virtual void Update()
    {
        if (playerCont.playerDead)
            PlayerDetected(playerTrans.gameObject, false);
    }*/

	IEnumerator waitCol ( )
	{
		yield return new WaitForSeconds ( 1 );

		checkDead = false;
		gameObject.GetComponent <Collider> ( ).enabled = true;

	}

	protected virtual void OnEnable ( )
	{
		if ( gameObject.GetComponent <Collider> ( ) == null )
		{
			Destroy ( gameObject );
			return;
		}
		/*string getName = gameObject.name;
		foreach (Transform getTran in getTrans.GetComponentsInChildren<Transform>())
		{
			getTran.name = "non";
		}
		gameObject.name = getName;*/
		StartCoroutine ( waitCol ( ) );
		gameObject.GetComponent <Collider> ( ).enabled = false;
		playerTrans = GlobalManager.GameCont.Player.transform;
		playerCont = playerTrans.GetComponent<PlayerController>();

		int a;
		foreach ( SkinnedMeshRenderer thisSkin in GetComponentsInChildren<SkinnedMeshRenderer> ( ))
		{
			for ( a = 0; a < thisSkin.materials.Length; a++ )
			{
				thisSkin.materials [ a ] = new Material ( thisSkin.materials [ a ] );
				thisSkin.materials [ a ].SetFloat ( "_highlight", 0f );
			}	
		}

		System.Action <DeadBallEvent> checkDBE = delegate ( DeadBallEvent thisEvnt ) 
		{ 
			if ( meshRigid != null )
			{
				distForDB = thisEvnt.CheckDist; 
				startDeadBall ( ); 
			}
		}; 

		GlobalManager.Event.Register ( checkDBE );
	}

    protected virtual void Start()
    {
		
    }
	#endregion

	#region Public Methods
	public void EventEnable ( Vector3 setPosition )
	{
		gameObject.SetActive ( false );

		Transform getPar = getTrans;
		int count = 0;
		while ( count < 50 && getPar.parent.tag != Constants._ChunkParent )
		{
			getPar = getPar.parent;
			count++;
		}

		if ( getPar.GetComponent<ChunkDisable> ( ) )
		{
			getPar.GetComponent<ChunkDisable> ( ).AddNewObj ( gameObject );
		}
		
		getTrans.localPosition = setPosition;
	}

	public virtual void Degat(Vector3 p_damage, int p_technic)
	{
		if ( !isDead )
		{
			projection = p_damage;
            techPunch = p_technic;
			Dead ( false, DeathType.Punch );
		}
	}

	public virtual void Dead ( bool enemy = false, DeathType thisDeath = DeathType.Enemy )
	{
        if ( enemy )
		{
			onEnemyDead ( getTrans.forward * onObjForward, thisDeath );
            int randomSong = UnityEngine.Random.Range(0, 8);

            GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "BodyImpact_" + (randomSong + 1), false);
        }
		else
		{
			onEnemyDead ( projection, thisDeath );
            GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
        }
	}

	protected virtual void CollDetect (  )
	{
		if ( !isDead )
		{
			Dead ( true, DeathType.Enemy );
		}
		else
		{
			mainCorps.velocity = mainCorps.velocity * ( VelRestant / 100 );
		}
	}

	public virtual void ForceProp ( Vector3 forceProp, DeathType thisDeath, bool checkConst = true, bool forceDead = false )
	{
		onEnemyDead ( forceProp, thisDeath, checkConst );
		if ( gameObject.activeSelf )
		{
			StartCoroutine ( enableColl ( ) );
		}
	}
	#endregion

	#region Private Methods
	protected virtual void OnCollisionEnter ( Collision thisColl )
	{
		if ( playerCont != null && playerCont.playerDead || gameObject.tag == Constants._ObjDeadTag )
		{
			return;
		}

		GameObject getThis = thisColl.gameObject;

		if ( getThis.tag == Constants._EnnemisTag || getThis.tag == Constants._ObjDeadTag || getThis.tag == Constants._ObsTag && checkDead )
		{
			//Debug.Log ( gameObject.name ); 
			Physics.IgnoreCollision ( thisColl.collider, GetComponent<Collider> ( ) );

			/*if ( getThis.tag == Constants._EnnemisTag || getThis.tag == Constants._ObjDeadTag )
			{
				//Debug.Log ( "ennemis touche" );
			}*/

			CollDetect ( );
		}

		/*else if ( getThis.tag == Constants._PlayerTag && gameObject.tag == Constants._ObjDeadTag )
		{
			Physics.IgnoreCollision ( thisColl.collider, GetComponent<Collider> ( ) );
		}*/
	}

	protected virtual void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._ChocWave )
		{
			ForceProp ( ( Vector3.up + Vector3.Normalize ( getTrans.position - GlobalManager.GameCont.Player.transform.position ) ) * 20, DeathType.SpecialPower, false, true );
		}
	}

	void startDeadBall ( ) 
	{ 
		float getDist = Vector3.Distance ( playerTrans.position, getTrans.position );
		
		if ( getDist < distForDB ) 
		{
            StaticRewardTarget.SSizeMagicSphere++;
			destGame = false;
			onEnemyDead ( Vector3.zero, DeathType.SpecialPower ); 

			float getConst = Constants.DB_Prepare;
			meshRigid.useGravity = false;
			meshRigid.velocity = Vector3.zero;

			meshRigid.transform.DOMove ( playerTrans.position + new Vector3 ( Random.Range ( -0.6f, 0.7f ), Random.Range ( -0.6f, 0.7f ), Random.Range ( 3, 6 ) ), Random.Range ( getConst * 0.25f, getConst  ), true ).OnComplete(() => {

				foreach ( Rigidbody thisRig in getTrans.GetComponentsInChildren<Rigidbody>())
				{
					thisRig.constraints = RigidbodyConstraints.FreezePosition;
					thisRig.useGravity = false;
				}

				System.Action <DeadBallParent> SetParent = delegate ( DeadBallParent thisEvnt ) 
				{ 
					foreach(Collider thisColl in gameObject.GetComponentsInChildren<Collider>())
					{
						thisColl.enabled = false;
					}

					getTrans.SetParent ( thisEvnt.NewParent );
					getTrans.localPosition = new Vector3 ( Random.Range ( -0.25f, 0.3f ), Random.Range ( -0.25f, 0.3f ), Random.Range ( -0.25f, 0.3f ) );
					getTrans.localRotation = new Quaternion ( Random.Range ( 0, 1.0f ), Random.Range ( 0, 1.0f ), Random.Range ( 0, 1.0f ), 0 );
					meshRigid.transform.position = getTrans.position;
				}; 

				GlobalManager.Event.Register ( SetParent ); 
			});

			meshRigid.transform.DOScale ( new Vector3 ( Random.Range ( 0.7f, 1 ), Random.Range ( 0.7f, 1 ), Random.Range ( 0.7f, 1 ) ), Random.Range ( getConst * 0.25f, getConst ) );
		} 
	} 

	void onEnemyDead ( Vector3 forceProp, DeathType thisDeath, bool checkConst = true )
	{
		if ( checkDead )
		{
			return;
		}

		checkDead = true;

		if ( playerCont != null )
		{
            if (playerCont.InMadness && tag == Constants._ObsPropSafe)
                StaticRewardTarget.SNbObstacleDestoyInMadness++;
			playerCont.RecoverTimer ( thisDeath, point, BonusMultTimer );
		}

		//Debug.Log ( gameObject.name + " / " + thisDeath );
		if ( thisObj == null )
		{
			thisObj = ( GameObject ) Instantiate ( gameObject, getTrans.position, getTrans.rotation, getTrans.parent );
			thisObj.SetActive ( false );
			thisObj.name = gameObject.name;
			thisObj.GetComponent<AbstractObject> ( ).EventEnable ( startPos );
		}

		isDead = true;

		if ( !GlobalManager.GameCont.Player.GetComponent<PlayerController> ( ).StopPlayer )
		{
			ScreenShake.Singleton.ShakeEnemy();
		}

        int randomSongBone = UnityEngine.Random.Range(0, 4);

		if ( !isObject )
		{
			GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "BoneBreak_" + (randomSongBone + 1), false);
		}

        var animation = GetComponentInChildren<Animator>();
        if(animation)
		    animation.enabled = false;

		if ( animation != null )
		{
			animation.enabled = false;
		}

		if ( checkConst )
		{
			checkConstAxe ( );
		}

		if ( useGravity )
		{
			mainCorps.useGravity = true;
		}

		if ( meshRigid.gameObject != gameObject && GetComponent<BoxCollider> ( ) != null )
		{
			GetComponent<BoxCollider> ( ).enabled = false;
		}
		
		meshRigid.AddForce ( forceProp, ForceMode.VelocityChange );

		string getObsT = Constants._ObjDeadTag;
		gameObject.tag = getObsT;

		foreach (Collider thisRig in gameObject.GetComponentsInChildren<Collider>())
		{
			thisRig.tag = getObsT;
			try
			{
				thisRig.GetComponent<Rigidbody>().AddForce(forceProp, ForceMode.VelocityChange );
			}
			catch
			{
				//Debug.Log ( "ForceOnBody failed" );
			}
		}
		//meshRigid.tag = getObsT;

		//GlobalManager.Event.UnRegister ( checkEnable );
		//GlobalManager.Event.UnRegister ( checkDBE );

		if ( destGame )
		{
			Destroy ( gameObject, delayDead );
		}
	}

	IEnumerator enableColl ( )
	{
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );
		Transform savePos = transform;
		Transform playPos = GlobalManager.GameCont.Player.transform;

		Physics.IgnoreCollision ( playPos.GetComponent<Collider> ( ), GetComponent<Collider> ( ) );

		yield return thisF;

		try{
			GetComponent<BoxCollider> ( ).enabled = true;
		}
		catch{
		}
	}

	void checkConstAxe ( )
	{
		mainCorps.constraints = RigidbodyConstraints.None;
		if ( FreezeAxe.x != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionX;
			foreach (Rigidbody thisRig in gameObject.GetComponentsInChildren<Rigidbody>())
			{
				thisRig.constraints = RigidbodyConstraints.FreezePositionX;
			}
			gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
		}

		if ( FreezeAxe.y != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionY;
			foreach (Rigidbody thisRig in gameObject.GetComponentsInChildren<Rigidbody>())
			{
				thisRig.constraints = RigidbodyConstraints.FreezePositionY;
			}
			gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
		}

		if ( FreezeAxe.z != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionZ;
			foreach (Rigidbody thisRig in gameObject.GetComponentsInChildren<Rigidbody>())
			{
				thisRig.constraints = RigidbodyConstraints.FreezePositionZ;
			}
			gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
		}

		if ( FreezeRot.x != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationX;
			foreach (Rigidbody thisRig in gameObject.GetComponentsInChildren<Rigidbody>())
			{
				thisRig.constraints = RigidbodyConstraints.FreezeRotationX;
			}
			gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
		}

		if ( FreezeRot.y != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationY;
			foreach (Rigidbody thisRig in gameObject.GetComponentsInChildren<Rigidbody>())
			{
				thisRig.constraints = RigidbodyConstraints.FreezeRotationY;
			}
			gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
		}

		if ( FreezeRot.z != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationZ;
			foreach (Rigidbody thisRig in gameObject.GetComponentsInChildren<Rigidbody>())
			{
				thisRig.constraints = RigidbodyConstraints.FreezeRotationZ;
			}
			gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
		}
	}
		
	public virtual void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
        //Renderer rend = GetComponentInChildren<Renderer>();
        //rend.material.shader.
        //rend.material.shader = Shader.Find("Character_toon");
        //Debug.Log(rend);
        //Debug.Log(Shader.GetGlobalFloat(");
        //Shader.SetGlobalFloat("highlight_amount", 0);
        //rend.material.SetFloat("highlight_amount", 0);

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
    #endregion
}
