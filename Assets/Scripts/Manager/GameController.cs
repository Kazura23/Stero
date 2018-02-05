using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Rewired;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.PostProcessing;
//using UnityEditor;
using System;

public class GameController : ManagerParent
{
	#region Variables
	public List<FxList> AllFx;
	public List<ChunkLock> ChunkToUnLock; 
	public Transform GarbageTransform;
	public MeshDesctruc MeshDest;
	[HideInInspector]
	public GameObject Player;
	public SpawnChunks SpawnerChunck;
	public GameObject BarrierIntro;
	public bool Intro;
    public bool UseTuto;

	public GameObject Tutoriel;

	[HideInInspector]
	public Dictionary <string, ItemModif> AllModifItem;
	[HideInInspector]
	public List <ItemModif> AllTempsItem;

	public Tween soundFootSteps;
	bool checkStart = false;
	bool isStay = true, isReady = false;
	[HideInInspector]
	public bool introFinished;
	private int chooseOption = 2;
	public Vector3[] moveRotate = new Vector3[5];
	public GameObject[] tabGameObject = new GameObject[5];
	public float delayRotate = 5;
	public GameObject textIntroObject;
	public Transform[] textIntroTransform;
	public string[] textIntroText;
	public Tween colorTw;
	public GameObject musicObject;
	[HideInInspector]
	public Camera thisCam;

	[HideInInspector]
	public bool LaunchTuto;

	[HideInInspector]
	public PostProcessingProfile postProfile;
    public PostProcessVolume postMadnessProfile;

	[HideInInspector]
	public List<Text> GetBonusText;

	Player inputPlayer;
	//Text textScore;

	Image getRank;
	Image iconeSpe;
	Slider sliderSpe;
	IEnumerator getCurWait;
	GameObject lastWall;
	bool restartGame = false;
	public bool canOpenShop = true;

	[Header ("Score Parametre")]
	public GameObject TextObj;
	public Rank[] AllRank;
	public ScoringInfo [] InfScore;

	bool GameStarted = false;
	bool onHub = true;
	bool coupSimpl = false;
	bool horiz = true;


    // reward menu
    private bool inReward = false;
    public Vector3 walkTowardReward;
    public float delayWalkReward = 1;
    public Vector3 ajustAngle;
    private int cursorTypeReward = 0, cursorReward = 0;
    private bool moveInReward = false;
    public Vector3[] rotateDeskReward = new Vector3[3];
    /*public RewardObject[] succes;
    public RewardObject[] defis;*/
    private Vector3 posInitPlayer;
    private bool isLookReward = false;
    private Transform lookReward;
    private Vector3 saveRewardPos;
    public Vector3 zoomRewardSucces;
    private bool canRotateReward = false;
    public float speedRotateReward = 5;
    public Sprite icon_sucess;
    public float delaySmoothPrintSucess = 0.5f;
    public float delayPrintSucess = 1;
    private Coroutine corouSucess = null;

	float rankValue = 0;

	int currIndex = -1;
	int currMax = 0;
	int currNeeded = 0;
	int lastNeeded = 0;

	int CurrentScore = 0;
    #endregion

    #region Mono
    private void Start()
    {
        posInitPlayer = Player.transform.position;
        AllPlayerPrefs.ANbRun = 0;
        inputPlayer = ReInput.players.GetPlayer(0);
    }

    void Update ( )
	{
		if ( GlobalManager.Ui.OnMenu )
		{
			return;
		}
		inputPlayer = ReInput.players.GetPlayer(0);
        posInitPlayer = Player.transform.position;
//        succes[0].Load();

        if(StaticRewardTarget.ListRewardEnAttente != null && StaticRewardTarget.ListRewardEnAttente.Count > 0 && corouSucess == null)
        {
            corouSucess = StartCoroutine(ListExecutionReward());
        }
        
		getRank.fillAmount = rankValue;

        BloomModel.Settings thisBloom = postProfile.bloom.settings;

        ChromaticAberrationModel.Settings thisChrom = postProfile.chromaticAberration.settings;

        thisBloom.bloom.intensity = currentValue;

        thisChrom.intensity = chromValue;

        postProfile.bloom.settings = thisBloom;

        postProfile.chromaticAberration.settings = thisChrom;
        //float thisMadnessWeight = postMadnessProfile.weight;

        //thisMadnessWeight = weightValue;
        postMadnessProfile.weight = weightValue;

		if ( inputPlayer.GetAxis ( "CoupSimple" ) == 0 )
		{
			coupSimpl = true;
		}

		if ( inputPlayer.GetAxis ( "Horizontal" ) == 0 )
		{
			horiz = true;
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			GlobalManager.Ui.OpenThisMenu(MenuType.Pause);
		}

        if (!checkStart && isStay && !isReady)
        {
			switch (chooseOption)
			{

			case 0: // Options

				if ( (inputPlayer.GetAxis ( "CoupSimple" ) == 1 || Input.GetKeyDown ( KeyCode.Return ) ) && coupSimpl )
				{
					GlobalManager.Ui.OpenThisMenu ( MenuType.Option );
					Debug.Log ( "Options" );
				}
				textIntroObject.transform.DOLocalMove(textIntroTransform[0].localPosition, 0);
				textIntroObject.transform.DOLocalRotate(textIntroTransform[0].localEulerAngles, 0);
				textIntroObject.GetComponent<TextMesh>().text = textIntroText[0];

				//GameStartedUpdate();
				break;

			case 1: // Leaderboards
				//Debug.Log("Leaderboards");
				textIntroObject.transform.DOLocalMove(textIntroTransform[1].localPosition, 0);
				textIntroObject.transform.DOLocalRotate(textIntroTransform[1].localEulerAngles, 0);
				textIntroObject.GetComponent<TextMesh>().text = textIntroText[1];
                if (inputPlayer.GetAxis("CoupSimple") == 1)
                {
                        isStay = false;
                        Player.transform.DOLocalMove(walkTowardReward, delayWalkReward).OnComplete(() =>
                        {
                            Player.transform.DOLocalRotate(ajustAngle, delayRotate).OnComplete(() => inReward = true);
                        });
                }
				break;

			case 2: //Start game

				//Debug.Log("Start");
				textIntroObject.transform.DOLocalMove(textIntroTransform[2].localPosition, 0);
				textIntroObject.transform.DOLocalRotate(textIntroTransform[2].localEulerAngles, 0);
				textIntroObject.GetComponent<TextMesh>().text = textIntroText[2];

				if ( ( inputPlayer.GetAxis("CoupSimple") == 1 || Input.GetKeyDown ( KeyCode.Return ) ) && coupSimpl && !restartGame)
				{
					coupSimpl = false;
					SetAllBonus ( );

					isStay = false;
					PlayerController getPlayer = Player.GetComponent<PlayerController> ( );

					if ( !LaunchTuto )
					{
						Animator getAnimator = Player.GetComponent<Animator> ( );
						getAnimator.enabled = true;

						getPlayer.PlayDirect.Play ( );
						//AnimationStartGame();

						DOVirtual.DelayedCall((float)getPlayer.PlayDirect.duration, () => {
							Player.transform.DOLocalMoveX ( 0, 0.7f );

							getAnimator.enabled = false;
							thisCam.transform.DOLocalMoveY(0.312f, 0.2f).OnComplete(() =>
							{
								//Player.GetComponentInChildren<RainbowMove>().enabled = true;
								Player.transform.DOMoveZ(3, 0.5f).OnComplete(() =>
								{
									isReady = true;
									isStay = true;
									//getPlayer.playAnimator.SetBool ( "WaitDoor", true );
									//Player.GetComponent<PlayerController>().StopPlayer = false;
									//Debug.Log("anime fonctionnelle");
								});
							});
						});

						/*
						 // Cri de Mr S après avoir pris sa dose
						GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Intro", false);


						// Démarrage de la musique du Hub amplifiée après Stéro
						musicObject.GetComponent<AudioSource>().volume = 0.0004f;
						musicObject.GetComponent<AudioLowPassFilter>().enabled = true;
						musicObject.GetComponent<AudioDistortionFilter>().enabled = true;
						*/
					}
					else
					{
						DOTween.To(() => GlobalManager.GameCont.currentValue, x => GlobalManager.GameCont.currentValue = x, .3f, .25f).OnComplete(() => {
							DOTween.To(() => GlobalManager.GameCont.currentValue, x => GlobalManager.GameCont.currentValue = x, 0, .12f);
						});

						DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 1f, .6f).OnComplete(() => {
							DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 0, .4f);
						});

						foreach ( Collider thisColl in lastWall.GetComponentsInChildren<Collider>() )
						{
							thisColl.enabled = false;
						}

						lastWall.GetComponent<CapsuleCollider> ( ).enabled = false;
						lastWall.transform.Find ( "Porte" ).DOLocalRotate ( new Vector3 ( 0, -90, 0 ), 1 );

						Player.transform.DOLocalMoveX ( 0, 0.7f );

						thisCam.transform.DOLocalMoveY(0.312f, 0.2f).OnComplete(() =>
						{
							//Player.GetComponentInChildren<RainbowMove>().enabled = true;
							Player.transform.DOMoveZ(3, 0.5f).OnComplete(() =>
							{
								isReady = true;
								isStay = true;
								ActiveGame ( );
								//getPlayer.playAnimator.SetBool ( "WaitDoor", true );

								//Player.GetComponent<PlayerController>().StopPlayer = false;
								//Debug.Log("anime fonctionnelle");
							});
						});
					}
				}

				break;

			case 3: // Shop
				//Debug.Log("Shop");


				textIntroObject.transform.DOLocalMove ( textIntroTransform [ 3 ].localPosition, 0 );
				textIntroObject.transform.DOLocalRotate ( textIntroTransform [ 3 ].localEulerAngles, 0 );
				textIntroObject.GetComponent<TextMesh> ( ).text = textIntroText [ 3 ];

				ActiveTextIntro ( );
				if ( ( inputPlayer.GetAxis ( "CoupSimple" ) == 1 || Input.GetKeyDown ( KeyCode.Return ) ) && coupSimpl && GlobalManager.GameCont.canOpenShop )
				{
					coupSimpl = false;
					GlobalManager.Ui.OpenThisMenu(MenuType.Shop);
				}
					
				break;

			case 4:  // Quitter
				//Debug.Log("Quit");
				textIntroObject.transform.DOLocalMove(textIntroTransform[4].localPosition, 0);
				textIntroObject.transform.DOLocalRotate(textIntroTransform[4].localEulerAngles, 0);
				textIntroObject.GetComponent<TextMesh>().text = textIntroText[4];

				break;
			}
	           
			if (inputPlayer.GetAxis("Horizontal") == -1 && horiz)
			{
				horiz = false;
				ActiveTextIntro();
				ChooseRotate(false);
			}
			else if (inputPlayer.GetAxis("Horizontal") == 1 && horiz)
			{
				horiz = false;
				ActiveTextIntro();
				ChooseRotate(true);
			}
		}
		else if (isReady && inputPlayer.GetAxis("CoupSimple") == 1 && coupSimpl && !restartGame && isStay )
	        {
			    coupSimpl = false;
	            Player.GetComponent<PlayerController>().GetPunchIntro();
	        }
        else
        {
            RewardMenu();
        }
        
	}

    private void OnDestroy()
    {
        if (AllPlayerPrefs.canSendAnalytics)
        {
            var resultat = Analytics.CustomEvent("Nombre de run", new Dictionary<string, object>
            {
                { "Nombre total de run", AllPlayerPrefs.ANbRun}
            });
            if (resultat.Equals(AnalyticsResult.Ok))
                Debug.Log(resultat);
            else
                Debug.LogWarning(resultat);
        }
    }

    void ActiveTextIntro()
    {
        colorTw = DOVirtual.DelayedCall(.2f, () => {
            Color alph = textIntroObject.GetComponent<TextMesh>().color;
            alph.a = 1;
            textIntroObject.GetComponent<TextMesh>().color = alph;
        });
    }

    private IEnumerator ListExecutionReward()
    {
        var tempReward = StaticRewardTarget.ListRewardEnAttente[0];
        var tempAttribut = tempReward.GetComponent<CanvasGroup>();
        DOTween.To(() => tempAttribut.alpha, x => tempAttribut.alpha = x, 1, delaySmoothPrintSucess);
        yield return new WaitForSeconds(delaySmoothPrintSucess + delayPrintSucess);
        DOTween.To(() => tempAttribut.alpha, x => tempAttribut.alpha = x, 0, delaySmoothPrintSucess).OnComplete(() =>
        {
            StaticRewardTarget.ListRewardEnAttente.Remove(tempReward);
            Destroy(tempReward);
            corouSucess = null;
        });

    }
    #endregion

    #region Public Methods
	public void IniFromUI ( )
	{
		inputPlayer = ReInput.players.GetPlayer(0);
		//textScore = GlobalManager.Ui.ScorePoints;
		AllPlayerPrefs.ANbRun = 0;
		Player.GetComponent<PlayerController> ( ).IniPlayer ( );
		
		getRank = GlobalManager.Ui.RankSlider;
		iconeSpe = GlobalManager.Ui.SlowMotion;
		sliderSpe = GlobalManager.Ui.MotionSlider;
	}


    public void ActiveGame()
    {
        GameStartedUpdate();
    }
    
	public void StartGame ( )
	{
        //GameObject thisObj = ( GameObject ) Instantiate ( BarrierIntro );
        AllPlayerPrefs.ATimerRun = 0;
        AllPlayerPrefs.ANbRun++;
		if ( lastWall != null )
		{
			Destroy ( lastWall );
		}

		int a;
		for ( a = 0; a < GetBonusText.Count; a++ )
		{
			GetBonusText [ a ].text = "LEVEL 1";

			GetBonusText [ a ].transform.parent.GetComponent<ItemModif> ( ).ResetPrice ( );
		}

		lastWall = ( GameObject ) Instantiate ( BarrierIntro );
        //Debug.Log("Start");
        AllPlayerPrefs.ResetStaticVar();
        StaticRewardTarget.ResetVar();
	
		PlayerController thisPcontr = Player.GetComponent<PlayerController> ( );

		thisPcontr.ResetPlayer ( );
		thisPcontr.ThisAct = SpecialAction.Nothing;

		Intro = true;
		isStay = true;

		currNeeded = 0;
		currIndex = 0;
		currMax = 0;
		CurrentScore = 0;
		lastNeeded = 0;

		DOTween.Kill ( rankValue );
		DOTween.To ( ( ) => rankValue, x => rankValue = x, 0, 0.1f );
		Rank [] getListRank = AllRank;

		for ( a = 0; a < getListRank.Length; a++ )
		{
			if ( getListRank [ a ].NeededScore < getListRank [ currIndex ].NeededScore )
			{
				currIndex = a;
			}
		}

		currMax = getListRank [ currIndex ].NeededScore;

		for ( a = 0; a < getListRank.Length; a++ )
		{
			if ( currMax >= currNeeded )
			{
				currNeeded = getListRank [ a ].NeededScore;
			}
			else if ( getListRank [ a ].NeededScore > currMax && getListRank [ a ].NeededScore < currNeeded )
			{
				currNeeded = getListRank [ a ].NeededScore;
			}
		}

		if ( currNeeded <= 0 )
		{
			currNeeded = 1;
		}

		GlobalManager.Ui.Multiplicateur.text = getListRank [ currIndex ].MultiPli.ToString ( );
		GlobalManager.Ui.RankText.color = getListRank [ currIndex ].Color;
		GlobalManager.Ui.RankText.text = getListRank [ currIndex ].NameRank;

		if ( getCurWait != null )
		{
			StopCoroutine ( getCurWait );
		}

		if ( restartGame )
        {
			onHub = false;
			isStay = false;
			Intro = false;

			Player.transform.DOMoveZ ( 3, 0.75f ).OnComplete ( ( ) =>
			{
				thisPcontr.GetPunchIntro ( );
				thisPcontr.StopPlayer = false;
				restartGame = false;
				GlobalManager.Ui.IntroRestart ( );
			} );
		}
		else
		{
			onHub = true;
			GlobalManager.AudioMa.CloseAllAudio ( );
			GlobalManager.AudioMa.CloseUnLoopAudio ( AudioType.MusicTrash, true );
			GlobalManager.AudioMa.OpenAudio ( AudioType.MusicBackGround, "Menu", true, null );
		}

		SetAllBonus ( );

		GameStarted = true;
		checkStart = false;

		if ( !LaunchTuto )
		{
			SpawnerChunck.FirstSpawn ( );
		}
		else
		{
			AllPlayerPrefs.SetStringValue ( Constants.TutoName );
			Instantiate ( Tutoriel );
		}

		thisCam.GetComponent<RainbowRotate>().time = 2;
		thisCam.GetComponent<RainbowMove>().time = 1;
		GlobalManager.Ui.CloseThisMenu ( );
    }

	public GameObject FxInstanciate ( Vector3 thisPos, string fxName, Transform parentObj = null, float timeDest = 0.35f )
	{
		List<FxList> getAllFx = AllFx;
		GameObject getObj;

		for ( int a = 0; a < getAllFx.Count; a++ )
		{
			if ( getAllFx [ a ].FxName == fxName )
			{
				getObj = getAllFx [ a ].FxObj;

				if ( parentObj != null )
				{
					getObj = ( GameObject ) Instantiate ( getObj, parentObj );
				}
				else
				{
					getObj = ( GameObject ) Instantiate ( getObj );
				}

				getObj.transform.position = thisPos;

				Destroy ( getObj, timeDest );

				return getObj;
			}
		}

		return null;
	}

    public void Restart () 
	{
        Time.timeScale = 1;
        AllPlayerPrefs.ATimerRun = 0;
        AllPlayerPrefs.ANbRun++;
		GlobalManager.Ui.thisCam.transform.DOKill ();
		ScreenShake.Singleton.StopShake ( );

        AllPlayerPrefs.ResetStaticVar();
        StaticRewardTarget.ResetVar();
		//SceneManager.LoadScene ( "MainScene", LoadSceneMode.Single );
        GlobalManager.Ui.DashSpeedEffect(false);
       
        checkStart = false;
        
        if (AllPlayerPrefs.relance)
        {
			restartGame = true;
            isReady = true;
            GameStarted = true;

			Player.GetComponent<PlayerController>().StopPlayer = false;
			thisCam.GetComponent<RainbowRotate>().time = .4f;
			thisCam.GetComponent<RainbowMove>().time = .2f;

			soundFootSteps = DOVirtual.DelayedCall(GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed / GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed - GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed / 25, () => {
				//Debug.Log("here");
				int randomSound = UnityEngine.Random.Range(0, 6);

				GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "FootSteps_" + (randomSound + 1), false);
				//J'ai essayé de jouer le son FootSteps_1 pour voir, mais ça marche
				//Debug.Log("Audio");
			}).SetLoops(-1, LoopType.Restart);
            //GameStartedUpdate();
           // StartCoroutine(TrashFunction());
        }
        else
        {
            isReady = false;
            GameStarted = false;
        }

		StartGame ( );
        //GameStarted = false;
    }   
    
	public void UnLockChunk ( ChunksScriptable thisScript, GameObject ThisChunk ) 
	{ 
		thisScript.TheseChunks.Add ( ThisChunk ); 

		AllPlayerPrefs.SetStringValue ( Constants.ChunkUnLock + ThisChunk.name ); 
	} 

	public void NewScore ( DeathType thisDeath, int nbrPoint )
	{
		//AllPlayerPrefs.scoreWhithoutDistance += point;
		if ( getCurWait != null )
		{
			StopCoroutine ( getCurWait );
		}

		if ( currIndex >= 0 )
		{
			getCurWait = waitRank ( AllRank [ currIndex ].Time );

			StartCoroutine ( getCurWait );
		}

		ScoringInfo getInfS;
		GameObject CurrText;

		for ( int a = 0; a < InfScore.Length; a++ )
		{
			getInfS = InfScore [ a ];
			if ( getInfS.TypeDeath == thisDeath )
			{
				if ( currIndex >= 0 )
				{
					getInfS.AllScore += nbrPoint * getInfS.Multiplicateur * AllRank [ currIndex ].MultiPli;
				}
				else
				{
					getInfS.AllScore += nbrPoint * getInfS.Multiplicateur;
				}

				if ( getInfS.WaitCulmul )
				{
					CurrText = ( GameObject ) Instantiate ( TextObj, GlobalManager.Ui.GameParent );
					CurrText.GetComponent<Text> ( ).text = "" + nbrPoint;

					getInfS.CurrSpawn.Add ( CurrText );
					getInfS.CurrCount++;

					if ( getInfS.CurrWait != null )
					{
						StopCoroutine ( getInfS.CurrWait );
					}

					getInfS.CurrWait = waitScore ( getInfS );
					StartCoroutine ( getInfS.CurrWait );
				}
				else
				{
					getInfS.CurrCount ++;
					addNewScore ( getInfS );
				}
				break;
			}
	
		}
	}

	public void SetAllBonus ( )
	{
		PlayerController currPlayer = Player.GetComponent<PlayerController> ( );

		if ( !LaunchTuto )
		{
			iconeSpe.enabled = false;
			iconeSpe.DOFade ( 0, 0.3f );
			sliderSpe.gameObject.SetActive ( false );
			sliderSpe.GetComponent<CanvasGroup> ( ).DOFade ( 0, .3f );

			currPlayer.SlowMotion = 1.25f; 
			currPlayer.SpeedSlowMot = 1; 
			currPlayer.SpeedDeacSM = 5; 
			currPlayer.ReduceSlider = 1.5f; 
			currPlayer.RecovSlider = 0.75f; 
		}

		Dictionary <string, ItemModif> getMod = AllModifItem;
		List <ItemModif> AllTI = AllTempsItem;
		ItemModif thisItem;
		//List<string> getKey = new List<string> ( );

		SpawnerChunck.EndLevel = 1;

		if ( getMod != null )
		{
			foreach ( KeyValuePair <string, ItemModif> thisKV in getMod )
			{
				thisItem = thisKV.Value;

				setItemToPlayer ( thisItem, currPlayer );
			}
		}

		if ( AllTI != null )
		{
			while ( AllTI.Count > 0 )
			{
				setItemToPlayer ( AllTI [ 0 ], currPlayer );

				AllTI.RemoveAt ( 0 );
			}
		}
	}
    #endregion

    #region Private Methods
	IEnumerator waitScore ( ScoringInfo thisInf )
	{
		yield return new WaitForSeconds ( thisInf.SecCumul );

		addNewScore ( thisInf );
	}

	void addNewScore ( ScoringInfo thisInf )
	{
		//GameObject newObj = ( GameObject ) Instantiate ( TextObj, GlobalManager.Ui.GameParent );
		//newObj.GetComponent<Text> ( ).text = "" + thisInf.AllScore;
		Rank[] getAllRank = AllRank;
		int a;
		for ( a = 0; a < thisInf.CurrSpawn.Count; a++ )
		{
			Destroy ( thisInf.CurrSpawn [ a ] );
		}
		//Destroy ( newObj, 3 );
		CurrentScore += thisInf.AllScore * thisInf.CurrCount;

		int currInd = currIndex;

		for ( a = 0; a < getAllRank.Length; a++ )
        {
			if ( a != currInd && getAllRank [ a ].NeededScore < CurrentScore && AllRank [ a ].NeededScore > currMax )
			{
				currInd = a;
            }
        }

		GlobalManager.Ui.ScorePlus ( thisInf.AllScore, getAllRank [ currInd ].Color, currIndex );

        if ( currInd != currIndex )
		{
			GlobalManager.Ui.RankText.color = getAllRank [ currInd ].Color;
			GlobalManager.Ui.Multiplicateur.text = getAllRank [ currInd ].MultiPli.ToString ( );
			GlobalManager.Ui.RankText.text = getAllRank [ currInd ].NameRank;

			lastNeeded = currNeeded;
			currMax = getAllRank [ currInd ].NeededScore;
		
			currIndex = currInd;

			for ( a = 0; a < getAllRank.Length; a++ )
			{
				if ( currMax >= currNeeded )
				{
					currNeeded = getAllRank [ a ].NeededScore;
				}
				else if ( getAllRank [ a ].NeededScore > currMax && getAllRank [ a ].NeededScore < currNeeded )
				{
					currNeeded = getAllRank [ a ].NeededScore;
				}
			}

			if ( getCurWait != null )
			{
				StopCoroutine ( getCurWait );
			}


			getCurWait = waitRank ( getAllRank [ currInd ].Time );


			StartCoroutine ( getCurWait );


            GlobalManager.Ui.NewRank(currInd);


            float getNewRank = (float)(CurrentScore - lastNeeded) / (currNeeded - lastNeeded);
            DOTween.Kill(rankValue);
            DOTween.To(() => rankValue, x => rankValue = x, getNewRank, 0.1f);

            thisInf.AllScore = 0;
            thisInf.CurrCount = 0;
            thisInf.CurrSpawn.Clear();
        }

    }

	void setMusic () 
	{ 
		GlobalManager.AudioMa.CloseUnLoopAudio ( AudioType.MusicTrash );
		GlobalManager.AudioMa.CloseUnLoopAudio ( AudioType.MusicBackGround );
		GlobalManager.AudioMa.OpenAudio ( AudioType.MusicBackGround, "", false, setMusic ); 
    } 

	private IEnumerator TrashFunction()
	{
		yield return new WaitForSeconds(0.5f); //=> attendre 0.5 seconde ok (mais code deguelasse)
		GameStartedUpdate();
	}

	IEnumerator waitRank ( float secs )
	{
		yield return new WaitForSeconds ( secs );

		currIndex = 0;
		Rank [] getListRank = AllRank;
		Image getRankSlid = getRank;

		for ( int a = 0; a < getListRank.Length; a++ )
		{
			if ( getListRank [ a ].NeededScore < getListRank [ currIndex ].NeededScore )
			{
				currIndex = a;
			}
		}

		currMax = 0;
		CurrentScore = 0;
		GlobalManager.Ui.Multiplicateur.text = getListRank [ currIndex ].MultiPli.ToString ( );
		GlobalManager.Ui.RankText.color = getListRank [ currIndex ].Color;
		GlobalManager.Ui.RankText.text = getListRank [ currIndex ].NameRank;

		getRankSlid.transform.parent.GetComponent<CanvasGroup>().DOFade(0, .3f);
		getRankSlid.transform.parent.transform.DOScale(0, .3f);

		getRankSlid.fillAmount = 0;
	}

    private void AnimationStartGame() // don't forget freeze keyboard when animation time
    {
        Player.transform.DORotate(new Vector3(0, 90, 0), 2).OnComplete(()=> 
        {
            //animation seringue + son
            Player.transform.DORotate(new Vector3(-65, 0, 0), 2).OnComplete(()=> 
            {
                // activation shader + son
                /*for(int i = 0; i < textMeshs.childCount; i++) // voir si active la liste des text mesh ou un par un
                {
                    textMeshs.GetChild(i).gameObject.SetActive(true);
                }*/
                //Player.GetComponentInChildren<RainbowRotate>().enabled = true;
                Player.transform.DORotate(Vector3.zero, 1).OnComplete(()=> 
                {
                    // Cri de Mr S après avoir pris sa dose
                    GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Intro", false);


                    // Démarrage de la musique du Hub amplifiée après Stéro
                    musicObject.GetComponent<AudioSource>().volume = 0.0004f;
                    musicObject.GetComponent<AudioLowPassFilter>().enabled = true;
                    musicObject.GetComponent<AudioDistortionFilter>().enabled = true;

					thisCam.transform.DOLocalMoveY(0.312f, 1).OnComplete(() =>
                    {
                        //Player.GetComponentInChildren<RainbowMove>().enabled = true;


                        Player.transform.DOMoveZ(3, 1).OnComplete(() =>
                        {
                            isReady = true;
                            isStay = true;
                            //Player.GetComponent<PlayerController>().StopPlayer = false;
                            //Debug.Log("anime fonctionnelle");
                        });
                    });
                });
            });
        });
    }

    private IEnumerator TimerRotate()
    {
        yield return new WaitForSeconds(delayRotate);
        isStay = true;
    }

    private void RewardMenu()
    {
        if (inReward && !moveInReward && !isLookReward)
        {
            switch (cursorTypeReward)
            {
                case 0: // leaderboard
                    // afficher les leaderboards
                    break;
                case 1: // succes
                    if(transform.GetChild(0).childCount > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            ChooseViewReward(true);
                        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            ChooseViewReward(false);
                        }/*else if (Input.GetKeyDown(KeyCode.W) && succes[cursorReward].isUnlock)
                        {
                            lookReward = succes[cursorReward].transform;
                            saveRewardPos = lookReward.position;
                            isLookReward = true;
                            lookReward.DOMove(zoomRewardSucces, delayWalkReward).OnComplete(()=>
                            {
                                canRotateReward = true;
                            });
                        }*/
                        //afficher effet
                    }
                    break;
                /*case 2: // defis
                    if(defis.Length > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            ChooseViewReward(false, true);
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            ChooseViewReward(false, false);
                        }
                        // afficher effet
                    }
                    break;*/
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveInReward = true;
                cursorTypeReward++;
                if (cursorTypeReward > 1)
                    cursorTypeReward = 0;
                RotateViewReward();
            }else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveInReward = true;
                cursorTypeReward--;
                if (cursorTypeReward < 0)
                    cursorTypeReward = 1;
                RotateViewReward();
            }else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                inReward = false;
                if(cursorTypeReward != 0)
                {
                    var tempEfface = StaticRewardTarget.listRewardTrans.GetChild(cursorReward).GetComponent<RewardObject>().afficheReward.GetComponent<CanvasGroup>();
                    DOTween.To(() => tempEfface.alpha, x => tempEfface.alpha = x, 0, delaySmoothPrintSucess);
                    cursorReward = 0;
                }
                Player.transform.DOLocalRotate(moveRotate[chooseOption], Player.transform.localEulerAngles == moveRotate[chooseOption] ? 0 : delayRotate).OnComplete(() =>
                {
                    Player.transform.DOMove(posInitPlayer, delayWalkReward).OnComplete(() =>
                    {
                        isStay = true;
                        cursorTypeReward = 0;
                    });
                });
            }
        }else if (canRotateReward)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                canRotateReward = false;
                lookReward.eulerAngles = new Vector3(0, 0, 0);
                lookReward.DOMove(saveRewardPos, delayWalkReward).OnComplete(() =>
                {
                    isLookReward = false;
                });
            }else if(Input.GetKey(KeyCode.RightArrow))
            {
                lookReward.Rotate(0, speedRotateReward * Time.deltaTime, 0);
            }else if(Input.GetKey(KeyCode.LeftArrow))
            {
                lookReward.Rotate(0, -1 * speedRotateReward * Time.deltaTime, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                lookReward.Rotate(speedRotateReward * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                lookReward.Rotate(-1 * speedRotateReward * Time.deltaTime, 0, 0);
            }
        }
    }

    private void RotateViewReward()
    {
        
        
        if(cursorTypeReward == 0)
        {
            var tempEfface = StaticRewardTarget.listRewardTrans.GetChild(cursorReward).GetComponent<RewardObject>().afficheReward.GetComponent<CanvasGroup>();
            DOTween.To(() => tempEfface.alpha, x => tempEfface.alpha = x, 0, delaySmoothPrintSucess).OnComplete(() => 
            {
                Player.transform.DOLocalRotate(rotateDeskReward[cursorTypeReward] + moveRotate[chooseOption], delayRotate).OnComplete(() =>
                {
                    cursorReward = 0;
                    moveInReward = false;
                });
            });
        }
        else
        {
            Player.transform.DOLocalRotate(rotateDeskReward[cursorTypeReward] + moveRotate[chooseOption], delayRotate).OnComplete(() =>
            {
                var tempEfface = StaticRewardTarget.listRewardTrans.GetChild(cursorReward).GetComponent<RewardObject>().afficheReward.GetComponent<CanvasGroup>();
                DOTween.To(() => tempEfface.alpha, x => tempEfface.alpha = x, 1, delaySmoothPrintSucess).OnComplete(() => 
                {
                    moveInReward = false;
                });
            });
                
        }
        
    }

    private void ChooseViewReward(bool p_dir)
    {
        moveInReward = true;
        if (p_dir)
        {
            var tempEfface = StaticRewardTarget.listRewardTrans.GetChild(cursorReward).GetComponent<RewardObject>().afficheReward.GetComponent<CanvasGroup>();
            DOTween.To(() => tempEfface.alpha, x => tempEfface.alpha = x, 0, delaySmoothPrintSucess).OnComplete(() =>
            {
                cursorReward++;
                if (cursorReward >= transform.GetChild(0).childCount)
                    cursorReward = 0;
                Debug.Log(cursorReward);
                var tempAffiche = StaticRewardTarget.listRewardTrans.GetChild(cursorReward).GetComponent<RewardObject>().afficheReward.GetComponent<CanvasGroup>();
                DOTween.To(() => tempAffiche.alpha, x => tempAffiche.alpha = x, 1, delaySmoothPrintSucess).OnComplete(() => moveInReward = false);
            });
            
        }
        else
        {
            var tempEfface = StaticRewardTarget.listRewardTrans.GetChild(cursorReward).GetComponent<RewardObject>().afficheReward.GetComponent<CanvasGroup>();
            DOTween.To(() => tempEfface.alpha, x => tempEfface.alpha = x, 0, delaySmoothPrintSucess).OnComplete(() =>
            {
                cursorReward--;
                if (cursorReward < 0)
                    cursorReward = transform.GetChild(0).childCount - 1;
                Debug.Log(cursorReward);
                var tempAffiche = StaticRewardTarget.listRewardTrans.GetChild(cursorReward).GetComponent<RewardObject>().afficheReward.GetComponent<CanvasGroup>();
                DOTween.To(() => tempAffiche.alpha, x => tempAffiche.alpha = x, 1, delaySmoothPrintSucess).OnComplete(() => moveInReward = false);
            });
        }
    }

    /*private void ChooseViewReward(bool p_succes, bool p_dir)
    {
        if (p_succes)
        {
            if (p_dir)
            {
                cursorReward++;
                if (cursorReward >= succes.Length)
                    cursorReward = 0;
            }
            else
            {
                cursorReward--;
                if (cursorReward < 0)
                    cursorReward = succes.Length - 1;
            }
        }
        else
        {
            if (p_dir)
            {
                cursorReward++;
                if (cursorReward >= defis.Length)
                    cursorReward = 0;
            }
            else
            {
                cursorReward--;
                if (cursorReward < 0)
                    cursorReward = defis.Length - 1;
            }
        }
    }*/

    private void LoadRewardInit()
    {
        
        var rewardSave = SaveDataReward.Load();
        if (rewardSave == null)
            return;
        for (int i = 0; i < rewardSave.Count; i++)
        {
            Debug.Log("id = " + rewardSave[i].id+", is unlock = "+rewardSave[i].unlock);
        }

        var listReward = StaticRewardTarget.listRewardTrans;
        for (int i = 0; i< listReward.childCount; i++)
        {
            var reward = listReward.GetChild(i).GetComponent<RewardObject>();
            for(int j = 0; j < rewardSave.Count; j++)
            {
                if(rewardSave[j].id == reward.idReward)
                {
                    if (rewardSave[j].unlock)
                    {
                        reward.UnlockWithFile();
                        Debug.Log("sprite find : "+icon_sucess);
                        reward.icon.sprite = icon_sucess;
                    }
                    break;
                }
            }
        }
    }

    private void ChooseRotate(bool p_add)
    {
		if ( !Intro )
		{
			return;
		}
        //colorTw.Kill(true);

        Color alphachg = textIntroObject.GetComponent<TextMesh>().color;
        alphachg.a = 0;
        textIntroObject.GetComponent<TextMesh>().color = alphachg;

        if (p_add && GlobalManager.Ui.menuOpen == MenuType.Nothing)
        {
            chooseOption++;
            if (chooseOption == moveRotate.Length)
                chooseOption = 0;
        }
        else 
        {
            if(GlobalManager.Ui.menuOpen == MenuType.Nothing)
            {
                chooseOption--;
                if (chooseOption == -1)
                    chooseOption = moveRotate.Length - 1;

            }
        }
        isStay = false;
        StartCoroutine(TimerRotate());
        Player.transform.DOLocalRotate(moveRotate[chooseOption], delayRotate);
    }

    private void GameStartedUpdate()
    {
        /*if (Input.GetAxis("CoupSimple") == 1 || Input.GetAxis("CoupDouble") == 1)
        {*/

		if ( onHub )
		{
			onHub = false;
			GlobalManager.AudioMa.CloseAudio ( AudioType.MusicBackGround );
			GlobalManager.AudioMa.CloseAudio ( AudioType.MusicTrash );
			GlobalManager.AudioMa.CloseUnLoopAudio ( AudioType.MusicTrash );

			AudioSource thisAud = GlobalManager.AudioMa.OpenAudio ( AudioType.MusicTrash, "", false, setMusic );
			thisAud.volume *= 1.25f;

			thisAud.DOFade ( thisAud.volume * 0.75f, 3.5f );
		}

        if (GameStarted && !checkStart)
        {

            GlobalManager.Ui.Intro();
            isStay = false;

        //GlobalManager.AudioMa.OpenAudio(AudioType.MusicBackGround, "", false);

        	musicObject.GetComponent<AudioLowPassFilter>().enabled = false;
            musicObject.GetComponent<AudioDistortionFilter>().enabled = false;


            checkStart = true;
            //Debug.Log("player = " + Player);
            Player.GetComponent<PlayerController>().StopPlayer = false;
			//Player.GetComponent<PlayerController>().playAnimator.SetBool ( "WaitDoor", false );

			//thisCam.GetComponent<RainbowRotate>().time = .4f;
			//thisCam.GetComponent<RainbowMove>().time = .2f;

            soundFootSteps = DOVirtual.DelayedCall(GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed / GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed - GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed / 25, () => {
                //Debug.Log("here");
                int randomSound = UnityEngine.Random.Range(0, 6);

                GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "FootSteps_" + (randomSound + 1), false);
                //J'ai essayé de jouer le son FootSteps_1 pour voir, mais ça marche
                //Debug.Log("Audio");
            }).SetLoops(-1, LoopType.Restart);
        }
            /*else
            {
                // punch the door
            }
        }*/

        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 playerPos = GlobalManager.GameCont.Player.transform.position;
            GameObject thisGO = GlobalManager.GameCont.FxInstanciate(new Vector3(.16f, 0.12f, 0.134f) + playerPos, "PlayerReady", GlobalManager.GameCont.Player.transform, 1f);
            thisGO.transform.SetParent(GlobalManager.GameCont.Player.GetComponent<PlayerController>().rightHand.transform);
            thisGO.transform.DOLocalMove(Vector3.zero, 0);

            GameObject thisGOLeft = GlobalManager.GameCont.FxInstanciate(new Vector3(.16f, 0.12f, 0.134f) + playerPos, "PlayerReady", GlobalManager.GameCont.Player.transform, 1f);
            thisGOLeft.transform.SetParent(GlobalManager.GameCont.Player.GetComponent<PlayerController>().leftHand.transform);
            thisGOLeft.transform.DOLocalMove(Vector3.zero, 0);
        }
    }

    public float currentValue = 0;
    public float chromValue = 0;
    public float weightValue = 0;

    /*public void updateBloom ( )
     {
         BloomModel.Settings thisBloom = postProfile.bloom.settings;

         thisBloom.bloom.intensity = currentValue;

         postProfile.bloom.settings = thisBloom;
     }*/

    protected override void InitializeManager ( )
	{
		if ( UseTuto )
		{
			LaunchTuto = !AllPlayerPrefs.GetBoolValue ( Constants.TutoName );
		}
		else
		{
			LaunchTuto = false;
		}

		Player = GameObject.FindGameObjectWithTag("Player");
		thisCam = Player.GetComponentInChildren<Camera> ( );
        musicObject = GlobalManager.AudioMa.transform.Find("Music").gameObject;

		var behaviour = thisCam.GetComponent<PostProcessingBehaviour>();

		postProfile = Instantiate(behaviour.profile, transform);
		behaviour.profile = postProfile;

        SpawnerChunck = GetComponentInChildren<SpawnChunks> ( );
		SpawnerChunck.InitChunck ( );

        StaticRewardTarget.icon_sucess = icon_sucess;
        StaticRewardTarget.ListRewardEnAttente = new List<GameObject>();
        AllPlayerPrefs.saveData = SaveData.Load();
        StaticRewardTarget.listRewardTrans = transform.GetChild(0);
        LoadRewardInit();

		List<ChunkLock> GetChunk = ChunkToUnLock; 
		List<NewChunk> CurrList; 

		int b; 

		for ( int a = 0; a < GetChunk.Count; a++ ) 
		{ 
			CurrList = GetChunk [ a ].AllUnlock; 
			for ( b = 0; b < CurrList.Count; b++ ) 
			{ 
				if ( AllPlayerPrefs.GetBoolValue ( Constants.ChunkUnLock + CurrList[ b ].ThisChunk.name ) ) 
				{ 
					UnLockChunk ( GetChunk [ a ].ThisChunk, CurrList [ b ].ThisChunk ); 
					GetChunk [ a ].AllUnlock.RemoveAt ( b ); 
					b--; 
				} 
			} 

			if ( GetChunk [ a ].AllUnlock.Count == 0 ) 
			{ 
				GetChunk.RemoveAt ( a ); 
				a--; 
			} 
		} 
	}

	void setItemToPlayer ( ItemModif thisItem, PlayerController currPlayer )
	{
		if ( thisItem.ModifSpecial )
		{
			iconeSpe.enabled = true;
			iconeSpe.DOKill ( );
			iconeSpe.DOFade ( 1, 1 );

			sliderSpe.gameObject.SetActive ( true );
			sliderSpe.GetComponent<CanvasGroup> ( ).DOKill ( );
			sliderSpe.GetComponent<CanvasGroup> ( ).DOFade ( 1, .3f );

			currPlayer.ThisAct = thisItem.SpecAction;

			switch ( thisItem.SpecAction )
			{
			case SpecialAction.OndeChoc:
				currPlayer.delayChocWave = thisItem.SliderTime;
                AllPlayerPrefs.ANameTechSpe = "Onde de choc";
				break;
			case SpecialAction.DeadBall:
				currPlayer.DelayDeadBall = thisItem.SliderTime;
                AllPlayerPrefs.ANameTechSpe = "Boule de la mort";
				break;
			default:
                AllPlayerPrefs.ANameTechSpe = "Slow Motion";
				currPlayer.DelaySlowMot = thisItem.SliderTime;
				break;
			}

			currPlayer.SliderSlow.maxValue = thisItem.SliderTime;
			currPlayer.SliderSlow.value = thisItem.SliderTime;

			if ( thisItem.SpecAction == SpecialAction.SlowMot ) 
			{ 
				currPlayer.SlowMotion = thisItem.SlowMotion; 
				currPlayer.SpeedSlowMot = thisItem.SpeedSlowMot; 
				currPlayer.SpeedDeacSM = thisItem.SpeedDeacSM; 
				currPlayer.ReduceSlider = thisItem.ReduceSlider; 
				currPlayer.RecovSlider = thisItem.RecovSlider; 
			} 
			else if ( thisItem.SpecAction == SpecialAction.DeadBall ) 
			{ 
				currPlayer.DistDBTake = thisItem.DistTakeDB; 
				if ( thisItem.BonusItem ) 
				{ 
					currPlayer.SlowMotion += thisItem.SlowMotion; 
					currPlayer.SpeedSlowMot += thisItem.SpeedSlowMot; 
					currPlayer.SpeedDeacSM += thisItem.SpeedDeacSM; 
					currPlayer.ReduceSlider += thisItem.ReduceSlider; 
					currPlayer.RecovSlider += thisItem.RecovSlider; 
				} 
				else 
				{ 
					currPlayer.SlowMotion = thisItem.SlowMotion; 
					currPlayer.SpeedSlowMot = thisItem.SpeedSlowMot; 
					currPlayer.SpeedDeacSM = thisItem.SpeedDeacSM; 
					currPlayer.ReduceSlider = thisItem.ReduceSlider; 
					currPlayer.RecovSlider = thisItem.RecovSlider; 
				} 
			} 
		}

		if ( thisItem.ModifVie )
		{
            currPlayer.Life++;
            AllPlayerPrefs.AHeartUse = currPlayer.Life;
		}

		if ( thisItem.StartBonus )
		{
			SpawnerChunck.StartBonus = true;
			SpawnerChunck.EndLevel++;
            AllPlayerPrefs.AExtraStart = SpawnerChunck.EndLevel;
		}
	}
    #endregion
}

#region Save_Score
public static class SaveData
{
    public static void Save(ListData p_dataSave)
    {
        if (!Directory.Exists(Application.dataPath + "/Save"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Save");
        }
        string path1 = Application.dataPath + "/Save/save.bin";
        FileStream fSave = File.Create(path1);
        AllPlayerPrefs.saveData.listScore.SerializeTo(fSave);
        fSave.Close();
        //GameObject.Find("Trash_text").GetComponent<Text>().text = "save";
    }

    public static ListData Load()
    {
        if (!Directory.Exists(Application.dataPath + "/Save"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Save");
        }
        string path1 = Application.dataPath + "/Save/save.bin";
        ListData l = new ListData();
        if (File.Exists(path1))
        {
            FileStream fSave = File.Open(path1, FileMode.Open, FileAccess.ReadWrite);
            l.listScore = fSave.Deserialize<List<DataSave>>();
            fSave.Close();
            //GameObject.Find("Trash_text").GetComponent<Text>().text = l.listScore.Count > 0 ? "score = "+l.listScore[0].finalScore : "no save";
        }
        return l;
    }
}

[System.Serializable]
public class DataSave
{
    public DataSave(int p_fs, int p_s, int p_p, float p_d)
    {
        finalScore = p_fs;
        score = p_s;
        piece = p_p;
        distance = p_d;
    }

    public DataSave()
    {
        finalScore = 0;
        score = 0;
        piece = 0;
        distance = 0;
    }

    public int finalScore;
    public int score;
    public int piece;
    public float distance;
}

public class ListData
{
    public List<DataSave> listScore;
    public ListData()
    {
        listScore = new List<DataSave>();
    }
    public void Tri_Insert()
    {
        int verif;
        int i, j;
        DataSave verifSave;
        for (i = 1; i < listScore.Count; i++)
        {
            verif = listScore[i].finalScore;
            verifSave = listScore[i];
            for (j = i; j > 0 && listScore[j - 1].finalScore < verif; j--)
            {
                listScore[j] = listScore[j - 1];
            }
            listScore[j] = verifSave;
        }
    }
    public void Add(DataSave p_save)
    {
        if (listScore.Count <= 9)
        {
            listScore.Add(p_save);
            Tri_Insert();
        }
        else if (p_save.finalScore > listScore[listScore.Count - 1].finalScore)
        {
            listScore.Add(p_save);
            Tri_Insert();
            listScore.RemoveAt(listScore.Count - 1);
        }
        SaveData.Save(this);
    }
}

public static class StreamExtensions
{
    public static void SerializeTo<T>(this T o, Stream stream)
    {
        new BinaryFormatter().Serialize(stream, o);  // serialize o not typeof(T)
    }

    public static T Deserialize<T>(this Stream stream)
    {
        return (T)new BinaryFormatter().Deserialize(stream);
    }
}
#endregion

#region other
[System.Serializable]
public class FxList 
{
	public string FxName;
	public GameObject FxObj;
}


[System.Serializable] 
public class ChunkLock 
{ 
	public ChunksScriptable ThisChunk; 
	public List<NewChunk> AllUnlock; 
} 

[System.Serializable] 
public class NewChunk  
{ 
	public GameObject ThisChunk; 
	public UnLockMethode ThisMethod; 
} 

[System.Serializable] 
public class Rank  
{ 
	public string NameRank; 
	public int MultiPli = 1;
	public float Time;
    public Color Color;
	public int NeededScore; 
} 

[System.Serializable] 
public class ScoringInfo 
{ 
	public DeathType TypeDeath;
	public int Multiplicateur = 1;

	[Header ("Si il faut attendre pour cumuler du score")]
	public bool WaitCulmul;
	public float SecCumul = 0;

	[HideInInspector]
	public IEnumerator CurrWait;
	[HideInInspector]
	public int CurrCount = 0;
	[HideInInspector]
	public int AllScore = 0;
	[HideInInspector]
	public List<GameObject> CurrSpawn;
}
#endregion