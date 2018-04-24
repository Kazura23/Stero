
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Rewired;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour 
{
	public PlayableDirector PlayDirect;
	public float TimeToFall = 0.5f;
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
	[HideInInspector]
	public float delayChocWave = 5;
	//[HideInInspector]
	//public float DelayDeadBall = 15;
	public float DistDBTake = 25;
	public GameObject DeadBallPref;

	[Header ("TimerCaract")]
	public float DelayTimerStandard = 5;
	[Tooltip ("Delay de survie une fois le timer à 0")]
	public float DelayTimerToDeath = 2.5f;
	public float DelayTimerToMad = 2.5f;
	public float DelayTimerOnMadness = 2.5f;
	[Tooltip ("Pourcentage de récupération pour chaque ennemis mort")]
	public float TimerRecover = 10;
	[Tooltip ("Pourcentage de récupération pour chaque ennemis mort dans le rouge")]
	public float TimerLastRecover = 10;
	[Tooltip ("Pourcentage de récupération pour chaque ennemis mort dans le vert")]
	public float TimerSecureRecover = 10;
	[Tooltip ("Pourcentage de récupération pour chaque ennemis mort pendant la madness")]
	public float TimerRecoverOnMadness = 10;

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
	//public float DashTime = 1.5f;
	[Tooltip ("La valeur de DashSpeed est un multiplicateur sur la vitesse du joueur")]
	public float DashSpeed = 2.5f;
	[Tooltip ("La valeur de DashSpeed est un multiplicateur sur la vitesse du joueur")]
	public float MadnessSpeed = 1.3f;
	[Tooltip ("Temps d'invicibilité apres avoir pris des dégats")]
	public float TimeInvincible = 2;

	[HideInInspector]
	public float SlowMotion, MadnessUse, MadnessMult, MadNeed;

	[Header ("Caractérique punchs")]
	//public float FOVIncrease = 20;
	public float TimeToDoublePunch = 0.25f;
    public float DelayDoublePunch = .05f;
    public float CooldownDoublePunch = 1;

	//[HideInInspector]
	//public Slider BarMadness;
	public SpecialAction ThisAct;
	[HideInInspector]
	public int NbrLineRight = 1;
	[HideInInspector]
	public int NbrLineLeft = 1;
	[HideInInspector]
	public bool Dash = false;
	[HideInInspector]
	public bool InMadness = false;
	[HideInInspector]
	public Slider SliderSlow;
    [HideInInspector]
    public bool playerInv = false;
    public int Life = 1;
	public bool StopPlayer = false;

	[HideInInspector]
	public float totalDis = 0;

	public BoxCollider punchBoxSimple;
    private SphereCollider sphereChocWave;
	private Punch punch;
    private bool canPunch, punchRight;

    [Header("GRAPH")]
    public GameObject leftHand;
    public GameObject rightHand;

	[HideInInspector]
	public int currLine = 0;
	[HideInInspector]
	public bool blockChangeLine = false;

	[HideInInspector]
	public int CurrNbrBall = 0;

	[HideInInspector]
	public bool onBonusStart = false;
	Transform pTrans;
	Rigidbody pRig;
	RigidbodyConstraints thisConst;

	Vector3 dirLine = Vector3.zero;
	Vector3 lastPos;

	string textDist;

	IEnumerator currWF;
	IEnumerator thisEnum;
	IEnumerator getCouldown;

	public Animator playAnimator;

	Camera thisCam;
	Camera otherCam;

	Transform pivotTrans;
	Punch getPunch;
    CameraFilterPack_Color_YUV camMad;
    Vector3 saveCamMad;

	IEnumerator geTimerP;
	Quaternion startRotRR;
	Quaternion startRotPlayer;
	Vector3 startPosRM;
	Vector3 startPlayer;
    Player inputPlayer;
	Slider timerFight;
	Image backTF;
    Image handleTF;

	AudioSource barMadSource;

	float checkDistY = -100;
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
	//float newDist;
	float saveDist;
	float nextIncrease = 0;
    float rationUse = 1;

	float valueSmooth = 0;
    float valueSmoothUse = 0;
	float timeToDP;
	//float getFOVDP;

	int LastImp = 0;
	int clDir = 0;

	bool Running = true;
	bool newPos = false;
	bool resetAxeS = true;
	bool resetAxeD = true;
	bool inAir = false;
	bool canChange = true;
	bool invDamage = false;
	bool animeSlo = false;
	bool canSpe = true;
    [HideInInspector]
    public bool playerDead = false;
	bool dpunch = true;
	bool chargeDp = false;
	bool canUseDash = true;
    bool onAnimeAir = false;
	bool lastTimer = false;
	bool secureTimer = false;
	bool useFord = true;
	bool getCamRM = false;
	bool newDir = false;
	bool onTuto;
   	//private int[] enemyKill;
    //private string[] deadType;

    #endregion

    #region Mono
    void Update ( )
	{
		#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            MaxSpeed = 1f;
            acceleration = 1;
        }

        if (Input.GetKeyDown(KeyCode.R))
            GlobalManager.GameCont.Restart();

			
        #endif

        float getTime = Time.deltaTime;

		rationUse = 1;

		if ( InMadness )
		{
			rationUse += timerFight.value;
		}

		punch.SetPunch ( !playerDead );

		playerAction ( getTime );

		Shader.SetGlobalFloat ( "_SlowMot", Time.timeScale );

        if (Input.GetKeyDown(KeyCode.O))
            PlayerPrefs.DeleteAll();

//		Debug.Log ( NbrLineRight + " / " + currLine + " / " + NbrLineLeft );

    }
	#endregion

	int getCull;
	CameraClearFlags thisClear;
	Color thisColor ;
	#region Public Functions
	public void IniPlayer ( )
	{
		barMadSource = GlobalManager.AudioMa.GetSource(AudioType.Madnesse);
		pTrans = transform;
		timerFight = GlobalManager.Ui.Madness;
		timerFight.value = 0.5f;
		backTF = timerFight.transform.GetChild(1).transform.GetChild(0).GetComponent<Image> ( );
        handleTF = timerFight.transform.GetChild(2).transform.GetChild(0).GetComponent<Image> ( );
		pRig = gameObject.GetComponent<Rigidbody> ( );
		thisConst =	pRig.constraints;
		sphereChocWave = pTrans.Find("ChocWave").GetComponent<SphereCollider>();
		punch = pTrans.GetChild(0).GetComponent<Punch>();
		canPunch = true; 
		punchRight = true;

		getPunch = GetComponentInChildren<Punch> ( );
		SliderSlow = GlobalManager.Ui.MotionSlider;
		lastPos = pTrans.position;
		textDist = GlobalManager.Ui.ScoreString;
	
		camMad = GetComponentInChildren<CameraFilterPack_Color_YUV>();
		saveCamMad = new Vector3(camMad._Y, camMad._U, camMad._V);

		inputPlayer = ReInput.players.GetPlayer(0);



        //Rewired.ReInput.mapping.GetKeyboardMapInstance(0, 0).GetElementMaps()[0];
        //inputPlayer.controllers.maps.GetMap(0).ReplaceElementMap(0, 0, Pole.Positive, KeyCode.A, ModifierKeyFlags.None);

		thisCam = GlobalManager.GameCont.thisCam;
		otherCam = thisCam.transform.Find ( "OtherCam" ).GetComponent<Camera> ( ); 
		pivotTrans = thisCam.transform.parent;

		startRotRR = thisCam.transform.localRotation;
		startPosRM = thisCam.transform.localPosition;
		startPlayer = pTrans.localPosition;
		startRotPlayer = pTrans.localRotation;

		getCull = thisCam.cullingMask;
		thisClear = thisCam.clearFlags;
		thisColor = thisCam.backgroundColor;
	}

	public void ResetPlayer ( )
	{
		onBonusStart = false;
		if ( getCouldown != null )
		{
			StopCoroutine ( getCouldown );
		}
        GlobalManager.AudioMa.CloseAudio(AudioType.Madnesse);

        GlobalManager.Ui.MadnessGreenEnd();

        textDist = "0";
		GlobalManager.Ui.UpdateScore();
		SliderSlow.value = SliderSlow.maxValue;
		onTuto = GlobalManager.GameCont.LaunchTuto;

		newStat ( StatePlayer.Normal );
		timerFight.DOValue ( 0.5f, Mathf.Abs ( timerFight.value - 0.5f ) );
		backTF.color = Color.white;
		lastTimer = false;
		secureTimer = false;
		newPos = false;
		canPunch = true; 
		punchRight = true;
		//getFOVDP = FOVIncrease;
		Life = 1;
		playerDead = false;
		StopPlayer = true;
		totalDis = 0;
		nextIncrease = DistIncMaxSpeed;

		if ( onTuto )
		{
			acceleration = Acceleration * 0.5f;
			maxSpeed = MaxSpeed * 0.5f;
        }
		else
		{
			acceleration = Acceleration;
			maxSpeed = MaxSpeed;
		}

		maxSpeedCL = MaxSpeedCL;
		accelerationCL = AccelerationCL;
		impulsionCL = ImpulsionCL;
		decelerationCL = DecelerationCL;
		timeToDP = TimeToDoublePunch;
        NbrLineRight = 0;
        NbrLineLeft = 0;
		newH = 0;
		currLine = 0;
		canChange = false;
		InMadness = false;
		pRig.constraints = RigidbodyConstraints.FreezeAll;
		playAnimator.Play ( "Start" );
		totalDis = 0;
		thisCam.GetComponent<RainbowMove> ( ).reStart ( );
		thisCam.GetComponent<RainbowRotate> ( ).reStart ( );
		currSpeed = 0;
		thisCam.transform.localRotation = startRotRR;
		thisCam.transform.localPosition = startPosRM;
		pTrans.localPosition = startPlayer;
		pTrans.localRotation = startRotPlayer;
		lastPos = startPlayer;
		canSpe = true;
		thisCam.clearFlags = thisClear;
		thisCam.cullingMask = getCull;
		thisCam.backgroundColor = thisColor;
	}

	public void ResetPosDo ( )
	{
		GlobalManager.Ui.DashSpeedEffect ( false );
		GlobalManager.Ui.MadnessRedEnd();
		GlobalManager.Ui.MadnessGreenEnd();


		StopPlayer = true;

		GlobalManager.GameCont.LaunchTuto = false;
		AllPlayerPrefs.relance = false;

		thisCam.GetComponent<RainbowMove> ( ).enabled = false;
		thisCam.GetComponent<RainbowRotate> ( ).enabled = false;
		thisCam.GetComponent<CameraFilterPack_Blur_BlurHole> ( ).enabled = false;

		thisCam.transform.DOLocalRotateQuaternion ( startRotRR, 0.5f );
		thisCam.transform.DOLocalMove ( startPosRM, 0.5f );

		pTrans.DOLocalRotateQuaternion ( startRotPlayer, 0.5f );
		pTrans.DOLocalMove ( startPlayer, 0.6f ).OnComplete ( ( ) =>
		{
			playAnimator.Play("Start");
			GlobalManager.GameCont.Restart ( );
		} );
	}

    public void GameOver ( bool forceDead = false )
	{
        if ( invDamage  && !forceDead )
		{
			return;
		}

		if ( onTuto )
		{
			currSpeed = 0;
			StopPlayer = true;

			pTrans.DOLocalMove ( pTrans.localPosition - pTrans.forward * 10, 1 ).OnComplete ( ( ) =>
			{
				textDist = "" + ( int.Parse ( textDist ) - 10 );
				totalDis -= 10;
				calDist = (int)totalDis;
				lastPos = pTrans.position;
				StopPlayer = false;
				GlobalManager.Ui.UpdateScore();
			} );
			return;
		}

		Life--;

        GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(1, 1);

		if ( Life > 0 || playerDead )
		{
			invDamage = true;
			Invoke ( "waitInvDmg", TimeInvincible );

			if ( !playerDead )
			{
				GlobalManager.Ui.StartBonusLife ( Life + 1 );
			}

			return;
		}

		thisCam.clearFlags = otherCam.clearFlags;
		thisCam.cullingMask = otherCam.cullingMask;
		thisCam.DOColor ( otherCam.backgroundColor, 0.3f );

        AllPlayerPrefs.distance = totalDis;

		StopPlayer = true;



		GameOverTok thisTok = new GameOverTok ( );
		thisTok.totalDist = AllPlayerPrefs.finalScore + totalDis;

		stopMadness ( );

        DOVirtual.DelayedCall(.2f, () =>
        {
            thisCam.transform.DORotate(new Vector3(-220, 0, 0), 1.8f, RotateMode.LocalAxisAdd);
            thisCam.transform.DOLocalMoveZ(-50f, .4f);
        });

        playerDead = true;
		thisCam.fieldOfView = Constants.DefFov;

		thisCam.GetComponent<RainbowMove>().enabled = false;
		thisCam.GetComponent<RainbowRotate>().enabled = false;
		thisCam.GetComponent<CameraFilterPack_Blur_BlurHole> ( ).enabled = false;

        GlobalManager.GameCont.soundFootSteps.Kill();

        DOVirtual.DelayedCall(1f, () =>
        {
            GlobalManager.Ui.OpenThisMenu(MenuType.GameOver, thisTok);

			thisCam.clearFlags = thisClear;
			thisCam.cullingMask = getCull;
			thisCam.backgroundColor = thisColor;
        });
		GlobalManager.Ui.GameOver();
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

	public void GetPunchIntro(  )
    {
		if ( StopPlayer && /*Input.GetAxis("CoupSimple") != 0 && */canPunch /* && resetAxeS*/ )
		{
			resetAxeS = false;
			canPunch = false;
			timeToDP = TimeToDoublePunch;
			if ( Time.timeScale < 1 )
				Time.timeScale = 1;

			ScreenShake.Singleton.ShakeIntro ( );
            
			GlobalManager.AudioMa.OpenAudio ( AudioType.Other, "PunchSuccess", false );

			if ( punchRight )
			{
				punch.RightPunch = true;

				playAnimator.SetTrigger ( "Right" );
			}
			else
			{
				punch.RightPunch = false;

				playAnimator.SetTrigger ( "Left" );
			}
			punchRight = !punchRight;
			punchBoxSimple.enabled = true;
			startPunch ( 0 );
		}
    }

	void startPunch ( int tech )
	{
		if ( geTimerP != null )
		{
			StopCoroutine ( geTimerP );
		}

		if ( getCBP != null )
		{
			StopCoroutine ( getCBP );
		}

		geTimerP = TimerHitbox ( tech );

		StartCoroutine ( geTimerP );

		punch.setTechnic ( tech );


		getCBP = CooldownPunch ( tech );
		StartCoroutine ( getCBP );
	}

	IEnumerator getCBP;
	public void StopCDPunch ()
	{
		StopCoroutine ( getCBP );
		playAnimator.SetBool("DoublePunchEnd", true);
		canPunch = true;
	}

	public void RecoverTimer ( DeathType thisDeath, int nbrPoint, float bonus )
	{
		if ( playerDead || StopPlayer )
		{
			return;
		}

		if ( !InMadness || thisDeath == DeathType.Enemy )
		{
			GlobalManager.GameCont.NewScore ( thisDeath, nbrPoint );

        }
		else
		{
			GlobalManager.GameCont.NewScore ( DeathType.Madness, nbrPoint );
		}

		if ( bonus <= 0 )
		{
			bonus = 1;
		}

		if ( thisDeath == DeathType.Acceleration )
		{
			bonus *= 0.5f;
		}
		else if ( thisDeath == DeathType.SpecialPower )
		{
			bonus *= MadnessMult;
		}

		float getCal;
		if ( secureTimer )
		{
			if ( !InMadness )
			{
				getCal = TimerSecureRecover * bonus;
			}
			else
			{
				getCal = TimerRecoverOnMadness * bonus;
			}

			getCal *= 0.25f;
			getCal = timerFight.value + getCal * 0.01f;

			if ( getCal >= 1 && !InMadness )
			{
				getCal = 1;
				InMadness = true;

                

                DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 1f, .9f).OnComplete(() => {
                    //DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 0, .1f);
                });

                //Time.timeScale = .2f;

				float getSpeed = currSpeed;
				float getAcc = acceleration;
				acceleration = 0;

				DOTween.To ( ( ) => 0, x => currSpeed = x, currSpeed, 0.5f ).SetEase ( Ease.InBack ).OnComplete ( ( ) =>
				{
					acceleration = getAcc;
				} );

				StartCoroutine ( camColor ( true ) );
				GlobalManager.Ui.OpenMadness ( );
				Dash = false;
				GlobalManager.Ui.DashSpeedEffect ( false );
			}
		}
		else if ( !lastTimer )
		{
			getCal = ( ( TimerRecover * 0.01f ) * bonus ) * 0.5f + timerFight.value;

			if ( getCal > 0.75f )
			{
                GlobalManager.AudioMa.OpenAudio(AudioType.Madnesse, "Green", true);
				getVol = barMadSource.pitch;
                newStat( StatePlayer.Madness );


                GlobalManager.Ui.MadnessGreenStart();

                secureTimer = true;
			}
		}
		else
		{
			getCal = timerFight.value + ( ( TimerLastRecover * 0.01f ) * bonus ) * 0.25f;

			if ( getCal > 0.25f )
			{
				newStat ( StatePlayer.Normal );

                GlobalManager.Ui.MadnessRedEnd();
                lastTimer = false;
				secureTimer = false;
			}
		}

		timerFight.DOKill ( );
		timerFight.DOValue ( getCal, 0.2f );
	}

	IEnumerator camColor ( bool enable )
	{
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );
		float currTime = 0;
		float inMad;
		float targetTime;
		float getDelT = Time.deltaTime;

		Vector3 getValue;

		if ( enable )
		{
			getValue = saveCamMad;
			//camMad.enabled = true;
			inMad = 0.5f;
			targetTime = 2;
		}
		else
		{
			getValue = -saveCamMad;
			inMad = 1;

			targetTime = 1;
		}

		do
		{
			camMad._Y += getValue.x * getDelT * inMad;
			camMad._U += getValue.y * getDelT * inMad;
			camMad._V += getValue.z * getDelT * inMad;
			currTime += getDelT;

			yield return thisF;

		} while ( currTime < targetTime );

		if ( !enable )
		{
			camMad._Y = 0; camMad._U = 0; camMad._V = 0;
		}
		else
		{
			camMad._Y = getValue.x;
			camMad._U = getValue.y;
			camMad._V = getValue.z;
		}
	}
	#endregion

	#region Private Functions
	void playerAction ( float getTime )
	{
		if ( playerDead || StopPlayer )
		{
			return;
		}

        AllPlayerPrefs.ATimerRun += getTime;
        StaticRewardTarget.LoadReward();

		TimerCheck ( getTime );
		distCal ( );

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

		if ( inputPlayer.GetAxis ( "CoupSimple" ) == 0 )
		{
			resetAxeS = true;
		}

		if ( inputPlayer.GetAxis ( "CoupDouble" ) == 0 )
		{
			resetAxeD = true;
			/*getFOVDP = FOVIncrease;

			if ( timeToDP < TimeToDoublePunch * 0.75f )
			{
				resetAxeD = false;
				dpunch = true;

				playAnimator.SetBool ( "ChargingPunch", true );
				playAnimator.SetBool ( "ChargingPunch_verif", true );
			}
			else
			{
				playAnimator.SetBool ( "ChargingPunch_verif", false );
				playAnimator.SetBool ( "ChargingPunch", false );

				timeToDP = TimeToDoublePunch;
			}*/
		}

		playerFight ( getTime );

		if ( inputPlayer.GetAxis ( "Dash" ) != 0 && !InMadness && !playerDead && canPunch && !chargeDp && canUseDash  )
		{				
			GlobalManager.AudioMa.OpenAudio ( AudioType.Acceleration, "", false, null, true );
			Time.timeScale = 1;
			Dash = true;
			canUseDash = false;
		}
		else if ( inputPlayer.GetAxis ( "Dash" ) == 0 )
		{
			canUseDash = true;
			Dash = false;
		}

		checkInAir ( getTime );
		speAction(getTime);
       
		if ( !inAir )
		{
			if ( !blockChangeLine )
			{
				changeLine ( getTime );
			}
			playerMove ( getTime, currSpeed );
		}
	}

	void newStat ( StatePlayer currStat )
	{
		if ( currStat == StatePlayer.Danger )
		{
            timerFight.GetComponents<RainbowScale>()[0].enabled = true;
            timerFight.GetComponents<RainbowScale>()[1].enabled = false;
            
            backTF.GetComponents<RainbowColor>()[1].enabled = false;
            backTF.DOKill(true);
            backTF.GetComponents<RainbowColor>()[0].enabled = true;

            handleTF.GetComponents<RainbowColor>()[1].enabled = false;
            handleTF.DOKill(true);
            handleTF.GetComponents<RainbowColor>()[0].enabled = true;
        }
		else if ( currStat == StatePlayer.Madness )
		{
            timerFight.GetComponents<RainbowScale>()[0].enabled = false;
            timerFight.GetComponents<RainbowScale>()[1].enabled = true;

            backTF.GetComponents<RainbowColor>()[0].enabled = false;
            backTF.DOKill(true);
            backTF.GetComponents<RainbowColor>()[1].enabled = true;

            handleTF.GetComponents<RainbowColor>()[0].enabled = false;
            handleTF.DOKill(true);
            handleTF.GetComponents<RainbowColor>()[1].enabled = true;
        }
		else // normal
		{
            timerFight.GetComponents<RainbowScale>()[0].enabled = false;
            timerFight.GetComponents<RainbowScale>()[1].enabled = false;
            timerFight.transform.DOKill();
            timerFight.transform.DOScale(1, 0f).SetEase(Ease.InSine);

            backTF.GetComponents<RainbowColor>()[0].enabled = false;
            backTF.GetComponents<RainbowColor>()[1].enabled = false;
            backTF.DOKill();
            backTF.DOColor(Color.white, 0);

            handleTF.GetComponents<RainbowColor>()[0].enabled = false;
            handleTF.GetComponents<RainbowColor>()[1].enabled = false;
            handleTF.DOKill();
            handleTF.DOColor(new Color32(0x4B,0xA0,0xCC,0xFF), 0);
        }
	}

	float getVol = 0;
	void TimerCheck ( float getTime )
	{
		if ( onBonusStart )
		{
			return;
		}

		if ( secureTimer )
		{
			if ( InMadness )
            {
                GlobalManager.AudioMa.CloseAudio(AudioType.Madnesse);

                timerFight.value -= ( getTime / DelayTimerOnMadness ) * 0.25f;
			}
			else
            {
                timerFight.value -= ( getTime / DelayTimerToMad ) * 0.25f;

				barMadSource.pitch = getVol + 1 * ((((timerFight.value - 0.75f) * 100) / 0.25f) / 100);
            }

			if ( timerFight.value < 0.75f )
			{
				GlobalManager.Ui.MadnessGreenEnd();
				secureTimer = false;
				lastTimer = false;

                newStat ( StatePlayer.Normal );

                if ( InMadness )
				{
					timerFight.DOValue ( 0.5f, 0.1f );
					stopMadness ( );
                }
			}
		}
		else if ( !lastTimer )
		{
            GlobalManager.AudioMa.CloseAudio(AudioType.Madnesse);
            timerFight.value -= ( getTime / DelayTimerStandard ) * 0.5f;

			if ( timerFight.value < 0.25f )
			{
                GlobalManager.AudioMa.OpenAudio(AudioType.Madnesse, "Red", true);
				
				getVol = barMadSource.pitch;

                GlobalManager.Ui.MadnessRedStart();

                newStat( StatePlayer.Danger );
                StaticRewardTarget.SRedMadness++;
				secureTimer = false;
				lastTimer = true;

            }
		}
		else if ( !onTuto )
		{
			timerFight.value -= ( getTime / DelayTimerToDeath ) * 0.25f;
			barMadSource.pitch = 2 - (getVol * (((timerFight.value * 100) / 0.25f) / 100)) * 0.5f;
			
			if ( timerFight.value <= 0)
            {
				secureTimer = false;
				lastTimer = false;
                AllPlayerPrefs.ATypeObstacle = "Madness";
                AllPlayerPrefs.ANameObstacle = "Madness a zero";
                RaycastHit hit;
                Physics.Raycast(this.transform.position, Vector3.down, out hit, 20);
                AllPlayerPrefs.ANameChunk = AnalyticsChunk(hit.transform);
				GameOver ( );

				if ( Life > 0 )
				{
					timerFight.value = 0.5f;
					newStat ( StatePlayer.Normal );

					lastTimer = false;
					secureTimer = false;
				}
			}
		}
	}

	int calDist = 0;
    void distCal ( )
	{
        int currDist = 0;

        if ( !inAir )
		{
			totalDis += Vector3.Distance ( lastPos, pTrans.position );
		}

		if ( totalDis - calDist > 1 )
		{
			currDist = ( int ) totalDis - calDist;
			calDist = ( int ) totalDis;
		}

		lastPos = pTrans.position;
		textDist = "" + ( int.Parse ( textDist ) + currDist );
		GlobalManager.Ui.UpdateScore();
		
		if ( totalDis > nextIncrease )
		{
			nextIncrease += DistIncMaxSpeed;

			if ( MaxSpeedInc > ( maxSpeed + SpeedIncrease ) - MaxSpeed )
			{
				maxSpeed += SpeedIncrease;
				acceleration += AcceleraInc;
			}
			else
			{
				maxSpeed = MaxSpeed + MaxSpeedInc;
			}

			if ( MaxCLInc > ( maxSpeedCL + CLSpeedIncrease ) - MaxSpeedCL )
			{
				maxSpeedCL += CLSpeedIncrease;
				accelerationCL += AcceleraCLInc;
				impulsionCL += AcceleraCLInc;
				decelerationCL += AcceleraCLInc;
			}
			else
			{
				maxSpeedCL = MaxSpeedCL + MaxCLInc;
			}
		}
        //StaticRewardTarget.SScoreLV = AllPlayerPrefs.scoreWhithoutDistance + (int)totalDis;
	}

	void speAction ( float getTime )
	{
		if ( inputPlayer.GetAxis ( "SpecialAction" ) == 0 || !canSpe )
		{
			if ( ( ThisAct == SpecialAction.SlowMot || onTuto ) )
			{
				canSpe = true;

				if ( animeSlo )
				{
					AllPlayerPrefs.ANbTechSpe++;
					thisCam.GetComponent<CameraFilterPack_Vision_Aura> ( ).enabled = false;
					animeSlo = false;
					Time.timeScale = 1;
				}
			}
			return;
		}
		Dash = false;
		float getPourc = ( timerFight.maxValue * 0.01f ) * MadnessUse;
		float getCurrPourc = ( timerFight.value / timerFight.maxValue ) * 100 ;
		if ( ThisAct == SpecialAction.SlowMot || onTuto )
        {
			if ( getCurrPourc > MadNeed )
			{
				thisCam.GetComponent<CameraFilterPack_Vision_Aura> ( ).enabled = true;

				if ( !animeSlo )
				{
					animeSlo = true;
					GlobalManager.Ui.StartSpecialAction ( "SlowMot" );
				}
				StaticRewardTarget.STimerSlowMo += getTime;

				Time.timeScale = SlowMotion;
				timerFight.value -= getPourc * Time.deltaTime;
			}
			else
			{
				canSpe = false;
			}
		}
		else if ( ThisAct == SpecialAction.OndeChoc && newH == 0 && getCurrPourc > MadNeed )
		{
			timerFight.value -= getPourc;
            GameObject target = GlobalManager.GameCont.FxInstanciate(GlobalManager.GameCont.Player.transform.position, "Target", transform, 4f);
            target.transform.DOScale(Vector3.one, 0);
			target.transform.localPosition = Vector3.zero;

			RaycastHit[] allHit;
			bool checkGround = true;

			allHit = Physics.RaycastAll ( target.transform.position, Vector3.down, 5 );
			foreach ( RaycastHit thisRay in allHit )
			{
				if ( thisRay.collider.tag == Constants._UnTagg )
				{
					checkGround = false;
					break;
				}
			}

			if ( checkGround )
			{
				Destroy ( target.gameObject );
				return;
			}


			AllPlayerPrefs.ANbTechSpe++;
			canSpe = false;
			playerInv = true;
			thisCam.GetComponent<RainbowMove>().enabled = false;
			pRig.useGravity = false;
			StopPlayer = true;

			GlobalManager.Ui.StartSpecialAction("OndeChoc");

            target.GetComponent<Rigidbody>().AddForce(Vector3.down * 20, ForceMode.VelocityChange);
            
			//MR S S'ABAISSE
			pTrans.DOLocalMoveY(pTrans.localPosition.y - .8f, .35f);
			pTrans.DOLocalRotate((new Vector3(17, 0, 0)), .35f, RotateMode.LocalAxisAdd).SetEase(Ease.InSine).OnComplete(()=> {

				DOVirtual.DelayedCall(.1f, () => {
					onAnimeAir = true;
				});

		

                //MR S SAUTE
				pTrans.DOLocalRotate((new Vector3(-25, 0, 0)), .25f, RotateMode.LocalAxisAdd).SetEase(Ease.InSine);

                pTrans.DOLocalMove(pTrans.localPosition + pTrans.up * 7, .25f).SetEase(Ease.Linear).OnComplete(() => {
					onAnimeAir = false;

                    //MR S RETOMBE
                    pTrans.DOLocalMove(pTrans.localPosition - pTrans.up * 2, .1f).SetEase(Ease.Linear).OnComplete(() => {
						pTrans.DOLocalRotate((new Vector3(35, 0, 0)), .13f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine).OnComplete(() => {
							pTrans.DOLocalRotate((new Vector3(0, 0, 0)), .15f, RotateMode.LocalAxisAdd).SetEase(Ease.InBounce);
                        });

						StopPlayer = false;
                        pRig.useGravity = true;
						pRig.AddForce(Vector3.down * 10, ForceMode.VelocityChange);
                        inAir = true;
						StartCoroutine(groundAfterChoc());
                    });
                });
            });

		}
		else if ( ThisAct == SpecialAction.DeadBall && newH == 0 && getCurrPourc > MadNeed )
		{
			timerFight.value -= getPourc;
            AllPlayerPrefs.ANbTechSpe++;
            pRig.constraints = RigidbodyConstraints.FreezeAll;
			StopPlayer = true;
            GlobalManager.Ui.StartSpecialAction("DeadBall");
			canSpe = false;
			var e = new DeadBallEvent ( );
			e.CheckDist = DistDBTake;
			e.Raise ( );

            StartCoroutine(prepDeadBall());
        }
	}

	IEnumerator groundAfterChoc ( )
	{
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );

		while ( inAir )
		{
			pRig.AddForce(Vector3.down, ForceMode.VelocityChange);
			yield return thisF;
		}
		GameObject circle  = GlobalManager.GameCont.FxInstanciate(Vector3.zero, "CircleGround", pTrans, 10f);
		circle.transform.DOScale(10, 4);
		circle.transform.GetComponent<SpriteRenderer>().DOFade(0, 1.5f);
		circle.transform.localPosition = Vector3.zero;

		thisCam.GetComponent<RainbowMove>().enabled = true;
		ScreenShake.Singleton.ShakeFall();
		sphereChocWave.enabled = true;
		getCouldown = CooldownWave ( );

		StartCoroutine(getCouldown);
		StartCoroutine(waitInvPlayer());

		DOVirtual.DelayedCall ( 0.25f, ( ) =>
		{
			sphereChocWave.enabled = false;
		} );
	}

	IEnumerator waitInvPlayer ( )
	{
		yield return new WaitForSeconds ( 1 );
		playerInv = false;
	}

	IEnumerator prepDeadBall ( )
	{
		yield return new WaitForSeconds ( Constants.DB_Prepare );

		GlobalManager.Ui.BallTransition.enabled = true;

		yield return new WaitForSeconds ( 0.3f );

		GlobalManager.Ui.BallTransition.enabled = false;

		if ( DeadBallPref != null && DeadBallPref.GetComponent<Rigidbody> ( ) != null )
		{
			GameObject currObj = ( GameObject ) Instantiate ( DeadBallPref );
			currObj.transform.position = pTrans.position + pTrans.forward * 12;

			var e = new DeadBallParent ( );
			e.NewParent = currObj.transform;
			e.Raise ( );
		}
		CurrNbrBall = 0;
		pRig.constraints = thisConst;
		StopPlayer = false;
        StaticRewardTarget.SSizeMagicSphere = 0;
		//getCouldown = CooldownDeadBall ( );
		//sStartCoroutine( getCouldown );

		canSpe = true;
	}

	void waitInvDmg ( )
	{
		invDamage = false;
	}


	void checkInAir ( float getTime )
	{
		RaycastHit[] allHit;
		bool checkAir = true;

		allHit = Physics.RaycastAll ( pTrans.position, Vector3.down, 2 );

		foreach ( RaycastHit thisRay in allHit )
		{
			if ( thisRay.collider.gameObject == gameObject || thisRay.collider.tag == Constants._EnnemisTag || thisRay.collider.tag == Constants._ObsPropSafe )
			{
				continue;
			}

			if ( thisRay.collider.gameObject.layer == 9 )
			{
				checkAir = false;
				Transform getThis = thisRay.collider.transform;

				pTrans.DORotate ( new Vector3 ( getThis.rotation.x, pTrans.rotation.eulerAngles.y, pTrans.rotation.eulerAngles.z ), 0 );
				pivotTrans.localRotation = Quaternion.Inverse ( Quaternion.Euler ( new Vector3 (  getThis.rotation.x, 0, 0 ) ) );

				pTrans.localPosition = new Vector3 ( pTrans.localPosition.x, thisRay.point.y + 1.6f, pTrans.localPosition.z );
				break;
			}
			else if (  thisRay.collider.tag == Constants._UnTagg && thisRay.collider.gameObject.layer == 0 )
			{
				checkAir = false;
				pTrans.localPosition = new Vector3 ( pTrans.localPosition.x, thisRay.point.y + 1.6f, pTrans.localPosition.z );
				pTrans.DOLocalRotate ( new Vector3 ( 0, pTrans.localRotation.eulerAngles.y, pTrans.localRotation.eulerAngles.z ), 0 );
				pivotTrans.localRotation = Quaternion.identity;

				pRig.constraints = RigidbodyConstraints.FreezeAll;
			}
		}

		if ( checkAir )
		{
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

			if ( inAir )
			{
				pRig.constraints = thisConst;
				pRig.useGravity = true;

				if ( thisEnum != null )
				{
					StopCoroutine ( thisEnum );
				}

				thisEnum = waitConstraint ( );
				StartCoroutine ( thisEnum );
				if ( pTrans.position.y < checkDistY )
				{
					GameOver ( true );
				}

				pRig.AddForce ( Vector3.down * BonusGrav * getTime, ForceMode.VelocityChange );
			}
        }
		else if ( !checkAir && getCamRM || inAir )
        {
			checkDistY = pTrans.position.y - 1000;

			if ( currWF != null )
			{
				StopCoroutine ( currWF );
				currWF = null;
			}

			getCamRM = false;

			if ( inAir )
			{
				inAir = false;

				thisCam.GetComponent<RainbowMove> ( ).enabled = true;
				thisCam.GetComponent<RainbowRotate> ( ).enabled = true;
			}
        }
	}

	IEnumerator waitConstraint ()
	{
		yield return new WaitForSeconds ( 2 );

		thisEnum = null;
		pRig.constraints = RigidbodyConstraints.FreezeAll;
	}

	IEnumerator waitFall ( )
	{
		yield return new WaitForSeconds ( TimeToFall );

		inAir = true;
		currWF = null;
		thisCam.GetComponent<RainbowMove> ( ).reStart ( );
		thisCam.GetComponent<RainbowRotate> ( ).reStart ( );
		thisCam.transform.localRotation = startRotRR;
		thisCam.transform.localPosition = startPosRM; 
		thisCam.GetComponent<RainbowMove>().enabled = false;
		thisCam.GetComponent<RainbowRotate>().enabled = false;
	}

	void playerMove ( float delTime, float speed )
	{
		Vector3 calTrans = Vector3.zero;
		delTime = Time.deltaTime;

		if ( Dash )
		{
			speed *= DashSpeed;
			AllPlayerPrefs.ATimeDash += delTime;
			thisCam.GetComponent<CameraFilterPack_Blur_BlurHole> ( ).enabled = true;
		}
		else if ( InMadness )
		{
            StaticRewardTarget.STimerMadness += delTime;
			speed *= MadnessSpeed;
		}
		else if ( chargeDp )
		{
			GlobalManager.Ui.DashSpeedEffect ( false );
			speed /= 1.60f;
		}
		else if ( onBonusStart )
		{
			speed *= DashSpeed * 2;
			thisCam.GetComponent<CameraFilterPack_Blur_BlurHole> ( ).enabled = true;
		}
		else
		{
			GlobalManager.Ui.DashSpeedEffect ( false );
			thisCam.GetComponent<CameraFilterPack_Blur_BlurHole>().enabled = false;
		}

		float calCFov = Constants.DefFov * ( speed / maxSpeed );

		if ( timeToDP == TimeToDoublePunch )
		{
			if ( !inAir )
			{
				Shader.SetGlobalFloat ( "_ReduceVis", speed / maxSpeed );

				if ( thisCam.fieldOfView < calCFov )
				{
					thisCam.fieldOfView += delTime * SpeedEffectTime;
					if ( thisCam.fieldOfView > calCFov )
					{
						thisCam.fieldOfView = calCFov;
					}
				}
				else if ( thisCam.fieldOfView > calCFov )
				{
					thisCam.fieldOfView -= delTime * SpeedEffectTime * 2;
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
					thisCam.fieldOfView += delTime * SpeedEffectTime;
					if ( thisCam.fieldOfView > Constants.DefFov )
					{
						thisCam.fieldOfView = Constants.DefFov;
					}
				}
				else if ( thisCam.fieldOfView > Constants.DefFov )
				{
					thisCam.fieldOfView -= delTime * SpeedEffectTime * 2;
					if ( thisCam.fieldOfView < Constants.DefFov )
					{
						thisCam.fieldOfView = Constants.DefFov;
					}
				}
			}
		}

		/*if ( newPos )
		{
			befRot -= speed * delTime;

			if ( befRot < 0 )
			{
				pTrans.transform.position = new Vector3 ( getNewRot.x, pTrans.transform.position.y, getNewRot.z );
				newPos = false;
				useFord = false;
				StartCoroutine ( rotPlayer ( delTime ) );
			}
		}*/
	
		if ( useFord )
		{
			calTrans = pTrans.forward * speed * delTime;
		}
		else
		{
			calTrans = Vector3.zero;
		}

		pTrans.Translate ( calTrans, Space.World );
	}

	void changeLine ( float delTime )
	{
		float newImp = inputPlayer.GetAxis ( "Horizontal" );
		float lineDistance = Constants.LineDist;

		if ( NbrLineRight != 0 || NbrLineLeft != 0 )
		{
			if ( ( canChange || newH == 0 ) && !inAir )
			{
				RaycastHit[] allHit;
				bool checkWall = false;

				allHit = Physics.RaycastAll ( pTrans.position, pTrans.right * newImp, 5 );

				foreach ( RaycastHit thisRay in allHit )
				{
					if ( thisRay.collider.tag == Constants._UnTagg )
					{
						checkWall = true;
						break;
					}
				}

				if ( !checkWall )
				{
					if ( newImp == 1 && LastImp != 1 && currLine + 1 <= NbrLineRight && ( clDir == 1 || newH == 0 ) )
					{
						canChange = false;
						currLine++;
						LastImp = 1;
						clDir = 1;
						newH = newH + lineDistance;
						saveDist = newH;
					}
					else if ( newImp == -1 && LastImp != -1 && currLine - 1 >= -NbrLineLeft && ( clDir == -1 || newH == 0 ) )
					{
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
				else if ( newImp == 0 )
				{
					LastImp = 0;
				}
			}
		}

		if ( newH != 0 )
		{
			if ( Running )
			{
				float accLine = 0;

				if ( saveDist < 0 && newH > -lineDistance * 0.80f || saveDist > 0 && newH < lineDistance * 0.80f )
				{
					canChange = true;
				}

				if ( saveDist < 0 && newH > -lineDistance * 0.20f || saveDist > 0 && newH < lineDistance * 0.20f )
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

			newH -= calTrans;

			if ( saveDist > 0 && newH - calTrans < 0 || saveDist < 0 && newH - calTrans > 0 )
			{
				
				calTrans += newH;
				newH = 0;
				currSpLine = 0;
			}

			dirLine = pTrans.right * calTrans;
			pTrans.Translate ( dirLine, Space.World );
		}
		else
		{
			currSpLine = 0;
		}
	}

	void playerFight ( float getDelta )
	{
		/*if ( inputPlayer.GetAxis ( "CoupDouble" ) != 0 && resetAxeD )
		{
			Dash = false;
			float calcRatio = ( FOVIncrease / TimeToDoublePunch ) * getDelta;

			chargeDp = true;

			if ( timeToDP == TimeToDoublePunch)
			{
				playAnimator.SetBool("ChargingPunch_verif", true);
				playAnimator.SetBool("ChargingPunch", true);
				playAnimator.SetTrigger("Double");
			}

			timeToDP -= getDelta;

			getFOVDP -= calcRatio;

			if ( getFOVDP > 0 )
			{
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

				resetAxeD = false;
				dpunch = true;
			}
		}
		else
		{
			chargeDp = false;
			if ( !dpunch && thisCam.fieldOfView > Constants.DefFov )
			{
				thisCam.fieldOfView -= getDelta * 10;

				if ( thisCam.fieldOfView < Constants.DefFov )
				{
					thisCam.fieldOfView = Constants.DefFov;
				}
			}
		}*/

		if(inputPlayer.GetAxis("CoupSimple") != 0 && canPunch && resetAxeS && GlobalManager.GameCont.introFinished )
        {
            AllPlayerPrefs.ANbCoupSimple++;
			Dash = false;
            thisCam.fieldOfView = Constants.DefFov;

			resetAxeS = false;
            canPunch = false;
			timeToDP = TimeToDoublePunch;

			if (getDelta < 1)
                Time.timeScale = 1;

            int randomSong = UnityEngine.Random.Range(0, 3);
			GlobalManager.AudioMa.OpenAudio(AudioType.Other, "PunchFail_" + (randomSong + 1), false );

            int rdmValue = UnityEngine.Random.Range(0, 10);
			GlobalManager.AudioMa.OpenAudio(AudioType.PunchVoice, "MrStero_Punch_" + rdmValue, false, null, true );

            ScreenShake.Singleton.ShakeHitSimple();
       
            if (punchRight)
            {
                punch.RightPunch = true;

				playAnimator.SetTrigger("Right");
                
            }
            else
            {
                punch.RightPunch = false;

				playAnimator.SetTrigger("Left");
            }
            punchRight = !punchRight;
			punchBoxSimple.enabled = true;
			startPunch ( 0 );
		}
		else if( inputPlayer.GetAxis("CoupDouble") != 0 && canPunch && dpunch && resetAxeD )
        {
			playAnimator.SetBool("DoublePunchEnd", false);

            AllPlayerPrefs.ANbCoupDouble++;
			Dash = false;
			thisCam.fieldOfView = Constants.DefFov;
			resetAxeD = false;
			playAnimator.SetBool("ChargingPunch_verif", false);
			playAnimator.SetBool("ChargingPunch", true);
			playAnimator.SetTrigger("Double");
			dpunch = false;

			DOVirtual.DelayedCall ( DelayDoublePunch, ( ) =>
			{
				ScreenShake.Singleton.ShakeHitDouble();
				punchBoxSimple.enabled = true;
				GlobalManager.Ui.DoubleCoup();
				playAnimator.SetBool("ChargingPunch_verif", true);
				playAnimator.SetBool("ChargingPunch", false);

				dpunch = true;
				startPunch ( 1 );

				DOVirtual.DelayedCall(0.1f, ()  =>
				{
					playAnimator.SetBool("ChargingPunch_verif", false);
					playAnimator.SetBool("ChargingPunch", false);
					/*dpunch = true;
					startPunch ( 1 );
					playAnimator.SetBool("ChargingPunch_verif", false);*/
				});
			} );

			canPunch = false;

			/*if (getDelta < 1)
                Time.timeScale = 1;
			
			timeToDP = TimeToDoublePunch;*/
        }
	}

	private IEnumerator CooldownPunch ( int type_technic )
    {

		if ( type_technic == 0 )
		{
			yield return new WaitForEndOfFrame ( );
		}
		else
		{
			yield return new WaitForSeconds ( CooldownDoublePunch );

            playAnimator.SetBool("DoublePunchEnd", true);

		}
	
		canPunch = true;
    }

	private IEnumerator TimerHitbox( int tech )
	{
		if ( tech == 0 )
		{
			yield return new WaitForSeconds ( 0.1f );
		}
		else
		{
			yield return new WaitForSeconds ( 0.3f );
		}
		punchBoxSimple.enabled = false;
	}

    IEnumerator CooldownWave()
    {
		float countTime = 0;
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );

		do
		{
			SliderSlow.value = countTime;

			yield return thisF;

			countTime += Time.deltaTime;
		} while ( countTime < delayChocWave );

		SliderSlow.value = delayChocWave;

		canSpe = true;
    }

	/*IEnumerator CooldownDeadBall()
	{
		float countTime = 0;
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );

		do
		{
			SliderSlow.value = countTime;

			yield return thisF;

			countTime += Time.deltaTime;
		} while ( countTime < DelayDeadBall );

		SliderSlow.value = DelayDeadBall;

		canSpe = true;
	}*/

	public void NewRotation ( GameObject thisColl, bool goRight )
	{
		//Debug.Log ( "new rotation : " + goRight );
		Vector3 getThisC = thisColl.transform.position;
			
		StopPlayer = true;
		newDir = goRight;

		getThisC = new Vector3 ( getThisC.x, pTrans.position.y, getThisC.z );
		pTrans.DOKill ( );
		pTrans.DOMove ( new Vector3 ( getThisC.x, pTrans.position.y, getThisC.z ), RotationSpeed );

		if ( newDir )
		{
			getThisC = new Vector3 ( 0, 90, 0 );
		}
		else
		{
			getThisC = new Vector3 ( 0, -90, 0 );
		}

		pTrans.DOLocalRotate ( getThisC, RotationSpeed, RotateMode.LocalAxisAdd ).OnComplete( ()=>
		{
			useFord = true;
			StopPlayer = false;
			blockChangeLine = false;
		});
	}

	void OnCollisionEnter ( Collision thisColl )
	{
        if (playerDead)
		{
            return;
		}

		GameObject getObj = thisColl.gameObject;
		if ( onAnimeAir && thisColl.collider.tag == Constants._UnTagg )
		{
            AllPlayerPrefs.ATypeObstacle = "Mur / Plafond / Sol";
            AllPlayerPrefs.ANameObstacle = thisColl.gameObject.name;
            AllPlayerPrefs.ANameChunk = AnalyticsChunk(getObj.transform);
            GameOver ( true );
		}

		if ( Dash || InMadness || playerInv || onBonusStart )
		{
            if (getObj.tag == Constants._EnnemisTag)
            {
                GlobalManager.Ui.BloodHitDash();
            }
            if ( getObj.tag == Constants._EnnemisTag || getObj.tag == Constants._ElemDash )
			{
				int rdmValue = UnityEngine.Random.Range(0, 3);

                GlobalManager.Ui.BloodHitDash();
                GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "Glass_" + rdmValue, false,null,false);
				if ( !onTuto )
				{
					thisColl.collider.enabled = false;
				}

				AbstractObject getAbstra = thisColl.gameObject.GetComponentInChildren<AbstractObject> ( );
				if ( getAbstra )
				{
					if ( Dash )
					{
						getAbstra.ForceProp ( getPunch.projection_dash * pTrans.forward, DeathType.Acceleration );
					}
					else if ( InMadness )
					{
						getAbstra.ForceProp ( getPunch.projection_dash * pTrans.forward, DeathType.Madness );
					}
					else
					{
						getAbstra.ForceProp ( getPunch.projection_dash * pTrans.forward, DeathType.Punch );
					}
				}
				return;
			}
			else if ( getObj.tag == Constants._Balls )
			{
				StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( getObj, pTrans, PropulseBalls, 1, false, true ) );
				return;
			}
		}
		else if ( getObj.tag == Constants._ElemDash )
		{
            AllPlayerPrefs.ATypeObstacle = Constants._ElemDash;
            AllPlayerPrefs.ANameObstacle = thisColl.gameObject.name;
            AllPlayerPrefs.ANameChunk = AnalyticsChunk(getObj.transform);
            GameOver ( );
		}

		if ( getObj.tag == Constants._MissileBazoo )
		{
            AllPlayerPrefs.ATypeObstacle = Constants._MissileBazoo;
            AllPlayerPrefs.ANameObstacle = thisColl.gameObject.name;
            AllPlayerPrefs.ANameChunk = AnalyticsChunk(getObj.transform);
            getObj.GetComponent<MissileBazooka> ( ).Explosion ( );
			GameOver ( );
		}
		else if ( getObj.tag == Constants._EnnemisTag || getObj.tag == Constants._Balls )
		{
            AllPlayerPrefs.ATypeObstacle = getObj.tag;
            AllPlayerPrefs.ANameObstacle = thisColl.gameObject.name;
            AllPlayerPrefs.ANameChunk = AnalyticsChunk(getObj.transform);
            GameOver ( );
		}
		else if ( getObj.tag == Constants._ObsTag )
		{
            AllPlayerPrefs.ATypeObstacle = Constants._ObsTag;
            AllPlayerPrefs.ANameObstacle = thisColl.gameObject.name;
            AllPlayerPrefs.ANameChunk = AnalyticsChunk(getObj.transform);

			if ( Life > 1 )
			{
				StopPlayer = true;
				currSpeed = 0;

				RaycastHit[] allHit;
				float getDist = 12;

				allHit = Physics.RaycastAll ( pTrans.position, -pTrans.forward, 12 );
				foreach ( RaycastHit thisRay in allHit )
				{
					if ( thisRay.collider.tag == Constants._UnTagg || thisRay.collider.tag == Constants._ObsTag )
					{
						if ( getDist > thisRay.distance )
						{
							getDist = thisRay.distance;
						}

						break;
					}
					else if ( thisRay.collider.tag == Constants._EnnemisTag || thisRay.collider.tag == Constants._ElemDash )
					{
						Destroy ( thisRay.collider.gameObject );
					}
				}

				getDist -= 2;
				
				GlobalManager.GameCont.PlayerCollider = true;
				pTrans.localPosition -= pTrans.forward;

        		thisCam.transform.DOShakePosition(0.5f, 5, 2, 5).OnComplete ( () => 
				{
					pTrans.DOLocalMove ( pTrans.localPosition - pTrans.forward * getDist, 0.5f ).OnComplete ( ( ) =>
					{
						StopPlayer = false;
					} );
				});
			}

			GameOver ( );
		}
	}

	void stopMadness ( )
	{
		DOVirtual.DelayedCall ( 0.25f, ( ) =>
		{
			InMadness = false;
		} );

		StartCoroutine ( camColor ( false ) );

		GlobalManager.Ui.CloseMadness();
	}
    private string AnalyticsChunk(Transform p_child)
    {
		if ( onTuto || p_child == null)
		{
			return "Tutorial";
		}

        Transform currentTrans = p_child;
        if(currentTrans == null)
        {
            return "Chunk non identifier";
        }
		while(currentTrans.parent != null && currentTrans.parent.name != "Chuncks" )
        {
            currentTrans = currentTrans.parent;
        }
        if (currentTrans.parent == null)
        {
            return "Chunk non identifier";
        }
        string nameChunk = currentTrans.name.Split('(')[0];
        return nameChunk;
    }

	#endregion
}
