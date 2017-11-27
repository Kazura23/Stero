﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	#region Variables
	[Header ("Caractéristique sur une même Lane")]
	public float MaxSpeed = 5;
	public float Acceleration = 10;
	public float Deceleration = 1;

	[Header ("Caractéristique de changement de Lane")]
	public float MaxSpeedCL = 5;
	public float AccelerationCL = 10;
	public float ImpulsionCL = 10;
	public float DecelerationCL = 1;

	[Header ("Autres runner Caractéristiques ")]
	public float RotationSpeed = 1;
	[Tooltip ("Effet d'augmentation du FOV")]
	public float SpeedEffectTime;
	[Tooltip ("Force bonus en plus de la gravitée")]
	public float BonusGrav = 0;
	[Tooltip ("Pourcentage de ralentissement du personnage dans les airs")]
	public float PourcRal = 50;

	[Header ("IncreaseSpeed")]
	[Tooltip ("Distance a parcourir pour augmenter la vitesse Max")]
	public int DistIncMaxSpeed = 100;
	[Tooltip ("Augmentation du speed max")]
	public float SpeedIncrease = 0;
	public float CLSpeedIncrease = 0;
	public float MaxSpeedInc = 10;
	public float MaxCLInc = 10;
	public float AcceleraInc = 0;
	public float AcceleraCLInc = 0;

	//public float JumpForce = 200;
    [Header("Caractéristique Dash")]
    /*public float delayLeft = 1;
	public float delayRight = 1;*/
	public float DashTime = 1.5f;
	[Tooltip ("La valeur de DashSpeed est un multiplicateur sur la vitesse du joueur")]
	public float DashSpeed = 2.5f;
	[Tooltip ("Temps d'invicibilité apres avoir pris des dégats")]
	public float TimeInvincible = 2;

	//[Header ("Slow Motion Caractéristique")]
	[HideInInspector]
	public float SlowMotion, SpeedSlowMot, SpeedDeacSM, RecovSlider, ReduceSlider;

	[Header ("Caractérique punchs")]
	public float FOVIncrease = 20;
    public float DelayPunch = 0.05f;
	public float TimeToDoublePunch = 0.25f;
	public float CooldownDoublePunch = 1;
	public float DelayHitbox = 0.05f;
	public float DelayPrepare = 0.05f;
	public float delayChocWave = 5;

	[Tooltip ("Le temps max sera delayPunch")]
	public float TimePropulsePunch = 0.1f, TimePropulseDoublePunch = 0.2f;
	[Tooltip ("La valeur est un multiplicateur sur la vitesse du joueur")]
	public float SpeedPunchRun = 1.2f, SpeedDoublePunchRun = 1.5f;

    [Header("Caractéristique Madness")]
    public float RatioMaxMadness = 4;
    public float DelayDownBar = 3;
    //public float LessPointPunchInMadness = 15;
    public float SmoothSpeed = 100;
    public float ratioDownInMadness = 1.5f;

    public float delayInBeginMadness = 2;
    public float delayInEndMadness = 2;
    public float slowInBeginMadness = 3;


	[Header ("SphereMask")]
	public float Radius;
	public float SoftNess;

	[HideInInspector]
	public Slider BarMadness;
	[HideInInspector]
	public SpecialAction ThisAct;
	[HideInInspector]
	public int NbrLineRight = 1;
	[HideInInspector]
	public int NbrLineLeft = 1;
	[HideInInspector]
	public bool Dash = false;
	[HideInInspector]
	public bool Running = true;
	[HideInInspector]
	public bool blockChangeLine = false;
	[HideInInspector]
	public bool InMadness = false;
	public int Life = 1;
	public bool StopPlayer = false;

	private BoxCollider punchBox;
    private SphereCollider sphereChocWave;
	private Punch punch;
    private bool canPunch, punchRight;//, punchLeft, preparRight, preparLeft, defense;
    //private Coroutine corou/*, preparPunch*/;

    [Header("GRAPH")]
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject Plafond;

	[HideInInspector]
	public int currLine = 0;
    Transform pTrans;
	Rigidbody pRig;

	Direction currentDir = Direction.North;
	Direction newDir = Direction.North;
	Vector3 dirLine = Vector3.zero;
	Vector3 lastPos;
	//Vector3 posDir;
	Text textDist;
	Text textCoin;

	IEnumerator currWF;
	IEnumerator propPunch;
	Animator playAnimator;
	Slider SliderSlow;
	Camera thisCam;
	Punch getPunch;
    CameraFilterPack_Color_YUV camMad;
    Vector3 saveCamMad;

	Quaternion startRotRR;
	Vector3 startPosRM;

	float maxSpeedCL = 0;
	float maxSpeed = 0;
	float accelerationCL = 0;
	float decelerationCL = 0;
	float acceleration = 0;
	float impulsionCL = 0;
	float currSpeed = 0;
	float currSpLine = 0;

	float PropulseBalls = 100;
	float newH = 0;
	float newDist;
	float saveDist;
	float nextIncrease = 0;
	float befRot = 0;
	float SliderContent;
	float totalDis = 0;
    float rationUse = 1;
	//float calPos = 0;

	float valueSmooth = 0;
    float valueSmoothUse = 0;
	float timeToDP;
    float timerBeginMadness = 0;
	float getFOVDP;

	int LastImp = 0;
	int clDir = 0;

	//bool canJump = true;
	bool propP = false;
	bool propDP = false;
	bool newPos = false;
	bool resetAxeS = true;
	bool resetAxeD = true;
	bool inAir = false;
	bool canChange = true;
	bool invDamage = false;
	bool animeSlo = false;
	bool canSpe = true;
    bool canChocWave = true;
    [HideInInspector]
    public bool playerDead = false;
	bool dpunch = false;
    bool InBeginMadness = false;
	#endregion

	#region Mono
	void Start ( )
	{
		pTrans = transform;
		pRig = gameObject.GetComponent<Rigidbody> ( );
		punchBox = pTrans.GetChild(0).GetComponent<BoxCollider>();
        sphereChocWave = pTrans.GetChild(0).GetComponent<SphereCollider>();
        punch = pTrans.GetChild(0).GetComponent<Punch>();
        canPunch = true; 
		punchRight = true;
		getPunch = GetComponentInChildren<Punch> ( );
		thisCam = GetComponentInChildren<Camera> ( );
		SliderSlow = GlobalManager.Ui.MotionSlider;
		SliderContent = 10;
		SliderSlow.maxValue = 10;
		lastPos = pTrans.position;
		textDist = GlobalManager.Ui.ScorePoints;
		textCoin = GlobalManager.Ui.MoneyPoints;
		nextIncrease = DistIncMaxSpeed;
		maxSpeed = MaxSpeed;
		maxSpeedCL = MaxSpeedCL;
		accelerationCL = AccelerationCL;
		acceleration = Acceleration;
		impulsionCL = ImpulsionCL;
		decelerationCL = DecelerationCL;
		playAnimator = GetComponentInChildren<Animator> ( );
        camMad = GetComponentInChildren<CameraFilterPack_Color_YUV>();
        saveCamMad = new Vector3(camMad._Y, camMad._U, camMad._V);
        
		startRotRR = thisCam.transform.localRotation;
		startPosRM = thisCam.transform.localPosition;

        /* punchLeft = true; preparRight = false; preparLeft = false; defense = false;
		preparPunch = null;*/

        Plafond.GetComponent<MeshRenderer>().enabled = true;
    }

	void Update ( )
	{
		Shader.SetGlobalFloat ( "_emisive_force", 1 - (BarMadness.value / BarMadness.maxValue)*2 );



        float getTime = Time.deltaTime;

		rationUse = 1 + (RatioMaxMadness * (InMadness ? 1 : (BarMadness.value / BarMadness.maxValue)));
		punch.SetPunch ( !playerDead );

		distCal ( );
		playerAction ( getTime );

		Mathf.Clamp ( Radius, 0, 100 );
		Mathf.Clamp ( SoftNess, 0, 100 );

		Shader.SetGlobalVector ( "GlobaleMask_Position", new Vector4 ( pTrans.position.x, pTrans.position.y, pTrans.position.z, 0 ) );
		Shader.SetGlobalFloat ( "GlobaleMask_Radius", Radius );
		Shader.SetGlobalFloat ( "GlobaleMask_SoftNess", SoftNess );
		Shader.SetGlobalFloat ( "_SlowMot", Time.timeScale );

        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            sphereChocWave.enabled = true;
            StartCoroutine(CooldownWave());
            StartCoroutine(TimerHitbox());
        }*/

        SmoothBar();

        if (!InBeginMadness)
        {
            camMad._Y = saveCamMad.x * (BarMadness.value / BarMadness.maxValue);
            camMad._U = saveCamMad.y * (BarMadness.value / BarMadness.maxValue);
            camMad._V = saveCamMad.z * (BarMadness.value / BarMadness.maxValue);
            if (BarMadness.value - (getTime * DelayDownBar * (InMadness ? ratioDownInMadness : 1)) > 0)
            {
                BarMadness.value -= getTime * DelayDownBar * (InMadness ? ratioDownInMadness : 1);
            }
            else
            {
                BarMadness.value = 0;
            }
        }
        else
        {
            if(timerBeginMadness < delayInBeginMadness)
            {
                timerBeginMadness += Time.deltaTime;
                camMad._Y += saveCamMad.x * Time.deltaTime / delayInBeginMadness;
                camMad._U += saveCamMad.y * Time.deltaTime / delayInBeginMadness;
                camMad._V += saveCamMad.z * Time.deltaTime / delayInBeginMadness;
            }
            else
            {
                InBeginMadness = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
            PlayerPrefs.DeleteAll();

    }
	#endregion

	#region Public Functions
	public void UpdateNbrLine ( int NbrLineL, int NbrLineR )
	{
		//NbrLineLeft = NbrLineL 
	}

	public void ResetPlayer ( )
	{
		getFOVDP = FOVIncrease;
		Life = 1;
		playerDead = false;
		StopPlayer = true;
		totalDis = 0;
		maxSpeed = MaxSpeed;
		maxSpeedCL = MaxSpeedCL;
		accelerationCL = AccelerationCL;
		acceleration = Acceleration;
		impulsionCL = ImpulsionCL;
		decelerationCL = DecelerationCL;
		ThisAct = SpecialAction.Nothing;
		timeToDP = TimeToDoublePunch;
		BarMadness = GlobalManager.Ui.Madness;
		BarMadness.value = 0;
        NbrLineRight = 0;
        NbrLineLeft = 0;
		InMadness = false;
		stopMadness ( );

        BarMadness.value = 80;

	}

	public void GameOver ( bool forceDead = false )
	{

        GlobalManager.Ui.GameOver();

        if ( invDamage  && !forceDead )
		{
			return;
		}


        ScreenShake.Singleton.ShakeGameOver();

        GameOverTok thisTok = new GameOverTok ( );
		thisTok.totalDist = totalDis;

        StopPlayer = true;

		thisCam.GetComponent<RainbowMove>().enabled = false;
		thisCam.GetComponent<RainbowRotate>().enabled = false;

        Life--;

		if ( Life > 0 || playerDead )
		{
			invDamage = true;
			Invoke ( "waitInvDmg", TimeInvincible );


            if (!playerDead)
            {

                GlobalManager.Ui.StartBonusLife();
            }

            return;
		}

        DOVirtual.DelayedCall(.2f, () =>
        {

            thisCam.transform.DORotate(new Vector3(-220, 0, 0), 1.8f, RotateMode.LocalAxisAdd);
            thisCam.transform.DOLocalMoveZ(-50f, .4f);
        });



        playerDead = true;





        DOVirtual.DelayedCall(1f, () =>
        {

            GlobalManager.Ui.OpenThisMenu(MenuType.GameOver, thisTok);
            ScreenShake.Singleton.ShakeGameOver();

        });
        //GlobalManager.Ui.OpenThisMenu ( MenuType.GameOver );

        //GlobalManager.GameCont.Restart ( );
    }

    public void AddSmoothCurve(float p_value)
    {
        valueSmooth = valueSmoothUse + p_value;
        valueSmoothUse = valueSmooth;
    }

    public bool IsInMadness()
    {
        return InMadness;
    }
	#endregion

	#region Private Functions
	void playerAction ( float getTime )
	{
        if ( playerDead || StopPlayer )
		{
			thisCam.fieldOfView = Constants.DefFov;
			return;
		}
		float getDelta = Time.deltaTime;

		if( BarMadness.value == 0 && InMadness )
		{
            /*playAnimator.SetBool("InMadness", false);

			//stopMadness ( );
            InMadness = false;*/
            stopMadnessLeft();
		}

		if ( Running )
		{
			if ( currSpeed < maxSpeed )
			{
				currSpeed += acceleration * getTime;

            }
			else if ( currSpeed > maxSpeed )
			{
				currSpeed = maxSpeed;
			}
		}
		else
		{
			currSpeed -= Deceleration * getTime;

			if ( currSpeed < 0 )
			{
				currSpeed = 0;
			}

			currSpLine -= Deceleration * getTime;

			if ( currSpLine < 0 )
			{
				currSpLine = 0;
			}
		}

		if ( !playerDead && !InBeginMadness)
		{
			if ( Input.GetAxis ( "CoupSimple" ) == 0 && !Dash)
			{
				resetAxeS = true;
			}

			if ( Input.GetAxis ( "CoupDouble" ) == 0 && !Dash )
			{
				resetAxeD = true;
				getFOVDP = FOVIncrease;


                if ( timeToDP < TimeToDoublePunch * 0.35f )
				{
					resetAxeD = false;
					dpunch = true;

                    playAnimator.SetBool("ChargingPunch", true);
                    playAnimator.SetBool("ChargingPunch_verif", true);

                }
                else
				{
                    playAnimator.SetBool("ChargingPunch_verif", false);
                    playAnimator.SetBool("ChargingPunch", false);

                    timeToDP = TimeToDoublePunch;
                }
			}

            if (Input.GetAxis("CoupDouble") < 0.02 && Input.GetAxis("CoupDouble") > 0.01 && resetAxeD && !Dash)
            {
            }
                if ( Input.GetAxis ( "CoupDouble" ) != 0 && resetAxeD && !Dash )
			{
				float calcRatio = ( FOVIncrease / TimeToDoublePunch ) * getDelta;


                if ( timeToDP == TimeToDoublePunch)
                {
                    playAnimator.SetBool("ChargingPunch_verif", true);
                    playAnimator.SetBool("ChargingPunch", true);
                    playAnimator.SetTrigger("Double");
                    Debug.Log("trigger");
                }


                timeToDP -= getDelta;


				getFOVDP -= calcRatio;



				if ( getFOVDP > 0 )
				{

                    Debug.Log("Charging");

                    thisCam.fieldOfView += calcRatio;
				}
				else
				{
					getFOVDP = 0;
				}

				if ( timeToDP <= 0 )
				{
					getFOVDP = FOVIncrease;
					timeToDP = 0;


                    Debug.Log("Not Charging");

                    resetAxeD = false;
					dpunch = true;

                    
                }
			}
			else
			{
				if ( !dpunch && thisCam.fieldOfView > Constants.DefFov )
				{
					thisCam.fieldOfView -= getDelta * 10;

					if ( thisCam.fieldOfView < Constants.DefFov )
					{
						thisCam.fieldOfView = Constants.DefFov;
					}
				}
			}

            playerFight ( );
		}

		if ( Input.GetAxis ( "Dash" ) != 0 && newH == 0 && !InMadness && !InBeginMadness && !playerDead )
		{
			Dash = true;
		}
		else
		{
			Dash = false;
		}

		checkInAir ( getTime );

		speAction ( getTime );

		changeLine ( getTime );

		playerMove ( getTime, currSpeed );
	}

	void distCal ( )
	{
		totalDis += Vector3.Distance ( lastPos, pTrans.position );
		lastPos = pTrans.position;
		textDist.text = "" + Mathf.RoundToInt ( totalDis );

		if ( totalDis > nextIncrease )
		{
			nextIncrease += DistIncMaxSpeed;

			if ( MaxSpeedInc > MaxSpeed - maxSpeed )
			{
				maxSpeed += SpeedIncrease;
				acceleration += AcceleraInc;
			}
			else
			{
				maxSpeed = SpeedIncrease;
			}

			if ( MaxCLInc > maxSpeedCL - MaxSpeedCL )
			{
				maxSpeedCL += CLSpeedIncrease;
				accelerationCL += AcceleraCLInc;
				impulsionCL += AcceleraCLInc;
				decelerationCL += AcceleraCLInc;
			}
			else
			{
				maxSpeedCL = MaxCLInc;
			}
		}
	}

	void speAction ( float getTime )
	{
		if ( ThisAct == SpecialAction.SlowMot )
		{
			if ( Input.GetAxis ( "SpecialAction" ) > 0 && canSpe && SliderContent > 0 && !Dash )
			{
				Camera.main.GetComponent<CameraFilterPack_Vision_Aura>().enabled = true;

				if ( !animeSlo )
				{
					animeSlo = true;
					GlobalManager.Ui.StartSlowMo();
				}

				if ( Time.timeScale > 1 / SlowMotion )
				{
					Time.timeScale -= Time.deltaTime * SpeedSlowMot;
				}

				SliderContent -= ReduceSlider * Time.deltaTime;
			}
			else if ( Time.timeScale < 1 )
			{
				if ( SliderContent < 0 )
				{
					canSpe = false;
					SliderContent = 0;
				}

				Time.timeScale += getTime * SpeedDeacSM;
			}
			else if ( SliderContent < 10 )
			{
				animeSlo = false;
				Time.timeScale = 1;
				SliderContent += RecovSlider * getTime;
				Camera.main.GetComponent<CameraFilterPack_Vision_Aura>().enabled = false;

				if ( SliderContent > 2 )
				{
					canSpe = true;
				}

			}
			else
			{
				canSpe = true;
				SliderContent = 10;
			}

			SliderSlow.value = SliderContent;
		}
	}

	void waitInvDmg ( )
	{
		invDamage = false;
	}

	bool getCamRM = false;
	void checkInAir ( float getTime )
	{
		RaycastHit[] allHit;
		bool checkAir = true;

		allHit = Physics.RaycastAll ( pTrans.position, Vector3.down, 2 );
		if ( Dash || InMadness )
		{
			getTime *= DashSpeed;
		}

		foreach ( RaycastHit thisRay in allHit )
		{
			if ( thisRay.collider.gameObject == gameObject )
			{
				continue;
			}
			checkAir = false;

			if ( thisRay.collider.gameObject.layer == 9 )
			{
				Transform getThis = thisRay.collider.transform;

				if ( getThis.rotation.x < 0 )
				{
					pTrans.Translate ( new Vector3 ( 0, ( ( 360 - getThis.eulerAngles.x ) / 4  ) * getTime, 0 ), Space.World );
					pRig.useGravity = false;
				}
				else if ( getThis.rotation.x > 0 )
				{
					pTrans.Translate ( new Vector3 ( 0, ( -getThis.eulerAngles.x / 4 ) * getTime * 2, 0 ), Space.World );
					pRig.useGravity = true;
				}
			}
		}

		if ( checkAir )
		{
            //pRig.useGravity = true;
			if ( !getCamRM )
			{
				getCamRM = true;

				if ( currWF != null )
				{
					StopCoroutine ( currWF );
				}

				currWF = waitFall ( );
				StartCoroutine ( currWF );
			}
            // Camera.main.GetComponent<RainbowMove>().enabled = false;

			pRig.AddForce ( Vector3.down * BonusGrav * getTime, ForceMode.VelocityChange );
        }
		else if ( !checkAir && getCamRM )
        {
			if ( currWF != null )
			{
				StopCoroutine ( currWF );
				currWF = null;
			}

			getCamRM = false;

			thisCam.GetComponent<RainbowMove> ( ).reStart ( );
			thisCam.GetComponent<RainbowRotate> ( ).reStart ( );
			thisCam.GetComponent<RainbowMove>().enabled = true;
			thisCam.GetComponent<RainbowRotate>().enabled = true;
           // ScreenShake.Singleton.ShakeFall();
        }

		inAir = checkAir;
	}

	IEnumerator waitFall ( )
	{
		yield return new WaitForSeconds ( 0.5f );

		currWF = null;
		thisCam.transform.localRotation = startRotRR;
		thisCam.transform.localPosition = startPosRM; 
		thisCam.GetComponent<RainbowMove>().enabled = false;
		thisCam.GetComponent<RainbowRotate>().enabled = false;
	}

	void playerMove ( float delTime, float speed )
	{
		Transform transPlayer = pTrans;
		Vector3 calTrans = Vector3.zero;
		delTime = Time.deltaTime;

		if ( inAir )
		{
			speed = ( speed / 100 ) * PourcRal;
		}
		/*else
		{
			canJump = true;
		}*/

		if ( Dash && !playerDead && !InMadness && !InBeginMadness )
		{
			speed *= DashSpeed;

			GlobalManager.Ui.DashSpeedEffect ( true );
			Camera.main.GetComponent<CameraFilterPack_Blur_BlurHole> ( ).enabled = true;
		}
		else
		{
			GlobalManager.Ui.DashSpeedEffect ( false );
			Camera.main.GetComponent<CameraFilterPack_Blur_BlurHole>().enabled = false;

			if ( propP )
			{
				speed *= SpeedPunchRun;
			}
			else if ( propDP )
			{
				speed *= SpeedDoublePunchRun;
			}
		}

		float calCFov = Constants.DefFov * ( speed / maxSpeed );

		if ( timeToDP == TimeToDoublePunch )
		{
			if ( !inAir )
			{
				Shader.SetGlobalFloat ( "_ReduceVis", speed / maxSpeed );

				if ( thisCam.fieldOfView < calCFov )
				{
					thisCam.fieldOfView += Time.deltaTime * SpeedEffectTime;
					if ( thisCam.fieldOfView > calCFov )
					{
						thisCam.fieldOfView = calCFov;
					}
				}
				else if ( thisCam.fieldOfView > calCFov )
				{
					thisCam.fieldOfView -= Time.deltaTime * SpeedEffectTime * 2;
					if ( thisCam.fieldOfView < calCFov )
					{
						thisCam.fieldOfView = calCFov;
					}
				}
			}
			else
			{
				if ( thisCam.fieldOfView < Constants.DefFov )
				{
					thisCam.fieldOfView += Time.deltaTime * SpeedEffectTime;
					if ( thisCam.fieldOfView > Constants.DefFov )
					{
						thisCam.fieldOfView = Constants.DefFov;
					}
				}
				else if ( thisCam.fieldOfView > Constants.DefFov )
				{
					thisCam.fieldOfView -= Time.deltaTime * SpeedEffectTime * 2;
					if ( thisCam.fieldOfView < Constants.DefFov )
					{
						thisCam.fieldOfView = Constants.DefFov;
					}
				}
			}
		}

		if ( currentDir == Direction.North )
		{
			calTrans = Vector3.forward * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 0, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.South )
		{
			calTrans = Vector3.back  * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 180, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.East )
		{
			calTrans = Vector3.right * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 90, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.West )
		{
			calTrans = Vector3.left * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, -90, 0 ) ), RotationSpeed * delTime );
		}

		if ( newPos )
		{
			befRot -= speed * delTime;

			if ( befRot < 0 )
			{
				newPos = false;
				currentDir = newDir;
				pTrans.Translate ( pTrans.forward * befRot, Space.World );
			}
		}

		pTrans.Translate ( calTrans / (InBeginMadness ? slowInBeginMadness : 1), Space.World );

		/*if ( canJump && Input.GetAxis ( "Jump" ) > 0 )
		{
			canJump = false;
			pRig.AddForce ( transPlayer.up * JumpForce, ForceMode.Impulse );
		}*/
	}

	void changeLine ( float delTime )
	{
		float newImp = Input.GetAxis ( "Horizontal" );
		float lineDistance = Constants.LineDist;

		if ( ( canChange || newH == 0 ) && !inAir && !blockChangeLine && !InBeginMadness)
		{
			if ( newImp == 1 && LastImp != 1 && currLine + 1 <= NbrLineRight && ( clDir == 1 || newH == 0 ) )
			{
                if(Time.timeScale < 1)
                {
                    Time.timeScale = 1;
                }
				canChange = false;
				currLine++;
				LastImp = 1;
				clDir = 1;
				newH = newH + lineDistance;
				saveDist = newH;
			}
			else if ( newImp == -1 && LastImp != -1 && currLine - 1 >= -NbrLineLeft && ( clDir == -1 || newH == 0 ) )
			{
                if (Time.timeScale < 1)
                {
                    Time.timeScale = 1;
                }
				canChange = false;
				currLine--;
				LastImp = -1;
				clDir = -1;
				newH = newH - lineDistance;
				saveDist = newH;
			}
			else if ( newImp == 0 )
			{
				LastImp = 0;
			}
		}

		if ( newH != 0 )
		{
			if ( Running )
			{
				float accLine = 0;

				if ( saveDist < 0 && newH > -lineDistance / 1.25f || saveDist > 0 && newH < lineDistance / 1.25f )
				{
					canChange = true;
				}

				if ( saveDist < 0 && newH > -lineDistance / 4 || saveDist > 0 && newH < lineDistance / 4 )
				{
					currSpLine -= decelerationCL * delTime;

					if ( currSpLine < 0 )
					{
						currSpLine = 0.1f;
					}
				}
				else if ( currSpLine < maxSpeedCL )
				{
					accLine = ( currSpLine * impulsionCL ) / maxSpeedCL; 

					if ( accLine > 1 || accLine == 0 )
					{
						accLine = 1;
					}

					currSpLine += accelerationCL * accLine * delTime;
				}
				else if ( currSpLine > maxSpeedCL )
				{
					currSpLine = maxSpeedCL;
				}
			}

			float calTrans = clDir * currSpLine * delTime;

			if ( inAir )
			{
				calTrans = ( calTrans / 100 ) * PourcRal;
			}
			newH -= calTrans;

			if ( saveDist > 0 && newH - calTrans < 0 || saveDist < 0 && newH > 0 )
			{
				calTrans += newH;
				newH = 0;
				currSpLine = 0;
			}

			//Debug.Log ( currSpLine );

			dirLine = pTrans.right * calTrans;
			pTrans.Translate ( dirLine, Space.World );
		}
		else
		{
			currSpLine = 0;
		}
	}

	void playerFight ( )
	{
		if ( Input.anyKeyDown && Input.GetAxis ( "SpecialAction" ) == 0 && Input.GetAxis ( "Horizontal" ) == 0 )
		{
			/*if (InMadness)
			{
				if (BarMadness.value - LessPointPunchInMadness < 0)
				{
					BarMadness.value = 0;
				}
				else
				{
					BarMadness.value -= LessPointPunchInMadness;
                    ScreenShake.Singleton.ShakeMad();
                    Camera.main.GetComponent<CameraFilterPack_Distortion_Dream2>().Distortion = (BarMadness.value/10);
                }
			}*/
		}

		if(Input.GetAxis("CoupSimple") != 0 && canPunch && resetAxeS  )
        {
            resetAxeS = false;
            canPunch = false;
            propP = true;
			timeToDP = TimeToDoublePunch;
            if (Time.timeScale < 1)
                Time.timeScale = 1;

			//if ( !InMadness )
			//{
				punch.MadnessMana("Simple");

            int randomSong = UnityEngine.Random.Range(0, 3);
            GlobalManager.AudioMa.OpenAudio(AudioType.Other, "PunchFail_" + (randomSong + 1), false);
            //}

            ScreenShake.Singleton.ShakeHitSimple();
       
            if (punchRight)
            {
                punch.RightPunch = true;

				playAnimator.SetTrigger("Right");

				GlobalManager.Ui.SimpleCoup();
            }
            else
            {
                punch.RightPunch = false;

				playAnimator.SetTrigger("Left");
            }
            punchRight = !punchRight;
			StartCoroutine( StartPunch ( 0 ) );
            propPunch = propulsePunch(TimePropulsePunch);
            StartCoroutine(propPunch);
		}
		else if( dpunch && canPunch )
        {
            playAnimator.SetBool("ChargingPunch", false);

            dpunch = false;
			canPunch = false;

            ScreenShake.Singleton.ShakeHitDouble();

            GlobalManager.Ui.DoubleCoup();


            if (Time.timeScale < 1)
                Time.timeScale = 1;
			//if ( !InMadness )
			//{
				punch.MadnessMana("Double");
			//}

            propDP = true;
			StartCoroutine ( StartPunch ( 1/*, timeToDP */) );

			propPunch = propulsePunch ( TimePropulseDoublePunch );
			StartCoroutine ( propPunch );

			timeToDP = TimeToDoublePunch;
        }
	}

	private IEnumerator StartPunch(int type_technic/*, float getTDp = 0*/ )
	{
		yield return new WaitForSeconds(DelayPrepare / rationUse);
		 
		if ( type_technic == 1 )
		{
			//float getPourc = ( ( TimeToDoublePunch - getTDp ) * 100 ) / TimeToDoublePunch;
			punch.setTechnic ( type_technic );
		}
		else
		{
			punch.setTechnic ( type_technic );
		}

        punchBox.enabled = true;
       /* corou =*/ StartCoroutine("TimerHitbox");

        Shader.SetGlobalFloat("_saturation", BarMadness.value);

		StartCoroutine ( CooldownPunch ( type_technic ) );

        /*if (InMadness)
        {
            if (BarMadness.value - LessPointPunchInMadness < 0)
            {
                BarMadness.value = 0;
            }
            else
            {
                   
                BarMadness.value -= LessPointPunchInMadness;
            }
            //AddSmoothCurve(-LessPointPunchInMadness);
        }*/
	}

	private IEnumerator CooldownPunch ( int type_technic )
    {
		if ( type_technic == 1 )
		{
			yield return new WaitForSeconds ( ( DelayPunch * 4 ) / rationUse );
		}
		else
		{
			yield return new WaitForSeconds(DelayPunch / rationUse);
		}

		canPunch = true;
    }

	private IEnumerator TimerHitbox()
	{
		yield return new WaitForSeconds(DelayHitbox);
		punchBox.enabled = false;
        sphereChocWave.enabled = false;
	}

    IEnumerator CooldownWave()
    {
        yield return new WaitForSeconds(delayChocWave);
        canChocWave = true;
    }

	IEnumerator propulsePunch ( float thisTime )
	{
		WaitForSeconds thisSec = new WaitForSeconds ( thisTime );

		yield return thisSec;

		propP = false;
		propDP = false;
	}

	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._NewDirec )
		{
			newPos = true;
			newDir = thisColl.GetComponent<NewDirect> ( ).NewDirection;
			blockChangeLine = false;
			Vector3 getThisC = thisColl.transform.position;
			getThisC = new Vector3 ( getThisC.x, 0, getThisC.z );

			Vector3 getPtr = pTrans.position;
			getPtr = new Vector3 ( getPtr.x, 0, getPtr.z );

			befRot = Vector3.Distance ( getThisC, getPtr );
		} 
	}

	void OnCollisionEnter ( Collision thisColl )
	{
		GameObject getObj = thisColl.gameObject;

		if ( Dash || InMadness )
		{
			if ( getObj.tag == Constants._EnnemisTag || getObj.tag == Constants._ElemDash )
			{
				GlobalManager.Ui.BloodHit ( );

				/*Vector3 getProj = getPunch.projection_basic;

				if ( Random.Range ( 0,2 ) == 0 )
				{
					getProj.x *= Random.Range ( -getProj.x, -getProj.x / 2 );
				}
				else
				{
					getProj.x *= Random.Range ( getProj.x / 2, getProj.x );
				}*/
				thisColl.collider.enabled = false;
                if(thisColl.gameObject.GetComponent<AbstractObject>())
				    thisColl.gameObject.GetComponent<AbstractObject> ( ).ForceProp ( getPunch.projection_dash );
				return;
			}
			else if ( getObj.tag == Constants._Balls )
			{
				StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( getObj, pTrans, PropulseBalls, 1, 5, true ) );
				return;
			}
		}
		else if ( getObj.tag == Constants._ElemDash )
		{
			GameOver ( );
		}

		if ( getObj.tag == Constants._MissileBazoo )
		{
			getObj.GetComponent<MissileBazooka> ( ).Explosion ( );
			GameOver ( );
		}
		else if ( getObj.tag == Constants._EnnemisTag || getObj.tag == Constants._MissileBazoo || getObj.tag == Constants._Balls )
		{
			GameOver ( );
		}
		else if ( getObj.tag == Constants._ObsTag )
		{
			Life = 0;
			GameOver ( true );
		}
	}

	void stopMadness ( )
	{
		InMadness = false;
        

		maxSpeed = MaxSpeed;
		maxSpeedCL = MaxSpeedCL;
		accelerationCL = AccelerationCL;
		acceleration = Acceleration;

		GlobalManager.Ui.CloseMadness();
	}

    void stopMadnessLeft()
    {
        Debug.Log("val = " + delayInEndMadness);
        InMadness = false;
        playAnimator.SetBool("InMadness", false);
        GlobalManager.Ui.CloseMadness();
        DOTween.To(() => maxSpeed,
            x => {
                maxSpeed = x;
                Debug.Log("val maxSpeed = "+maxSpeed);
            },
            MaxSpeed,
            delayInEndMadness
        );
        DOTween.To(() => maxSpeedCL,
            x => maxSpeedCL = x,
            MaxSpeedCL,
            delayInEndMadness
        );
        DOTween.To(() => accelerationCL,
            x => accelerationCL = x,
            AccelerationCL,
            delayInEndMadness
        );
        DOTween.To(() => acceleration,
            x => acceleration = x,
            Acceleration,
            delayInEndMadness
        );
    }

    private void SmoothBar()
    {
        float res = valueSmoothUse * (Time.deltaTime * SmoothSpeed);
        if(BarMadness.value + res <= 0)
        {
            BarMadness.value = 0;
            valueSmooth = 0;
            valueSmoothUse = 0;

            if (InMadness)
            {
                stopMadnessLeft();
            }
        }else if (BarMadness.value + res >= 100)
        {
            //Debug.Log("first etape");
            BarMadness.value = 100;
            valueSmooth = 0;
            valueSmoothUse = 0;

            if (!InMadness)
            {
                GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MadnessBegin", false);
                InMadness = !InMadness;
                InBeginMadness = true;
                timerBeginMadness = 0;
                camMad._Y = 0; camMad._U = 0; camMad._V = 0;

                //camMad._Y = saveCamMad.x; camMad._U = saveCamMad.y; camMad._V = saveCamMad.z;

                maxSpeedCL = MaxSpeedCL * 2;
				maxSpeed = MaxSpeed * 3;
				accelerationCL = AccelerationCL * 2;
				acceleration = Acceleration * 4;

                GlobalManager.Ui.OpenMadness();
            }
        }
        else
        {
            BarMadness.value += res;
            valueSmoothUse -= res;
        }
    }
	#endregion
}
