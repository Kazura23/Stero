using System.Collections;
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

    public bool useGravity = true;

	protected Rigidbody mainCorps;
	protected Transform getTrans;
    PlayerController playerCont;
    protected Transform playerTrans;
    protected bool activeSlow = true;
	Rigidbody meshRigid;
    private int techPunch;

	Vector3 projection;
	float distForDB = 0; 
	System.Action <DeadBallEvent> checkDBE;
	#endregion

	#region Mono
	protected virtual void Awake () 
	{
		isDead = false;

		getTrans = transform;

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

		checkDBE = delegate ( DeadBallEvent thisEvnt ) 
		{ 
			if ( meshRigid != null )
			{
				distForDB = thisEvnt.CheckDist; 
				startDeadBall ( ); 
			}
		}; 

		GlobalManager.Event.Register ( checkDBE ); 
	}

    void Update()
    {
        if (playerCont.playerDead)
            PlayerDetected(playerTrans.gameObject, false);
    }

    protected virtual void Start()
    {
		playerTrans = GlobalManager.GameCont.Player.transform;
		playerCont = playerTrans.GetComponent<PlayerController>();
    }
	#endregion

	#region Public Methods
	public virtual void Degat(Vector3 p_damage, int p_technic)
	{
		if ( !isDead )
		{
			projection = p_damage;
            techPunch = p_technic;
			Dead ( );
		}
	}

	public virtual void Dead ( bool enemy = false )
	{
		isDead = true;
        Time.timeScale = 1;
        //StartCoroutine ( disableColl ( ) );
        
        int randomSong = UnityEngine.Random.Range(0, 8);

		GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "BodyImpact_" + (randomSong + 1),false);


       // Debug.Log("BoneBreak");

        //checkConstAxe ( );

        if ( enemy )
		{
			onEnemyDead ( getTrans.forward * onObjForward );
		}
		else
		{
			Vector3 getFor = getTrans.forward * projection.z;
			Vector3 getRig = getTrans.right * projection.x;
			Vector3 getUp = transform.up * projection.y;
			onEnemyDead ( getFor + getRig + getUp );
		}
        
		Destroy ( gameObject, delayDead );
	}

	protected virtual void CollDetect (  )
	{
		if ( !isDead )
		{
			Dead ( true );
		}
		else
		{
			mainCorps.velocity = mainCorps.velocity * ( VelRestant / 100 );
		}
	}

	public virtual void ForceProp ( Vector3 forceProp )
	{
		onEnemyDead ( forceProp );
		StartCoroutine ( enableColl ( ) );
		Destroy ( gameObject, delayDead );
	}
	#endregion

	#region Private Methods
	protected virtual void OnCollisionEnter ( Collision thisColl )
	{
		GameObject getThis = thisColl.gameObject;

		if ( getThis.tag == Constants._EnnemisTag && gameObject.tag != Constants._ObsSafe || getThis.tag == Constants._ObjDeadTag || getThis.tag == Constants._ObsTag )
		{
			Physics.IgnoreCollision ( thisColl.collider, GetComponent<Collider> ( ) );

			if ( getThis.tag == Constants._EnnemisTag || getThis.tag == Constants._ObjDeadTag )
			{
				//Debug.Log ( "ennemis touche" );
			}

			CollDetect ( );
		}

		/*else if ( getThis.tag == Constants._PlayerTag && gameObject.tag == Constants._ObjDeadTag )
		{
			Physics.IgnoreCollision ( thisColl.collider, GetComponent<Collider> ( ) );
		}*/
	}

	void startDeadBall ( ) 
	{ 
		float getDist = Vector3.Distance ( playerTrans.position, getTrans.position );
		
		if ( getDist < distForDB ) 
		{ 
			onEnemyDead ( Vector3.zero ); 

			float getConst = Constants.DB_Prepare;
			meshRigid.useGravity = false;
			meshRigid.velocity = Vector3.zero;

			meshRigid.transform.DOMove ( playerTrans.position + new Vector3 ( Random.Range ( -0.6f, 0.7f ), Random.Range ( -0.6f, 0.7f ), Random.Range ( 3, 6 ) ), Random.Range ( getConst * 0.25f, getConst  ), true ).OnComplete(() => {
				meshRigid.velocity = Vector3.zero;
				meshRigid.constraints = RigidbodyConstraints.FreezeAll;

				foreach (Rigidbody thisRig in meshRigid.GetComponentsInChildren<Rigidbody>())
				{
					thisRig.constraints = RigidbodyConstraints.FreezeAll;
				}
			});

			meshRigid.transform.DOScale ( new Vector3 ( Random.Range ( 0.5f, 1 ), Random.Range ( 0.5f, 1 ), Random.Range ( 0.5f, 1 ) ), Random.Range ( getConst * 0.25f, getConst ) );

			Destroy ( gameObject, Constants.DB_Prepare + 0.1f );
		} 
	} 

	void onEnemyDead ( Vector3 forceProp )
	{
		isDead = true;
        AllPlayerPrefs.scoreWhithoutDistance += point;

		if ( !GlobalManager.GameCont.Player.GetComponent<PlayerController> ( ).StopPlayer )
		{
			ScreenShake.Singleton.ShakeEnemy();
		}

        int randomSongBone = UnityEngine.Random.Range(0, 4);

        GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "BoneBreak_" + (randomSongBone + 1), false);

        var animation = GetComponentInChildren<Animator>();
        if(animation)
		    animation.enabled = false;

		if ( animation != null )
		{
			animation.enabled = false;
		}

		mainCorps.constraints = RigidbodyConstraints.None;
		checkConstAxe ( );
		if ( useGravity )
		{
			mainCorps.useGravity = true;
		}

		if ( meshRigid.gameObject != gameObject )
		{
			GetComponent<BoxCollider> ( ).enabled = false;
		}

        if (techPunch == 1)
        {
            meshRigid.constraints = RigidbodyConstraints.FreezePositionX;
        }
		meshRigid.AddForce ( forceProp, ForceMode.VelocityChange );
		string getObsT = Constants._ObjDeadTag;
		foreach (Rigidbody thisRig in meshRigid.GetComponentsInChildren<Rigidbody>())
		{
			thisRig.tag = getObsT;
		}
		meshRigid.tag = getObsT;
	}

	IEnumerator enableColl ( )
	{
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );
		Transform savePos = transform;
		Transform playPos = GlobalManager.GameCont.Player.transform;

		Physics.IgnoreCollision ( playPos.GetComponent<Collider> ( ), GetComponent<Collider> ( ) );

		yield return thisF;

		//GetComponent<BoxCollider> ( ).enabled = true;
	}

	void checkConstAxe ( )
	{
		if ( FreezeAxe.x != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionX;
		}

		if ( FreezeAxe.y != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionY;
		}

		if ( FreezeAxe.z != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionZ;
		}

		if ( FreezeRot.x != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationX;
		}

		if ( FreezeRot.y != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationY;
		}

		if ( FreezeRot.z != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationZ;
		}
	}
		
	IEnumerator disableColl ( )
	{
		WaitForSeconds thisSec = new WaitForSeconds ( 0.0f );

		yield return thisSec;

		getTrans.tag = Constants._ObjDeadTag;
	}

	public virtual void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
        Renderer rend = GetComponentInChildren<Renderer>();
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
