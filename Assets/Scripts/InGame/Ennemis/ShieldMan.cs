using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShieldMan : AbstractObject {

    private bool shieldActive;
    public float delay = 1;
    public float distance = 30, hauteur = 2;
    private float saveVal;

    #region Variables
    public Color NewColor;
    Color saveCol;

    Material parMat;

    bool detected;
    #endregion

    #region Mono
    protected override void Awake()
    {
		base.Awake();

        shieldActive = true;
		parMat = getTrans.GetComponent<MeshRenderer>().material;
        saveCol = parMat.color;
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
	protected override void OnEnable ( )
	{
		if ( !shieldActive )
		{
			Destroy ( gameObject );
		}
		else
		{
			base.OnEnable ( );
		}
	}

	protected override void OnCollisionEnter ( Collision thisColl )
	{
		base.OnCollisionEnter ( thisColl );
		if ( isDead )
		{
			GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
		}
	}
    #endregion
	public override void Dead(bool enemy = false)
	{
		base.Dead(enemy);
        AllPlayerPrefs.ANbTotalEnemyKill++;
        AllPlayerPrefs.ANbKnighty++;
        //Debug.Log("knighty " + AllPlayerPrefs.ANbKnighty);
        //mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
    }

	public override void ForceProp ( Vector3 forceProp, DeathType thisDeath, bool checkConst, bool forceDead = false )
	{
		if ( shieldActive && !GlobalManager.GameCont.Player.GetComponent<PlayerController> ( ).InMadness && !forceDead )
		{
			GlobalManager.GameCont.Player.GetComponent<PlayerController> ( ).GameOver ( );
		}
		else
		{
			getTrans.DOKill ( );
			base.ForceProp ( forceProp, thisDeath );
		}
	}

	public override void PlayerDetected( GameObject thisObj, bool isDetected )
	{
		base.PlayerDetected ( thisObj, isDetected );


       // if (GetComponenstInChildren<Transform>().tag == "Knighty")
          //  Destroy(tran.gameObject);

       /* foreach (Transform trans in transform)
        {
            if(trans.gameObject.tag == "Knighty")
            {
                Debug.Log(trans.gameObject);
            }
        }*/

        if (!detected)
        {
            detected = true;

            GetComponentInChildren<Animator>().SetTrigger("Attack");

            GameObject txt = GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), "TextEnemy", transform.parent, 3);
            txt.transform.DOScale(Vector3.one * .15f, 0);
            txt.GetComponent<TextMesh>().text = GlobalManager.DialMa.dial[2].quotes[UnityEngine.Random.Range(0, GlobalManager.DialMa.dial[2].quotes.Length)];
        }

		if ( isDetected && parMat != null )
		{
			parMat.color = NewColor;
		}
		else
		{
			parMat.color = saveCol;
		}
	}

    public override void Degat(Vector3 p_damage, int p_technic)
    {
        if (p_technic == 1)
        {
            if (shieldActive)
            {
				thisObj = ( GameObject ) Instantiate ( gameObject, getTrans.parent );
				thisObj.SetActive ( false );
				thisObj.GetComponent<AbstractObject> ( ).EventEnable ( );
				thisObj.transform.localPosition = startPos;

                shieldActive = false;
                playerCont.MadnessMana(1);

				getTrans.DOLocalMove ( getTrans.localPosition + getTrans.forward * distance, delay ); 

                int randomSong = UnityEngine.Random.Range(0, 3);

                GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "MetalHit_" + (randomSong + 1), false);

				foreach (Transform trans in gameObject.GetComponentsInChildren<Transform> ( ) )
                {
					if ( trans.tag == "Knighty" )
					{
						StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( trans.gameObject, GlobalManager.GameCont.Player.transform, 200, 2, 10 ) );
						//Destroy ( trans.gameObject );
					}
                }

                //animation shield destroy
            }
            else
            {
				getTrans.DOKill ( );
                base.Degat(p_damage, p_technic);
            }
        }else
        {
            if (!shieldActive)
            {
				base.Degat(p_damage, p_technic);
            }
        }
    }

	protected override void CollDetect ( )
	{
		if ( !shieldActive )
		{
			base.CollDetect ( );
		}

		GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
	}
}

//getTrans.DOLocalMove(getTrans.localPosition + getTrans.forward* distance, delay, true );
				//getTrans.DOMoveY((saveVal = getTrans.position.y) + hauteur, delay / 2).OnComplete<Tweener>(() => getTrans.DOMoveY(saveVal, delay * 0.5f));