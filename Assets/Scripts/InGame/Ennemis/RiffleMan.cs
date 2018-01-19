using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RiffleMan : AbstractObject 
{
	#region Variables
	public GameObject ball;
	public int NbrBalls = 20;
	public int ForceBall; 
	public float angle = 1;
	public int angleY = 3;
	public float SpeedSpawn = 0.2f;
	public float TimeDestr = 0.4f;

	Transform localShoot;

    bool detected = false;
	#endregion

	#region Mono
	protected override void Start () 
	{
		localShoot = getTrans.Find ( "SpawnShoot" );
        base.Start();
	}
	#endregion

	#region Public Methods
	public override void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		base.PlayerDetected ( thisObj, isDetected );

        if (!isDead)
        {
            
            //Debug.Log("anime active !!!");
        }

        if ( isDetected && !isDead )
		{
			StartCoroutine ( shootPlayer ( new WaitForSeconds ( SpeedSpawn ), false ) );
		}

		GetComponentInChildren<Animator> ( ).SetTrigger ( "Attack" );


        if (!detected)
        {
            detected = true;


            GlobalManager.AudioMa.OpenAudio(AudioType.OtherSound, "VinoHeadPop", false);
            GlobalManager.AudioMa.OpenAudio(AudioType.OtherSound, "VinoAttack", false);

            GameObject txt = GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + 8, transform.position.z), "TextEnemy", GlobalManager.Ui.transform.parent, 3);
            //txt.transform.DOLocalMoveY(6, 0);
            txt.transform.DOScale(Vector3.one * .15f, 0);
            txt.GetComponent<TextMesh>().text = GlobalManager.DialMa.dial[1].quotes[UnityEngine.Random.Range(0, GlobalManager.DialMa.dial[1].quotes.Length)];
        }

    }

	protected override void OnEnable ( )
	{
		base.OnEnable ( );
		detected = false;
	}

	public override void Dead ( bool enemy = false ) 
	{
		base.Dead ( enemy );
        AllPlayerPrefs.ANbVino++;
        AllPlayerPrefs.ANbTotalEnemyKill++;
        //Debug.Log("vino " + AllPlayerPrefs.ANbVino);
        //mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
    }
	#endregion

	#region Private Methods
	protected override void OnCollisionEnter ( Collision thisColl )
	{
		base.OnCollisionEnter ( thisColl );
		if ( isDead )
		{
			GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
		}
	}

	protected override void CollDetect ( )
	{
		base.CollDetect ( );
		GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
	}

	IEnumerator shootPlayer ( WaitForSeconds thisF, bool checkDir )
	{
		int a;
		GameObject getCurr;

		if ( NbrBalls < 1 )
		{
			NbrBalls = 1;
		}

		for ( a = 0; a < NbrBalls; a++ )
		{
			yield return thisF;

			if ( isDead )
			{
				break;
			}

			getCurr = ( GameObject ) Instantiate ( ball, localShoot );
			getCurr.transform.localPosition = new Vector3 ( 0, 0, 0 );

			if ( checkDir )
			{
				getCurr.GetComponent<Rigidbody> ( ).AddForce ( new Vector3 ( -NbrBalls * 0.5f + a * angle, Random.Range ( -angleY, angleY + 1 ), ForceBall ), ForceMode.VelocityChange );
			}
			else
			{
				getCurr.GetComponent<Rigidbody> ( ).AddForce ( new Vector3 ( NbrBalls * 0.5f - a * angle, Random.Range ( -angleY, angleY + 1 ), ForceBall ), ForceMode.VelocityChange );
			}

			Destroy ( getCurr, TimeDestr );
		}

		if ( !isDead )
		{
			StartCoroutine ( shootPlayer ( thisF, !checkDir ) );
		}
	}
	#endregion
}
