using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using Rewired;

public class UiManager : ManagerParent
{
	#region Variables
	public Slider MotionSlider;
    public Slider Madness;
	public Image RankSlider;
	public Image RedScreen;
	public GameObject speedEffect;
	public Transform MenuParent;
	public Transform GameParent;
	public GameObject PatternBackground;
	public GameObject GlobalBack;
    public GameObject PostProcessGlobal;
	public GameObject PostProcessMadness;
	public GameObject GetHubDir;

    [Header("INTRO MAD")]
    public GameObject SwearWordsObject;
    public string[] SwearWordsName;
    public Tween SwearWordTw;

    public Image ArrowTuto;

	public string ScoreString;
	public Text ScorePoints;
	public Text MoneyPoints;
	public Text RankText;
	public Text Multiplicateur;

	public Image BallTransition;

    [Header("MAIN MENU")]
    public int MenuSelection = 1;
	public List<UpdateImage> ImageController;

    [Header("SHOP STUFF")]
    public Image SlowMotion;
    public Image BonusLife;
	public List<Image> ExtraHearts;
    public Sprite[] AbilitiesSprite;

    [Header("MISC GAMEFEEL")]
    public Image CircleFeel;
    public GameObject TextFeelMadness;
	[HideInInspector]
    public Camera thisCam;


    [Header("REWARDS")]
    public Image arrowLeft;
    public Image arrowRight;
    public CanvasGroup rewardsArrows;
    public CanvasGroup rewardsKeys;

    private Tween shopTw1, shopTw2, shopTw3, shopTw4, arrowLeftTw, arrowRightTw;

    public Tween madnessRedTw;

    Dictionary <MenuType, UiParent> AllMenu;
	public MenuType menuOpen;

    [HideInInspector]
    public bool OnMenu = false;
	GameObject InGame;
	Player inputPlayer;
	//bool onMainScene = true;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public void OpenThisMenu ( MenuType thisType, MenuTokenAbstract GetTok = null )
	{
		CheckContr ( );
		UiParent thisUi;
		//Debug.Log ( "open " + thisType );

		if ( AllMenu.TryGetValue ( thisType, out thisUi ) )
		{
			if ( menuOpen == thisType )
			{
				return;
			}
            
			InGame.SetActive ( false );
			if ( menuOpen != MenuType.Nothing )
			{
				CloseThisMenu ( true );
			}

            OnMenu = true;
			menuOpen = thisType;
			GlobalBack.SetActive ( true );
			thisUi.OpenThis ( GetTok );

            if ( thisType != MenuType.Title )
            {
                OpenShop();
            }

			GetHubDir.SetActive ( false );
		}
    }

	public void CloseThisMenu ( bool openNew = false )
	{
		GetHubDir.SetActive ( true );

		UiParent thisUi;

		if ( menuOpen != MenuType.Nothing && AllMenu.TryGetValue ( menuOpen, out thisUi ) )
		{

			InGame.SetActive ( true );
			GlobalBack.SetActive ( false );
			thisUi.CloseThis ( );
            if ( menuOpen != MenuType.Title )
            {
                CloseShop();
            }
			menuOpen = MenuType.Nothing;
            OnMenu = false;
		}
	}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            /*
            DOTween.To(() => GlobalManager.GameCont.currentValue, x => GlobalManager.GameCont.currentValue = x, 1.5f, .2f).OnComplete(() => {
                DOTween.To(() => GlobalManager.GameCont.currentValue, x => GlobalManager.GameCont.currentValue = x, 0, .1f);
            });

            DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 1f, .6f).OnComplete(() => {
                //DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 0, .12f);
            });*/

            DOTween.To(() => GlobalManager.GameCont.weightValue, x => GlobalManager.GameCont.weightValue = x, 1f, .6f).OnComplete(() => {
                //DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 0, .12f);
            });

			var volume = PostProcessManager.instance.QuickVolume ( PostProcessMadness.layer, 100f, GlobalManager.GameCont.postMadnessProfile.profile.settings.ToArray ( ) );
			volume.weight = 0f;

			DOTween.Sequence()
				.Append(DOTween.To(() => volume.weight, x => volume.weight = x, 1f, 1f))
				.AppendInterval(1f)
				.Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0f, 1f))
				.OnComplete(() =>
				{
					RuntimeUtilities.DestroyVolume(volume, true);
					Destroy(this);
				});
        }
    }

    public void SetCam ( Camera newCame )
	{
		thisCam = newCame;
	}

    public void MenuGlobal(int whichMenu)
    {
        if(whichMenu == 1)
        {
            GlobalManager.GameCont.Player.transform.DORotate(new Vector3(0, -60, 0), .5f, RotateMode.WorldAxisAdd);
        }

        if (whichMenu == 2)
        {
            GlobalManager.GameCont.Player.transform.DORotate(new Vector3(0, -30, 0), .5f, RotateMode.WorldAxisAdd);
        }

        if (whichMenu == 3)
        {
            GlobalManager.GameCont.Player.transform.DORotate(new Vector3(0, 0, 0), .5f, RotateMode.WorldAxisAdd);
        }

        if (whichMenu == 4)
        {
            GlobalManager.GameCont.Player.transform.DORotate(new Vector3(0, 30, 0), .5f, RotateMode.WorldAxisAdd);
        }

        MenuSelection = whichMenu;
    }

    public void Intro()
    {
        /*if ( GlobalManager.GameCont.LaunchTuto )
		{
			return;
        }*/
        SwearWordTw.Kill(true);

		if ( GlobalManager.GameCont.LaunchTuto )
		{
			thisCam.transform.DOLocalRotate(new Vector3(0, 0, -3.5f), 0);
			thisCam.GetComponent<RainbowRotate>().time = .4f;
			thisCam.GetComponent<RainbowMove>().time = .2f;

			thisCam.GetComponent<RainbowRotate>().enabled = true; 
			thisCam.GetComponent<RainbowMove>().enabled = true;

			return;
		}

        Time.timeScale = .05f;
		float saveFov = thisCam.fieldOfView;

        DOVirtual.DelayedCall(.35f, () => {
            Time.timeScale = .0f;
            DOVirtual.DelayedCall(.1f, () =>
            {
                Time.timeScale = 1f;
                GlobalManager.GameCont.Intro = false;

                thisCam.DOFieldOfView(4, .25f); 
			    thisCam.DOFieldOfView(100, .15f);

                DOVirtual.DelayedCall(1f, () =>
                {
                    GlobalManager.GameCont.introFinished = true;
                });

                DOVirtual.DelayedCall(.25f, () => { 
                    thisCam.transform.DOKill(true);
                    thisCam.GetComponent<RainbowMove>().DOKill(true);
                    thisCam.GetComponent<RainbowMove>().enabled = false;
                    thisCam.GetComponent<RainbowMove>().DOKill(true);
                    thisCam.GetComponent<RainbowRotate>().DOKill(true);
                    thisCam.GetComponent<RainbowRotate>().enabled = false;
                    thisCam.GetComponent<RainbowRotate>().DOKill(true);
                    thisCam.GetComponent<RainbowRotate>().time = .4f;
                    thisCam.GetComponent<RainbowMove>().time = .2f;
                    thisCam.transform.DOLocalRotate(new Vector3(0, 0, -3.5f), 0);

                    DOVirtual.DelayedCall(.75f,()=>{

                        thisCam.transform.DOLocalRotate(new Vector3(0, 0, -3.5f), 0);
                        thisCam.GetComponent<RainbowRotate>().enabled = true; thisCam.GetComponent<RainbowMove>().enabled = true;
                        // thisCam.GetComponent<RainbowRotate>().reStart();
                    });

                    DOVirtual.DelayedCall(2f, () =>
                    {
                        thisCam.DOFieldOfView(saveFov, .25f).OnComplete(()=> {
                        });
                    });
                });
            });
        });
    }

    public void IntroRestart()
    {
        Time.timeScale = .05f;

        DOVirtual.DelayedCall(.35f, () => {
            Time.timeScale = .0f;
            DOVirtual.DelayedCall(.1f, () =>
            {
                Time.timeScale = 1f;
                GlobalManager.GameCont.Intro = false;
                DOVirtual.DelayedCall(1f, () =>
                {
                    GlobalManager.GameCont.introFinished = true;
                });
                {
                    thisCam.transform.DOKill(true);
                    thisCam.GetComponent<RainbowMove>().DOKill(true);
                    thisCam.GetComponent<RainbowMove>().enabled = false;
                    thisCam.GetComponent<RainbowMove>().DOKill(true);
                    thisCam.GetComponent<RainbowRotate>().DOKill(true);
                    thisCam.GetComponent<RainbowRotate>().enabled = false;
                    thisCam.GetComponent<RainbowRotate>().DOKill(true);
                    thisCam.GetComponent<RainbowRotate>().time = .4f;
                    thisCam.GetComponent<RainbowMove>().time = .2f;
                    thisCam.transform.DOLocalRotate(new Vector3(0, 0, -3.5f), 0);
                    DOVirtual.DelayedCall(.75f, () =>
                    {
                        thisCam.GetComponent<RainbowRotate>().enabled = true; thisCam.GetComponent<RainbowMove>().enabled = true;
                    });
                }
            });
        });
    }

    public void OpenRewards()
    {

        Debug.Log("OpenRewards");


        rewardsKeys.DOFade(1, .1f);
        rewardsArrows.DOFade(1, .1f);
        /*

        float arrowLeftpos = arrowLeft.transform.localPosition.x;
        float arrowRightpos = arrowRight.transform.localPosition.x;

        
        arrowLeft.DOKill(true);

        arrowLeftTw = arrowLeft.transform.DOLocalMoveX(arrowLeft.transform.localPosition.x - 50, .9f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(.2f, () =>
             {
                 //arrowLeft.DOFade(0, .4f).OnComplete(() =>
                 //{
                     arrowLeft.transform.DOLocalMoveX(arrowLeftpos, 0f);
//                     arrowLeft.DOFade(1, 0);

                 //});
             });
        }).SetLoops(-1, LoopType.Restart);
        */
    }

    public void CloseRewards()
    {
        //arrowLeftTw.Kill(true);

        Debug.Log("CloseRewards");
        
        rewardsKeys.DOFade(0, .1f);
        rewardsArrows.DOFade(0, .1f);
    }

    public void DoubleCoup()
    {
		float saveFov = thisCam.fieldOfView;

        thisCam.DOFieldOfView(40, .1f).OnComplete(() => {
			thisCam.DOFieldOfView(saveFov, .15f);
        });
    }

	public void BloodHit()
	{
        VibrationManager.Singleton.CoupSimpleVibration();

        Time.timeScale = 0.0f;
        //fixedDeltaTime = 0.02F * Time.timeScale;
        DOVirtual.DelayedCall(.05f, () => {
			Time.timeScale = 1;
            //Time.fixedDeltaTime = .02F;
        });

		float saveFov = thisCam.fieldOfView;
		thisCam.DOFieldOfView(25.5f, .16f);//.SetEase(Ease.InBounce);
        RedScreen.DOColor(new Color32 (0xCA, 0x23, 0x23, 0x21),0);
		RedScreen.DOFade(.4f, .16f).OnComplete(() => {
			RedScreen.DOFade(0, .12f);
			thisCam.DOFieldOfView(saveFov, .08f);//.SetEase(Ease.InBounce);
		});
	}

    public void BloodHitDash()
    {
        //Debug.Log("HitDash");
        //Time.timeScale = 0.0f;
        //fixedDeltaTime = 0.02F * Time.timeScale;
        VibrationManager.Singleton.CoupSimpleVibration();

        DOVirtual.DelayedCall(.4f, () => {
            Time.timeScale = 1;
            //Time.fixedDeltaTime = .02F;
        });

        float saveFov = thisCam.fieldOfView;
        thisCam.DOFieldOfView(27f, .1f);//.SetEase(Ease.InBounce);
        RedScreen.DOFade(.4f, .1f).OnComplete(() => {
            RedScreen.DOFade(0, .08f);
            thisCam.DOFieldOfView(saveFov, .08f);//.SetEase(Ease.InBounce);
        });
    }

    public void GameOver()
    {
		GlobalManager.GameCont.soundFootSteps.Kill ( );
        //Debug.Log("ShakeOver");
        MadnessGreenEnd();
        MadnessRedEnd();

        VibrationManager.Singleton.GameOverVibration();
        //Time.timeScale = 0f;
        //Time.fixedDeltaTime = 0.02F * Time.timeScale;
        //DOVirtual.DelayedCall(.4f, () => {
        Time.timeScale = 1;
            Time.fixedDeltaTime = .02F;
            ScreenShake.Singleton.ShakeGameOver();
        //});
        RedScreen.DOKill ( );
        RedScreen.DOFade(.7f, .25f).OnComplete(() => {
            RedScreen.DOFade(0, .0f);
            MadnessRedEnd();
            MadnessGreenEnd();

			RedScreen.GetComponents<RainbowColor>()[1].enabled = false;
			RedScreen.GetComponents<RainbowColor>()[0].enabled = false;

        });

        int rdmValue = UnityEngine.Random.Range(0, 4);
        GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Death_" + rdmValue, false);

        RankSlider.transform.parent.GetComponent<CanvasGroup>().DOFade(0,0.1f);
    }

    public void IntroWord()
    {
        //DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 1f, .6f);
      

        SwearWordTw = DOVirtual.DelayedCall(.1f, () =>
        {

            var word = Instantiate(SwearWordsObject, transform.position, Quaternion.identity, transform.transform.GetChild(0).transform.GetChild(0));
            word.transform.localScale = Vector3.one;
            float randomX = UnityEngine.Random.Range(-900, 900);
            float randomY = UnityEngine.Random.Range(-450, 450);
            float randomRotate = UnityEngine.Random.Range(-25, 25);
            int randomSize = UnityEngine.Random.Range(45, 70);
            int randomWord = UnityEngine.Random.Range(0, SwearWordsName.Length);
            Vector2 tmpPos = word.transform.localPosition;
            tmpPos.x = randomX;
            tmpPos.y = randomY;
            word.transform.localPosition = tmpPos;
            word.transform.DORotate(new Vector3(0, 0, randomRotate), 0);
            word.GetComponent<Text>().fontSize = randomSize;
            word.GetComponent<Text>().text = SwearWordsName[randomWord];

            word.transform.DOScale(3, 0);
            word.transform.DOScale(1, .15f);
            word.GetComponent<Text>().DOFade(1, 0.15f);
            word.GetComponent<RainbowColor>().enabled = true;
            word.transform.SetAsFirstSibling();

            Destroy(word, .7f);
        }).SetLoops(-1, LoopType.Restart);



    }


    public void OpenMadness()
    {
        AllPlayerPrefs.ANbPassageMadness++;
        VibrationManager.Singleton.FleshBallVibration();

        thisCam.GetComponent<CameraFilterPack_Distortion_Dream2>().enabled = true;
        //thisCam.GetComponent<CameraFilterPack_Color_YUV>().enabled = true;

        PostProcessMadness.GetComponent<PostProcessVolume>().enabled = true;

        Transform getPlayer = GlobalManager.GameCont.Player.transform;
		GameObject textMadness = GlobalManager.GameCont.FxInstanciate ( getPlayer.position + getPlayer.forward * 10, "TextMadness", transform, 10f );
        textMadness.transform.DORotate(new Vector3(0, GlobalManager.GameCont.Player.transform.rotation.y, 0), 0, RotateMode.WorldAxisAdd);


        Destroy(textMadness, 3);


		DOTween.Kill ( GlobalManager.GameCont.chromValue );
		DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 1f, .6f);

        int rdmValue = UnityEngine.Random.Range(0, 3);
        GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Madness_" + rdmValue, false);

		closeMad = false;
		StartCoroutine ( waitMad ( ) );

        //textMad.GetComponentInChildren<TextMesh>().text = 
        //thisCam.transform.GetComponent<RainbowMove>().enabled = false;

        //thisCam.transform.DOKill(false);

        /*
        thisCam.transform.DOLocalMoveY(0, .3f).OnComplete(() => {
            DOVirtual.DelayedCall(.65f,()=>{
                thisCam.transform.DOLocalMoveY(.9f, .1f);
            });
        }).SetLoops(-1,LoopType.Yoyo);*/

        /*
        thisCam.DOFieldOfView(40, .35f).OnComplete(() => {
            thisCam.DOFieldOfView(60, .35f);
        }).SetLoops(-1,LoopType.Yoyo);
        */
    }

	bool closeMad = false;
	IEnumerator waitMad () 
	{
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );

		var volume = PostProcessManager.instance.QuickVolume(PostProcessMadness.layer, 100f, GlobalManager.GameCont.postMadnessProfile.profile.settings.ToArray());
		volume.weight = 0f;

		DOTween.To ( ( ) => volume.weight, x => volume.weight = x, 1f, .6f );

		while ( !closeMad )
		{
			yield return thisF;
		}

		DOTween.To ( ( ) => volume.weight, x => volume.weight = x, 0, .3f ).OnComplete ( ( ) =>
		{
			PostProcessMadness.GetComponent<PostProcessVolume>().enabled = false;
		} );
	}

    public void CloseMadness()
    {
		DOTween.To ( ( ) => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 0, 0.25f ).OnComplete ( ( ) =>
		{
			thisCam.GetComponent<CameraFilterPack_Distortion_Dream2>().enabled = false;
			thisCam.transform.DORotate(new Vector3(0, 0, 3), 0f, RotateMode.LocalAxisAdd);
			closeMad = true;
		} );


        //var volume = PostProcessManager.instance.QuickVolume(PostProcessMadness.layer, 100f, GlobalManager.GameCont.postMadnessProfile.profile.settings.ToArray());
	
        //thisCam.GetComponent<CameraFilterPack_Color_YUV>().enabled = false;

        //thisCam.GetComponent<RainbowRotate>().enabled = false;



        //thisCam.DOKill(true);

       // Debug.Log("CloseMad");
        //thisCam.GetComponent<RainbowRotate>().enabled = true;

       //thisCam.transform.GetComponent<RainbowMove>().enabled = true;
    }

	public void DashSpeedEffect ( bool enable )
	{
		if ( speedEffect == null )
		{
			return;
		}

		if ( enable )
		{
			speedEffect.GetComponent<CanvasGroup>().DOFade(1, .25f); 
		}
		else
		{
			speedEffect.GetComponent<CanvasGroup>().DOFade(0, .10f);
            //Debug.Log("DashStop");
		}
	}

    public void TakeCoin()
    {
        MoneyPoints.transform.DOScale(1.5f, .1f).SetEase(Ease.InBounce).OnComplete(() => {
            MoneyPoints.transform.DOScale(1f, .05f).SetEase(Ease.InBounce);
        });


        int rdmValue = UnityEngine.Random.Range(0, 4);
        GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Money_" + rdmValue, false, null, true);
    }

    public void MadnessRedStart()
    {

        //Debug.Log("MadnessRed Start");
        madnessRedTw = null;/*
        madnessRedTw = RedScreen.DOFade(.3f, .2f).OnComplete(() => {
            RedScreen.DOFade(0, .2f);
        }).SetLoops(-1,LoopType.Restart);*/

        RedScreen.GetComponents<RainbowColor>()[0].enabled = true;

    }

    public void MadnessRedEnd()
    {

        //Debug.Log("MadnessRed Stop");

        /*
        madnessRedTw.Kill(true);
        RedScreen.DOFade(0f, .05f);*/

        RedScreen.GetComponents<RainbowColor>()[0].enabled = false;
        RedScreen.DOColor(new Color32(0xff, 0xff, 0xff, 0x00), 0);
    }

    public void MadnessGreenStart()
    {

        //Debug.Log("MadnessRed Start");
        madnessRedTw = null;/*
        madnessRedTw = RedScreen.DOFade(.3f, .2f).OnComplete(() => {
            RedScreen.DOFade(0, .2f);
        }).SetLoops(-1,LoopType.Restart);*/

        RedScreen.GetComponents<RainbowColor>()[1].enabled = true;

    }

    public void MadnessGreenEnd()
    {

        RedScreen.GetComponents<RainbowColor>()[1].enabled = false;
        RedScreen.DOColor(new Color32(0xff, 0xff, 0xff, 0x00), 0);
    }

	string NbrString ( string getText )
	{
		string getNew = string.Empty;
		int count = 0;
		int a;
		for  ( a = getText.Length - 1; a >= 0; a -- )
		{
			if ( count > 0 && count % 3 == 0 )
			{
				getNew += " ";
			}
			count++;
			getNew += getText [ a ];
		}

		getText = string.Empty;

		for  ( a = getNew.Length - 1; a >= 0; a -- )
		{
			getText += getNew [ a ];
		}

		return getText;
	}

    public void UpdateScore ( )
    {
        ScorePoints.text = NbrString(ScoreString);
    }

    public void ScorePlus(int number, Color rankColor, int currIndex, string type)
    {

        float randomPos = UnityEngine.Random.Range(-600, 600);
        float randomRot = UnityEngine.Random.Range(200, 200);
        //Debug.Log("score");
        
        var scoreInt = (int.Parse(ScoreString) + number);
        ScoreString = "" + scoreInt;
        ScorePoints.text = NbrString(ScoreString);

        StaticRewardTarget.SScoreLV = scoreInt;
		AllPlayerPrefs.finalScore += number;
        AllPlayerPrefs.scoreWhithoutDistance += number;

        Text scoretxt = GlobalManager.GameCont.FxInstanciate(new Vector2(randomPos, randomRot), "TextScore", InGame.transform, 4f).GetComponent<Text>();
        scoretxt.text = "+ " + number + "\n" + type;
        scoretxt.transform.localPosition = new Vector2(randomPos, randomRot);

        if (currIndex == 6)
        {
            scoretxt.GetComponentsInChildren<RainbowColor>()[1].enabled = true;
            scoretxt.GetComponentsInChildren<RainbowColor>()[0].enabled = false;
        } else
        {

            scoretxt.GetComponent<Text>().color = rankColor;
        }


        scoretxt.transform.DOScale(1.4f, 0);
        scoretxt.transform.DOScale(.55f, .1f).OnComplete(() => {
            scoretxt.transform.DOPunchScale((Vector3.one * .45f), .25f, 15, 1).OnComplete(() => {
                scoretxt.transform.DOScale(0, .5f);
                scoretxt.transform.DOLocalMove(ScorePoints.transform.gameObject.transform.localPosition, .5f);
            });
        });

        Destroy(scoretxt.gameObject, 4);
    }

    public void NewRank(int currIndex)
    {
        Transform getRank = GlobalManager.Ui.RankSlider.transform.parent; 

        getRank.DOKill(true);
        //Vector2 localPos = getRank.localPosition;
        getRank.DOLocalMove(new Vector2(-120, -450), 0);
        getRank.DOPunchPosition(Vector2.one * 30f, 1f, 18, 1).OnComplete(() => {
            getRank.DOLocalMove(new Vector2(-833, -200), .3f).OnComplete(()=> {

                //GlobalManager.Ui.Multiplicateur.transform.parent.DOShakePosition(.05f, 30f, 12, 360).SetLoops(-1, LoopType.Restart);
                //GlobalManager.Ui.Multiplicateur.transform.parent.DOPunchPosition(Vector2.one * 90f, .4f, 18, .3f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            });
        });

        DOVirtual.DelayedCall(.2f, () => {

            if(currIndex == 6)
            {
                getRank.GetChild(0).GetComponentsInChildren<RainbowColor>()[0].enabled = false;
                getRank.GetChild(0).GetComponentsInChildren<RainbowColor>()[1].enabled = true;

                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[0].enabled = false;
                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[2].enabled = false;
                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[1].enabled = true;
                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[3].enabled = true;
            }
            else
            {
                getRank.GetChild(0).GetComponentsInChildren<RainbowColor>()[0].enabled = true;
                getRank.GetChild(0).GetComponentsInChildren<RainbowColor>()[1].enabled = false;

                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[0].enabled = true;
                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[2].enabled = true;
                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[1].enabled = false;
                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[3].enabled = false;

                getRank.GetChild(0).GetComponent<RainbowColor>().colors[1] = GlobalManager.GameCont.AllRank[currIndex].Color;
                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[0].colors[1] = GlobalManager.GameCont.AllRank[currIndex].Color;
                getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[1].colors[1] = GlobalManager.GameCont.AllRank[currIndex].Color;
            }
        });
        // getRank.GetChild(3).GetComponentsInChildren<RainbowColor>()[0].colors[2] = GlobalManager.GameCont.AllRank[currIndex].Color;


        //getRank.DOPunchPosition(new Vector3(30, 0, 0), 0.4f);

        getRank.GetComponent<CanvasGroup>().DOFade(0, 0);
        getRank.GetComponent<CanvasGroup>().DOFade(1, .15f);

        getRank.DOScale(5, 0);
        getRank.DOScale(1, .5f);


    }

    public void StartSpecialAction(string type)
    {
        if (type == "SlowMot")
        {
            SlowMotion.sprite = AbilitiesSprite[0];
            //float intensityBloom = 0;
            //DOTween.To(() => intensityBloom, x => intensityBloom = x, 1, 1);

            DOTween.To(() => GlobalManager.GameCont.currentValue, x => GlobalManager.GameCont.currentValue = x, .3f, .25f).OnComplete(() => {
                DOTween.To(() => GlobalManager.GameCont.currentValue, x => GlobalManager.GameCont.currentValue = x, 0, .12f);
            });

            DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 1f, .6f).OnComplete(() => {
                DOTween.To(() => GlobalManager.GameCont.chromValue, x => GlobalManager.GameCont.chromValue = x, 0, .4f);
            });

            //PostProcessGlobal.GetComponent<Bloom>().intensity.value = intensityBloom;
        }

        if (type == "OndeChoc")
            SlowMotion.sprite = AbilitiesSprite[1];

        if (type == "DeadBall")
            SlowMotion.sprite = AbilitiesSprite[2];

        SlowMotion.transform.DOLocalMove(new Vector2(930, -510), .05f);
        CircleFeel.transform.DOScale(1, 0);
        CircleFeel.DOColor(Color.white, 0);
        SlowMotion.DOFade(0, .05f);
        DOVirtual.DelayedCall(.1f, () => {
            SlowMotion.DOFade(.75f, .1f);
            SlowMotion.transform.DOScale(4, 0f);
            CircleFeel.transform.DOScale(25, .25f);
            CircleFeel.DOFade(.75f, .15f).OnComplete(() => {
                CircleFeel.DOFade(0, .1f);
            });
            SlowMotion.transform.DOPunchPosition(Vector3.one * 30f, .15f, 18, 1).OnComplete(()=> {
                SlowMotion.transform.DOLocalMove(new Vector2(0, 0), .05f);
                SlowMotion.DOFade(0, .05f);
                DOVirtual.DelayedCall(.2f, () =>
                {
                    SlowMotion.DOFade(1, .15f);
                    SlowMotion.transform.DOScale(1, 0f);
                });
            });
        });
    }

    public void SelectShop()
    {

        shopTw1.Kill(true);
        shopTw2.Kill(true);
        shopTw3.Kill(true);
        shopTw4.Kill(true);


        shopTw1 = SlowMotion.transform.DOLocalMove(new Vector2(930, -510), .05f);
        shopTw2 = SlowMotion.DOFade(0, .05f);
        shopTw3 = DOVirtual.DelayedCall(.1f, () => {
            shopTw2 = SlowMotion.DOFade(1f, .1f);
            SlowMotion.transform.DOScale(4, 0f);
            shopTw4 = SlowMotion.transform.DOPunchPosition(Vector3.one * 30f, .6f, 18, 1).OnComplete(() => {
                shopTw1 = SlowMotion.transform.DOLocalMove(new Vector2(0, 0), .2f);
                shopTw2 = SlowMotion.DOFade(0, .05f);
                shopTw3 = DOVirtual.DelayedCall(.15f, () =>
                {
                    shopTw1 = SlowMotion.DOFade(1, .1f);
                    shopTw2 = SlowMotion.transform.DOScale(1, 0f);
                });
            });
        });
    }

	public void OpenShop()
    {
        thisCam.DOFieldOfView(10, .05f);
        //GlobalManager.GameCont.Player.GetComponent<PlayerController>().
    }

    public void CloseShop()
    {
        thisCam.DOFieldOfView(60, .3f);
    }

    public void HeartShop(int number)
    {
		Image getImage = ExtraHearts [ number ];
		Transform getImgTrans = getImage.transform;

		getImgTrans.DOLocalMove(new Vector2(930, -510), .05f);
        DOVirtual.DelayedCall(.1f, () => {
			getImage.DOFade(1f, .1f);
			getImage.GetComponent<RainbowScale>().enabled = false;
            getImgTrans.DOScale(4, 0f);
			getImgTrans.DOPunchPosition(Vector3.one * 30f, .6f, 18, 1).OnComplete(() => {
				getImgTrans.DOLocalMove(new Vector2(75 * (number + 1), 0), .2f);
				getImage.DOFade(0, .05f);
                DOVirtual.DelayedCall(.15f, () =>
                {
					getImage.DOFade(1, .1f);
					getImgTrans.DOScale(1, 0f);
					getImage.GetComponent<RainbowScale>().enabled = true;
                });
            });
        });
    }

	public void NewLife ( int currLife )
	{
		Image getCurrHeat;
		if ( ExtraHearts [ 0 ].enabled == false )
		{
			getCurrHeat = ExtraHearts [ 0 ];
			getCurrHeat.transform.localPosition = new Vector3 (105, -45,0);
		}
		else if ( ExtraHearts [ 1 ].enabled == false )
		{
			getCurrHeat = ExtraHearts [ 1 ];
			getCurrHeat.transform.localPosition = new Vector3 (195,-45f);
		}
		else
		{
			return;
		}

		getCurrHeat.enabled = true;
		getCurrHeat.DOKill ( );
		getCurrHeat.transform.localScale = new Vector3 ( 1, 1, 1 );
		CircleFeel.DOFade ( 0, 0 );
		getCurrHeat.DOFade(1, .5f);
		getCurrHeat.GetComponent<RainbowScale>().enabled = true;
	}

    public void StartBonusLife ( int currLife )
    {
		Image getCurrHeat;
		if ( currLife == 3 )
		{
			getCurrHeat = ExtraHearts [ 1 ];
		}
		else if ( currLife == 2 )
		{
			getCurrHeat = ExtraHearts [ 0 ];
		}
		else
		{
			getCurrHeat = BonusLife;
		}

		getCurrHeat.DOKill ( );

        getCurrHeat.GetComponent<RainbowScale>().enabled = false;

        CircleFeel.transform.DOScale(1, 0);
        CircleFeel.DOColor(new Color32(0xf4,0x6c,0x6e,0xff),0);
		
        getCurrHeat.transform.DOLocalMove(new Vector2(780, -480), .05f);
		getCurrHeat.DOFade(0, .05f);
        DOVirtual.DelayedCall(.15f, () => {
			getCurrHeat.DOFade(.75f, .1f);
			getCurrHeat.transform.DOScale(10, 0f);
			getCurrHeat.transform.DOPunchPosition(Vector3.one * 20f, .7f, 18, 1).OnComplete(() => {
				CircleFeel.transform.DOScale(28, .8f).OnComplete(() => {
					getCurrHeat.enabled = false;
				});
                CircleFeel.DOFade(1, .2f).OnComplete(() => {
                    CircleFeel.DOFade(0, .4f);
                });
				getCurrHeat.transform.DOScale(40f, .5f);
				getCurrHeat.DOFade(0, .5f);
            });
        });
    }


	public void CheckContr ( )
	{
		Controller controller = inputPlayer.controllers.GetLastActiveController();
		if ( controller != null )
		{
			UpdateImage [] getImg = ImageController.ToArray ( );
			switch ( controller.type )
			{
			case ControllerType.Keyboard:
			case ControllerType.Mouse:
				for ( int a = 0; a < getImg.Length; a++ )
				{
					getImg [ a ].ThisImage.sprite = getImg [ a ].SpriteMouse;
				}
				break;
			case ControllerType.Joystick:
				for ( int a = 0; a < getImg.Length; a++ )
				{
					getImg [ a ].ThisImage.sprite = getImg [ a ].SpriteJoyst;
				}
				break;
			}
		}
	}
	#endregion

	#region Private Methods
	protected override void InitializeManager ( )
	{
		InieUI ( );
        ScoreString = "0";
		inputPlayer = ReInput.players.GetPlayer(0);
		thisCam = GlobalManager.GameCont.thisCam;
		Object[] getAllMenu = Resources.LoadAll ( "Menu" );
		Dictionary<MenuType, UiParent> setAllMenu = new Dictionary<MenuType, UiParent> ( getAllMenu.Length );

		GameObject thisMenu;
		UiParent thisUi;

		for ( int a = 0; a < getAllMenu.Length; a++ )
		{
			thisMenu = ( GameObject ) Instantiate ( getAllMenu [ a ], MenuParent );
			thisUi = thisMenu.GetComponent<UiParent> ( );
			thisUi.Initialize ( );
			setAllMenu.Add ( thisUi.ThisMenu, thisUi );
		}

		AllMenu = setAllMenu;

		InGame = transform.Find ( "Canvas/InGame" ).gameObject;
		GlobalManager.GameCont.IniFromUI ( );
		//GlobalManager.GameCont.StartGame ( );
	}

	public void InieUI ( )
	{
        //	InvokeRepeating ( "checkCurosr", 0, 0.5f );

		/*System.Action <HomeEvent> checkLevel = delegate ( HomeEvent thisEvnt )
		{
			onMainScene = thisEvnt.onMenuHome;
		};*/

		//GlobalManager.Event.Register ( checkLevel );

        MoneyPoints.text = "" + AllPlayerPrefs.GetIntValue(Constants.Coin);

        if ( PatternBackground != null )
		{
            PatternBackground.transform.DOLocalMoveY(1092, 0).SetEase(Ease.Linear);

            PatternBackground.transform.DOLocalMoveY(-60, 5f).SetEase(Ease.Linear).OnComplete(() => {
				PatternBackground.transform.DOLocalMoveY(1092, 0).SetEase(Ease.Linear);
			}).SetLoops(-1, LoopType.Restart);
		}
	}
    
	void checkCurosr ( )
	{
		if ( menuOpen != MenuType.Nothing )
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
	#endregion
}


[System.Serializable]
public class UpdateImage 
{
	public Image ThisImage;
	public Sprite SpriteMouse;
	public Sprite SpriteJoyst;
}
