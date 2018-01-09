using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Rewired;

public class PlayerController : MonoBehaviour 
{
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
	[Tooltip ("Pourcentage de ralentissement du personnage dans les airs")]
	public float PourcRal = 50;
	public float delayChocWave = 5;
	public float DelayDeadBall = 15;
	public float DistDBTake = 25;
	public GameObject DeadBallPref;

	[Header ("TimerCaract")]
	public float DelayTimerFight = 5;
	[Tooltip ("Delay de survie une fois le timer à 0")]
	public float DelayLastTimer = 2.5f;
	public float DelaySecureTimer = 2.5f;
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
    public int debugNumTechnic = 2;
    public float addPointBarByPunchSimple = 3;
    public float addPointBarByPunchDouble = 5;

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

    //public float delayInBeginMadness = 2;
   // public float delayInEndMadness = 2;

    [Header ("SphereMask")]
	public float Radius;
	public float SoftNess;

	[HideInInspector]
	public Slider BarMadness;
	public SpecialAction ThisAct;
	[HideInInspector]
	public int NbrLineRight = 1;
	[HideInInspector]
	public int NbrLineLeft = 1;
	[HideInInspector]
	public bool Dash = false;
	[HideInInspector]
	public bool blockChangeLine = false;
	[HideInInspector]
	public bool InMadness = false;
	[HideInInspector]
	public Slider SliderSlow;
    [HideInInspector]
    public bool playerInv = false;
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
    //public GameObject Plafond;

	[HideInInspector]
	public int currLine = 0;

	[HideInInspector]
	public int MultiPli = 1;

    Transform pTrans;
	Rigidbody pRig;
	RigidbodyConstraints thisConst;

	Direction currentDir = Direction.North;
	Direction newDir = Direction.North;
	Vector3 dirLine = Vector3.zero;
	Vector3 lastPos;
	Vector3 currVect;

	//Vector3 posDir;
	Text textDist;
	//Text textCoin;

	IEnumerator currWF;
	IEnumerator propPunch;
	IEnumerator thisEnum;
	IEnumerator getCouldown;

	Animator playAnimator;

	Camera thisCam;
	Transform pivotTrans;
	Punch getPunch;
    CameraFilterPack_Color_YUV camMad;
    Vector3 saveCamMad;

	Quaternion startRotRR;
	Quaternion startRotPlayer;
	Vector3 startPosRM;
	Vector3 startPlayer;
    Player inputPlayer;
	Slider timerFight;
	Image backTF;
    Image handleTF;

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
	float newDist;
	float saveDist;
	float nextIncrease = 0;
	float befRot = 0;
	float SliderContent;
	public float totalDis = 0;
    float rationUse = 1;
	//float calPos = 0;

	float valueSmooth = 0;
    float valueSmoothUse = 0;
	float timeToDP;
    //float timerBeginMadness = 0;
	float getFOVDP;

	int LastImp = 0;
	int clDir = 0;
    int debugTech = 0;

	//bool canJump = true;
	bool Running = true;
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
    [HideInInspector]
    public bool playerDead = false;
	bool dpunch = false;
    //bool InBeginMadness = false;
	bool chargeDp = false;
	bool canUseDash = true;
    bool onAnimeAir = false;
	bool lastTimer = false;
	bool secureTimer = false;
	bool useFord = true;
	bool getCamRM = false;
    #endregion

    #region Mono
    void Update ( )
	{
        if (Input.GetKeyDown(KeyCode.K))
        {
            MaxSpeed = 1f;
            acceleration = 1;
        }
            
        if (Input.GetKeyDown(KeyCode.R))
            GlobalManager.GameCont.Restart();

        float getTime = Time.deltaTime;

		rationUse = 1;

		if ( InMadness )
		{
			rationUse += timerFight.value;
		}

		punch.SetPunch ( !playerDead );

		playerAction ( getTime );

		Mathf.Clamp ( Radius, 0, 100 );
		Mathf.Clamp ( SoftNess, 0, 100 );

		Shader.SetGlobalVector ( "GlobaleMask_Position", new Vector4 ( pTrans.position.x, pTrans.position.y, pTrans.position.z, 0 ) );
		Shader.SetGlobalFloat ( "GlobaleMask_Radius", Radius );
		Shader.SetGlobalFloat ( "GlobaleMask_SoftNess", SoftNess );
		Shader.SetGlobalFloat ( "_SlowMot", Time.timeScale );
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            debugTech++;
            if (debugTech == debugNumTechnic)
                debugTech = 0;
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

	public void IniPlayer ( )
	{
		pTrans = transform;

		timerFight = GlobalManager.Ui.Madness;
		timerFight.value = 0.5f;
		backTF = timerFight.transform.GetChild(1).transform.GetChild(0).GetComponent<Image> ( );
        handleTF = timerFight.transform.GetChild(2).transform.GetChild(0).GetComponent<Image> ( );
		pRig = gameObject.GetComponent<Rigidbody> ( );
		thisConst =	pRig.constraints;
		punchBox = pTrans.GetChild(0).GetComponent<BoxCollider>();
		sphereChocWave = pTrans.Find("ChocWave").GetComponent<SphereCollider>();
		punch = pTrans.GetChild(0).GetComponent<Punch>();
		canPunch = true; 
		punchRight = true;

		getPunch = GetComponentInChildren<Punch> ( );
		SliderSlow = GlobalManager.Ui.MotionSlider;
		SliderContent = 10;
		lastPos = pTrans.position;
		textDist = GlobalManager.Ui.ScorePoints;
		//textCoin = GlobalManager.Ui.MoneyPoints;
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

		inputPlayer = ReInput.players.GetPlayer(0);

		GameObject getObj = ( GameObject ) Instantiate ( new GameObject ( ), pTrans );
		getObj.transform.localPosition = Vector3.zero;
		getObj.name = "pivot";
		getObj.transform.localPosition = Vector3.zero;
		thisCam = GlobalManager.GameCont.thisCam;
		thisCam.transform.SetParent ( getObj.transform );
		pivotTrans = getObj.transform;

		startRotRR = thisCam.transform.localRotation;
		startPosRM = thisCam.transform.localPosition;
		startPlayer = pTrans.localPosition;
		startRotPlayer = pTrans.localRotation;
		//Plafond.GetComponent<MeshRenderer>().enabled = true;
	}

	public void ResetPlayer ( )
	{
		if ( getCouldown != null )
		{
			StopCoroutine ( getCouldown );
		}

		textDist.text = "0";
		SliderSlow.value = SliderSlow.maxValue;

		newStat ( StatePlayer.Normal );
        currentDir = Direction.North;
		timerFight.DOValue ( 0.5f, Mathf.Abs ( timerFight.value - 0.5f ) );
		backTF.color = Color.white;
		lastTimer = false;
		secureTimer = false;
		newPos = false;
		blockChangeLine = false;
		canPunch = true; 
		punchRight = true;
		getFOVDP = FOVIncrease;
		Life = 1;
		playerDead = false;
		StopPlayer = true;
		totalDis = 0;
		nextIncrease = DistIncMaxSpeed;
		maxSpeed = MaxSpeed;
		maxSpeedCL = MaxSpeedCL;
		accelerationCL = AccelerationCL;
		acceleration = Acceleration;
		impulsionCL = ImpulsionCL;
		decelerationCL = DecelerationCL;
		ThisAct = SpecialAction.Nothing;
		timeToDP = TimeToDoublePunch;
		//BarMadness = GlobalManager.Ui.Madness;
		//BarMadness.value = 0;
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
		stopMadness ( );

		currVect = Vector3.forward;
	}

    public void GameOver ( bool forceDead = false )
	{
        if ( invDamage  && !forceDead )
		{
			return;
		}

        Life--;

        GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(1, 1);

        if ( Life > 0 || playerDead )
		{
			invDamage = true;
			Invoke ( "waitInvDmg", TimeInvincible );

            if (!playerDead)
            {
				GlobalManager.Ui.StartBonusLife ( Life + 1 );
            }

            return;
		}

        AllPlayerPrefs.distance = totalDis;
		AllPlayerPrefs.finalScore = AllPlayerPrefs.scoreWhithoutDistance + (int) totalDis;

		StopPlayer = true;

		thisCam.GetComponent<RainbowMove>().enabled = false;
		thisCam.GetComponent<RainbowRotate>().enabled = false;

		GameOverTok thisTok = new GameOverTok ( );
		thisTok.totalDist = totalDis;

		GlobalManager.Ui.GameOver();

        DOVirtual.DelayedCall(.2f, () =>
        {
            thisCam.transform.DORotate(new Vector3(-220, 0, 0), 1.8f, RotateMode.LocalAxisAdd);
            thisCam.transform.DOLocalMoveZ(-50f, .4f);
        });

        playerDead = true;
		thisCam.fieldOfView = Constants.DefFov;

        GlobalManager.GameCont.soundFootSteps.Kill();

        DOVirtual.DelayedCall(1f, () =>
        {
            GlobalManager.Ui.OpenThisMenu(MenuType.GameOver, thisTok);
        });
    }

    public void MadnessMana(int type)
    {
        if (/*barMadness.value + addPointBarByPunchSimple < barMadness.maxValue &&*/ type == 0)
        {
            this.AddSmoothCurve(addPointBarByPunchSimple);
        }
        else if (/*barMadness.value + addPointBarByPunchDouble < barMadness.maxValue &&*/ type == 1)
        {
            this.AddSmoothCurve(addPointBarByPunchDouble);
        }
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

    public void GetPunchIntro()
    {
		if (StopPlayer && /*Input.GetAxis("CoupSimple") != 0 && */canPunch /* && resetAxeS*/)
        {
            resetAxeS = false;
            canPunch = false;
            propP = true;
            timeToDP = TimeToDoublePunch;
            if (Time.timeScale < 1)
                Time.timeScale = 1;

            ScreenShake.Singleton.ShakeIntro();
            
			GlobalManager.AudioMa.OpenAudio(AudioType.Other, "PunchSuccess", false );

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
            StartCoroutine(StartPunch(0));
            propPunch = propulsePunch(TimePropulsePunch);
            StartCoroutine(propPunch);
        }
    }

	public void RecoverTimer ( DeathType thisDeath, int nbrPoint, float malus )
	{
		if ( playerDead || StopPlayer )
		{
			return;
		}

		GlobalManager.GameCont.NewScore ( thisDeath, nbrPoint );

		if ( malus <= 0 )
		{
			malus = 1;
		}

		if ( Dash )
		{
			malus /= 2;
		}

		float getCal;
		if ( secureTimer )
		{
			if ( !InMadness )
			{
				getCal = TimerSecureRecover / malus;
			}
			else
			{
				getCal = TimerRecoverOnMadness / malus;
			}

			getCal *= 0.25f;
			getCal = timerFight.value + getCal * 0.01f;

			if ( getCal >= 1 && !InMadness )
			{
				getCal = 1;
				InMadness = true;
				StopPlayer = true;

				DOVirtual.DelayedCall(2f, () => {
					StopPlayer = false;
				});

				StartCoroutine ( camColor ( true ) );
				GlobalManager.Ui.OpenMadness ( );
			}
		}
		else if ( !lastTimer )
		{
			getCal = ( ( TimerRecover * 0.01f ) / malus ) * 0.5f + timerFight.value;

			if ( getCal > 0.75f )
			{
				newStat ( StatePlayer.Madness );

				secureTimer = true;
			}
		}
		else
		{
			getCal = timerFight.value + ( ( TimerLastRecover * 0.01f ) / malus ) * 0.25f;

			if ( getCal > 0.25f )
			{
				newStat ( StatePlayer.Normal );
                
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
			camMad.enabled = true;
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
			camMad.enabled = false;
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
			getFOVDP = FOVIncrease;

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
			}
		}

		playerFight ( getTime );

		if ( inputPlayer.GetAxis ( "Dash" ) != 0 && !InMadness && !playerDead && canPunch && !chargeDp && canUseDash )
		{				
			int rdmValue = UnityEngine.Random.Range(0, 3);
			GlobalManager.AudioMa.OpenAudio ( AudioType.Acceleration, "MrStero_Acceleration_" + rdmValue, false, null, true );
			Time.timeScale = 1;
			Dash = true;
			canUseDash = false;
			GlobalManager.Ui.DashSpeedEffect ( true );
		}
		else if ( inputPlayer.GetAxis ( "Dash" ) == 0 )
		{
			canUseDash = true;
			Dash = false;
		}

		checkInAir ( getTime );
		changeLine ( getTime );

        switch ( debugTech )
        {
        case 0:
            speAction(getTime);
        break;
        case 1:
			if (inputPlayer.GetAxis("SpecialAction") > 0) {
                sphereChocWave.enabled = true;
                StartCoroutine(CooldownWave());
                StartCoroutine(TimerHitbox());
            }
        break;
        }
		
		playerMove ( getTime, currSpeed );
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

	void TimerCheck ( float getTime )
	{
		if ( secureTimer )
		{
			if ( InMadness )
			{
				timerFight.value -= ( getTime / DelayTimerOnMadness ) * 0.25f;
			}
			else
			{
				timerFight.value -= ( getTime / DelaySecureTimer ) * 0.25f;
			}

			if ( timerFight.value < 0.75f )
			{
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
			timerFight.value -= ( getTime / DelayTimerFight ) * 0.5f;

			if ( timerFight.value < 0.25f )
			{
				newStat ( StatePlayer.Danger );

				secureTimer = false;
				lastTimer = true;

            }
		}
		else
		{
			timerFight.value -= ( getTime / DelayLastTimer ) * 0.25f;

			if ( timerFight.value <= 0 )
			{
				secureTimer = false;
				lastTimer = false;
				GameOver ( true );
			}
		}
	}

	void distCal ( )
	{
		if ( !inAir )
		{
			totalDis += Vector3.Distance ( lastPos, pTrans.position );
		}

		lastPos = pTrans.position;
		textDist.text = "" + Mathf.RoundToInt ( totalDis );
		//Debug.Log ( maxSpeed );
		if ( totalDis > nextIncrease )
		{
			nextIncrease += DistIncMaxSpeed;

			if ( MaxSpeedInc > ( maxSpeed + SpeedIncrease ) - MaxSpeed )
			{
				maxSpeed += SpeedIncrease;
				acceleration += AcceleraInc;
                //Debug.Log(maxSpeed);
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
	}

	void speAction ( float getTime )
	{
		if ( inputPlayer.GetAxis ( "SpecialAction" ) == 0 || !canSpe )
		{
			SliderSlow.value += getTime;

			if ( ThisAct == SpecialAction.SlowMot && animeSlo )
			{
				thisCam.GetComponent<CameraFilterPack_Vision_Aura> ( ).enabled = false;
				animeSlo = false;
				Time.timeScale = 1;
			}
			return;
		}
		Dash = false;

		if (ThisAct == SpecialAction.SlowMot )
        {
            if (SliderContent > 0)
            {
                thisCam.GetComponent<CameraFilterPack_Vision_Aura>().enabled = true;

                if (!animeSlo)
                {
                    animeSlo = true;
                    GlobalManager.Ui.StartSpecialAction("SlowMot");
                }

                if (Time.timeScale > 1 / SlowMotion)
                {
                    Time.timeScale -= Time.deltaTime * SpeedSlowMot;
                }

                SliderContent -= ReduceSlider * Time.deltaTime;
            }
            else if (Time.timeScale < 1)
            {
                if (SliderContent < 0)
                {
                    canSpe = false;
                    SliderContent = 0;
                }

				Time.timeScale = 1;
            }
            else if (SliderContent < 10)
            {
                animeSlo = false;
                Time.timeScale = 1;
                SliderContent += RecovSlider * getTime;
                thisCam.GetComponent<CameraFilterPack_Vision_Aura>().enabled = false;

                if (SliderContent > 2)
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
		else if ( ThisAct == SpecialAction.OndeChoc && newH == 0 )
		{
			canSpe = false;
			playerInv = true;
			thisCam.GetComponent<RainbowMove>().enabled = false;
			pRig.useGravity = false;
			StopPlayer = true;

            GlobalManager.Ui.StartSpecialAction("OndeChoc");

            //MR S S'ABAISSE
            GameObject target = GlobalManager.GameCont.FxInstanciate(GlobalManager.GameCont.Player.transform.position, "Target", transform.parent, 4f);
            target.transform.DOScale(Vector3.one, 0);
            pTrans.DOLocalMoveY(pTrans.localPosition.y - .8f, .35f);
            target.transform.position = pTrans.forward * 14 + pTrans.position + Vector3.up * 3;
            target.GetComponent<Rigidbody>().AddForce(Vector3.down * 20, ForceMode.VelocityChange);
            pTrans.DOLocalRotate((new Vector3(17, 0, 0)), .35f, RotateMode.LocalAxisAdd).SetEase(Ease.InSine).OnComplete(()=> {

				DOVirtual.DelayedCall(.1f, () => {
					onAnimeAir = true;
				});
                //MR S SAUTE
				pTrans.DOLocalRotate((new Vector3(-25, 0, 0)), .25f, RotateMode.LocalAxisAdd).SetEase(Ease.InSine);
                //target.transform.DOLocalMove(pTrans.localPosition + pTrans.forward * 5 + pTrans.up * 7, 0).SetEase(Ease.Linear);
                //target.transform.DOLocalMove(pTrans.localPosition + pTrans.forward * 3 - pTrans.up * 2, 0f).SetEase(Ease.Linear);
                pTrans.DOLocalMove(pTrans.localPosition + pTrans.forward * 5 + pTrans.up * 7, .25f).SetEase(Ease.Linear).OnComplete(() => {
					onAnimeAir = false;

                    //MR S RETOMBE
                    pTrans.DOLocalMove(pTrans.localPosition + pTrans.forward * 3 - pTrans.up * 2, .1f).SetEase(Ease.Linear).OnComplete(() => {
						pTrans.DOLocalRotate((new Vector3(35, 0, 0)), .13f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine).OnComplete(() => {
							pTrans.DOLocalRotate((new Vector3(0, 0, 0)), .15f, RotateMode.LocalAxisAdd).SetEase(Ease.InBounce);

                            GameObject circle  = GlobalManager.GameCont.FxInstanciate(GlobalManager.GameCont.Player.transform.position, "CircleGround", transform, 10f);
                            circle.transform.DOScale(10, 4);
                            circle.transform.GetComponent<SpriteRenderer>().DOFade(0, 1.5f);

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
		else if ( ThisAct == SpecialAction.DeadBall && newH == 0 )
		{
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

		thisCam.GetComponent<RainbowMove>().enabled = true;
		ScreenShake.Singleton.ShakeFall();
		sphereChocWave.enabled = true;
		getCouldown = CooldownWave ( );

		StartCoroutine(getCouldown);
		StartCoroutine(TimerHitbox());
		StartCoroutine(waitInvPlayer());
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
			currObj.transform.position = pTrans.position + pTrans.forward * 8;

			var e = new DeadBallParent ( );
			e.NewParent = currObj.transform;
			e.Raise ( );
		}

		pRig.constraints = thisConst;
		StopPlayer = false;
		getCouldown = CooldownDeadBall ( );
		StartCoroutine( getCouldown );
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

		if ( Dash )
		{
			getTime *= DashSpeed * 1.1f;
		}

		getTime *= ( maxSpeed / MaxSpeed );

		foreach ( RaycastHit thisRay in allHit )
		{
			if ( thisRay.collider.gameObject == gameObject || thisRay.collider.tag == Constants._EnnemisTag || thisRay.collider.tag == Constants._ObsPropSafe )
			{
				continue;
			}

			checkAir = false;

			if ( thisRay.collider.gameObject.layer == 9 )
			{
				Transform getThis = thisRay.collider.transform;

				if ( !waitRotate )
				{
					pTrans.DORotate ( new Vector3 ( getThis.rotation.x, pTrans.rotation.eulerAngles.y, pTrans.rotation.eulerAngles.z ), 0 );
					pivotTrans.localRotation = Quaternion.Inverse ( Quaternion.Euler ( new Vector3 ( pTrans.localRotation.x, 0, 0 ) ) );
				}

				pTrans.localPosition = new Vector3 ( pTrans.localPosition.x, thisRay.point.y + 1.5f, pTrans.localPosition.z );
			}
			else if (  thisRay.collider.tag == Constants._UnTagg && thisRay.collider.gameObject.layer == 0 )
			{
				pTrans.localPosition = new Vector3 ( pTrans.localPosition.x, thisRay.point.y + 1.5f, pTrans.localPosition.z );
				if ( !waitRotate )
				{
					pTrans.DOLocalRotate ( new Vector3 ( 0, pTrans.localRotation.eulerAngles.y, pTrans.localRotation.eulerAngles.z ), 0 );
					pivotTrans.localRotation = Quaternion.identity;
				}

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
		Transform transPlayer = pTrans;
		Vector3 calTrans = Vector3.zero;
		delTime = Time.deltaTime;

		if ( inAir )
		{
			speed = ( speed / 100 ) * PourcRal;
		}

		if ( InMadness )
		{
			speed *= 1.5f;
			thisCam.GetComponent<CameraFilterPack_Blur_BlurHole> ( ).enabled = true;
		}
		if ( Dash && !playerDead && !InMadness )
		{
			speed *= DashSpeed;

			thisCam.GetComponent<CameraFilterPack_Blur_BlurHole> ( ).enabled = true;
		}
		else if ( chargeDp )
		{
			GlobalManager.Ui.DashSpeedEffect ( false );
			speed /= 1.60f;
		}
		else
		{
			GlobalManager.Ui.DashSpeedEffect ( false );
			thisCam.GetComponent<CameraFilterPack_Blur_BlurHole>().enabled = false;

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

		if ( newPos )
		{
			befRot -= speed * delTime;

			if ( befRot < 0 )
			{
				newPos = false;
				currentDir = newDir;
				pTrans.Translate ( pTrans.forward * befRot, Space.World );
				useFord = false;
				StartCoroutine ( rotPlayer ( delTime ) );
			}
		}
	
		if ( useFord )
		{
			calTrans = pTrans.forward * speed * delTime;
		}
		else
		{
			calTrans = currVect * speed * delTime;
		}

		pTrans.Translate ( calTrans, Space.World );
	}

	bool waitRotate = false;
	IEnumerator rotPlayer ( float delTime )
	{
		Transform transPlayer = pTrans;
		Vector3 currRot = Vector3.zero;
		float calcTime = RotationSpeed * delTime;

		if ( InMadness || Dash )
		{
			calcTime *= 2;
		}

		waitRotate = true;

		transPlayer.DOKill ( );

		switch ( currentDir )
		{
		case Direction.North: 
			currVect = Vector3.forward;
			transPlayer.DOLocalRotate ( currRot, calcTime, RotateMode.Fast );
			break;
		case Direction.South: 
			currVect = Vector3.back;
			currRot = new Vector3 ( 0, 180, 0 );
			transPlayer.DOLocalRotate ( currRot, calcTime, RotateMode.Fast );
			break;
		case Direction.East: 
			currVect = Vector3.right;
			currRot = new Vector3 ( 0, 90, 0 );
			transPlayer.DOLocalRotate ( currRot, calcTime, RotateMode.Fast );
			break;
		case Direction.West: 
			currVect = Vector3.left;
			currRot = new Vector3 ( 0, -90, 0 );
			transPlayer.DOLocalRotate ( currRot, calcTime, RotateMode.Fast );
			break;
		}

		yield return new WaitForSeconds ( calcTime );

		yield return new WaitForEndOfFrame ( );
		transPlayer.localRotation = Quaternion.Euler ( currRot );
		waitRotate = false;
		useFord = true;
		currVect = pTrans.forward;
	}

	void changeLine ( float delTime )
	{
		float newImp = inputPlayer.GetAxis ( "Horizontal" );
		float lineDistance = Constants.LineDist;

		if ( ( canChange || newH == 0 ) && !inAir && !blockChangeLine )
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

				if ( saveDist < 0 && newH > -lineDistance * 0.60f || saveDist > 0 && newH < lineDistance * 0.60f )
				{
					canChange = true;
				}

				if ( saveDist < 0 && newH > -lineDistance * 0.40f || saveDist > 0 && newH < lineDistance * 0.40f )
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
		if ( inputPlayer.GetAxis ( "CoupDouble" ) != 0 && resetAxeD )
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
		}

		if(inputPlayer.GetAxis("CoupSimple") != 0 && canPunch && resetAxeS && GlobalManager.GameCont.introFinished )
        {
			Dash = false;
            thisCam.fieldOfView = Constants.DefFov;

			resetAxeS = false;
            canPunch = false;
            propP = true;
			timeToDP = TimeToDoublePunch;

			if (getDelta < 1)
                Time.timeScale = 1;

			this.MadnessMana(0);

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
			StartCoroutine( StartPunch ( 0 ) );
            propPunch = propulsePunch(TimePropulsePunch);
            StartCoroutine(propPunch);
		}
		else if( dpunch && canPunch )
        {
			Dash = false;
			thisCam.fieldOfView = Constants.DefFov;

            playAnimator.SetBool("ChargingPunch", false);

            dpunch = false;
			canPunch = false;

            ScreenShake.Singleton.ShakeHitDouble();

            GlobalManager.Ui.DoubleCoup();

			if (getDelta < 1)
                Time.timeScale = 1;
			
			this.MadnessMana(1);

            propDP = true;
			StartCoroutine ( StartPunch ( 1/*, timeToDP */) );

			propPunch = propulsePunch ( TimePropulseDoublePunch );
			StartCoroutine ( propPunch );

			timeToDP = TimeToDoublePunch;
        }
	}

	private IEnumerator StartPunch(int type_technic/*, float getTDp = 0*/ )
	{
		yield return new WaitForSeconds ( DelayPrepare );
		 
		if ( type_technic == 1 )
		{
			punch.setTechnic ( type_technic );
		}
		else
		{
			punch.setTechnic ( type_technic );
		}

        punchBox.enabled = true;
        StartCoroutine("TimerHitbox");

        Shader.SetGlobalFloat("_Saturation", 5);

		StartCoroutine ( CooldownPunch ( type_technic ) );
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

	IEnumerator CooldownDeadBall()
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
			Vector3 getThisC = thisColl.transform.position;

			if ( !onAnimeAir )
			{
				newPos = true;
				newDir = thisColl.GetComponent<NewDirect> ( ).NewDirection;
				blockChangeLine = false;
				getThisC = new Vector3 ( getThisC.x, 0, getThisC.z );

				Vector3 getPtr = pTrans.position;
				getPtr = new Vector3 ( getPtr.x, 0, getPtr.z );

				befRot = Vector3.Distance ( getThisC, getPtr );
			}
			else
			{
				currentDir = thisColl.GetComponent<NewDirect> ( ).NewDirection;
				pTrans.position = new Vector3 ( getThisC.x, pTrans.position.y, getThisC.z );
			}
		} 
	}

	void OnCollisionEnter ( Collision thisColl )
	{
		GameObject getObj = thisColl.gameObject;
		if ( onAnimeAir && thisColl.collider.tag == Constants._UnTagg )
		{
			GameOver ( true );
		}

		if ( Dash || InMadness || playerInv )
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
                thisColl.collider.enabled = false;

				if ( thisColl.gameObject.GetComponent<AbstractObject> ( ) )
				{
					if ( Dash )
					{
						thisColl.gameObject.GetComponent<AbstractObject> ( ).ForceProp ( getPunch.projection_dash * pTrans.forward, DeathType.Acceleration );
					}
					else if ( InMadness )
					{
						thisColl.gameObject.GetComponent<AbstractObject> ( ).ForceProp ( getPunch.projection_dash * pTrans.forward, DeathType.Madness );
					}
					else
					{
						thisColl.gameObject.GetComponent<AbstractObject> ( ).ForceProp ( getPunch.projection_dash * pTrans.forward, DeathType.Punch );
					}
				}
				return;
			}
			else if ( getObj.tag == Constants._Balls )
			{
				StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( getObj, pTrans, PropulseBalls, 1, 5, true, false, true ) );
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
		else if ( getObj.tag == Constants._EnnemisTag || getObj.tag == Constants._Balls )
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

		StartCoroutine ( camColor ( false ) );

		GlobalManager.Ui.CloseMadness();
	}
	#endregion
}
