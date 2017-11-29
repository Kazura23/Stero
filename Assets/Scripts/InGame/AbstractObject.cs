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

	public bool useGravity = true;

	protected Rigidbody mainCorps;
	protected Transform getTrans;
    PlayerController playerCont;
    protected Transform playerTrans;
    protected bool activeSlow = true;
	Rigidbody meshRigid;

	Vector3 projection;
	#endregion

	#region Mono
	void Awake () 
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

		if ( getThis.tag == Constants._EnnemisTag || getThis.tag == Constants._ObjDeadTag || getThis.tag == Constants._ObsTag )
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

	void onEnemyDead ( Vector3 forceProp )
	{
		isDead = true;
        ScreenShake.Singleton.ShakeEnemy();


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
