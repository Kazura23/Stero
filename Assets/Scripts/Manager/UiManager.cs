﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.EventSystems;

public class UiManager : ManagerParent
{
	#region Variables
	public Slider MotionSlider;
    public Slider Madness;
	public Image RedScreen;
	public GameObject speedEffect;
	public Transform MenuParent;
	public GameObject PatternBackground;
	public GameObject GlobalBack;

	public Text ScorePoints;
	public Text MoneyPoints;

	public Image BallTransition;
	public Image TimerFight;

    [Header("MAIN MENU")]
    public int MenuSelection = 1;

    [Header("SHOP STUFF")]
    public Image SlowMotion;
    public Image BonusLife;
	public List<Image> ExtraHearts;
    public Sprite[] AbilitiesSprite;

    [Header("MISC GAMEFEEL")]
    public Image CircleFeel;
    public GameObject TextFeelMadness;
    private Camera thisCam;

    private Tween shopTw1, shopTw2, shopTw3, shopTw4;

    Dictionary <MenuType, UiParent> AllMenu;
	public MenuType menuOpen;

	GameObject InGame;
	//bool onMainScene = true;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public void OpenThisMenu ( MenuType thisType, MenuTokenAbstract GetTok = null )
	{
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

			menuOpen = thisType;
			GlobalBack.SetActive ( true );
			thisUi.OpenThis ( GetTok );
            OpenShop();
		}
	}

	public void CloseThisMenu ( bool openNew = false )
	{

		UiParent thisUi;

		if ( menuOpen != MenuType.Nothing && AllMenu.TryGetValue ( menuOpen, out thisUi ) )
		{

			InGame.SetActive ( true );
			GlobalBack.SetActive ( false );
			thisUi.CloseThis ( );
			menuOpen = MenuType.Nothing;
            CloseShop();
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
        Time.timeScale = .05f;
		float saveFov = thisCam.fieldOfView;

        DOVirtual.DelayedCall(.35f, () => {
            Time.timeScale = .0f;
            DOVirtual.DelayedCall(.1f, () =>
            {
                Time.timeScale = 1f;
                GlobalManager.GameCont.Intro = false;
				thisCam.DOFieldOfView(4, .25f);
                DOVirtual.DelayedCall(1f, () =>
                {
                    GlobalManager.GameCont.introFinished = true;
                });
				thisCam.DOFieldOfView(100, .15f);

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

    public void DoubleCoup()
    {
		float saveFov = thisCam.fieldOfView;

        thisCam.DOFieldOfView(40, .1f).OnComplete(() => {
			thisCam.DOFieldOfView(saveFov, .15f);
        });
    }

	public void BloodHit()
	{
		Time.timeScale = 0.0f;
        //fixedDeltaTime = 0.02F * Time.timeScale;
        DOVirtual.DelayedCall(.05f, () => {
			Time.timeScale = 1;
            //Time.fixedDeltaTime = .02F;
        });

		float saveFov = thisCam.fieldOfView;
		thisCam.DOFieldOfView(25.5f, .16f);//.SetEase(Ease.InBounce);
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
        //Debug.Log("ShakeOver");

        //Time.timeScale = 0f;
        //Time.fixedDeltaTime = 0.02F * Time.timeScale;
        //DOVirtual.DelayedCall(.4f, () => {
            Time.timeScale = 1;
            Time.fixedDeltaTime = .02F;
            ScreenShake.Singleton.ShakeGameOver();
        //});
        RedScreen.DOFade(.7f, .25f).OnComplete(() => {
            RedScreen.DOFade(0, .0f);
        });

        int rdmValue = UnityEngine.Random.Range(0, 4);
        GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Death_" + rdmValue, false);
    }

    public void OpenMadness()
    {
        thisCam.GetComponent<CameraFilterPack_Distortion_Dream2>().enabled = true;
        thisCam.GetComponent<CameraFilterPack_Color_YUV>().enabled = true;

        Vector3 tmpPos = GlobalManager.GameCont.Player.transform.position;
        GameObject textMadness;
        textMadness = GlobalManager.GameCont.FxInstanciate(new Vector3(tmpPos.x, tmpPos.y, tmpPos.z + 10), "TextMadness", transform, 10f);

        Destroy(textMadness, 3);

        int rdmValue = UnityEngine.Random.Range(0, 3);
        GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Madness_" + rdmValue, false);
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

    public void CloseMadness()
    {
		
        thisCam.GetComponent<CameraFilterPack_Distortion_Dream2>().enabled = false;
        thisCam.GetComponent<CameraFilterPack_Color_YUV>().enabled = false;

        //thisCam.GetComponent<RainbowRotate>().enabled = false;
        

        //thisCam.DOKill(true);

		thisCam.transform.DORotate(new Vector3(0, 0, 3), 0f, RotateMode.LocalAxisAdd);
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

	public void StartSpecialAction(string type)
    {
        if (type == "SlowMot")
            SlowMotion.sprite = AbilitiesSprite[0];

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
        ExtraHearts[number].transform.DOLocalMove(new Vector2(930, -510), .05f);
        DOVirtual.DelayedCall(.1f, () => {
            ExtraHearts[number].DOFade(1f, .1f);
            ExtraHearts[number].GetComponent<RainbowScale>().enabled = false;
            ExtraHearts[number].transform.DOScale(4, 0f);
            ExtraHearts[number].transform.DOPunchPosition(Vector3.one * 30f, .6f, 18, 1).OnComplete(() => {
                ExtraHearts[number].transform.DOLocalMove(new Vector2(75 * (number + 1), 0), .2f);
                ExtraHearts[number].DOFade(0, .05f);
                DOVirtual.DelayedCall(.15f, () =>
                {
                    ExtraHearts[number].DOFade(1, .1f);
                    ExtraHearts[number].transform.DOScale(1, 0f);
                    ExtraHearts[number].GetComponent<RainbowScale>().enabled = true;
                });
            });
        });
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

        getCurrHeat.GetComponent<RainbowScale>().enabled = false;

        CircleFeel.transform.DOScale(1, 0);
        CircleFeel.DOColor(new Color32(0xf4,0x6c,0x6e,0xff),0);

        getCurrHeat.transform.DOLocalMove(new Vector2(960, -480), .05f);
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
	#endregion

	#region Private Methods
	protected override void InitializeManager ( )
	{
		InieUI ( );

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


		if ( GlobalManager.GameCont.Player != null )
		{
			GlobalManager.GameCont.Player.GetComponent<PlayerController> ( ).InitPlayer ( );
		}
		//GlobalManager.GameCont.StartGame ( );
	}

	void InieUI ( )
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
