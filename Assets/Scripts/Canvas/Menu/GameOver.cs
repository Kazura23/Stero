﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Rewired;

public class GameOver : UiParent 
{
	#region Variables
	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.GameOver;
		}
	}

	public GameObject PatternGameOver, BarGameOver;
	public Text YouGameOver, MadeGameOver, PointsGameOver, PressPunch, PressSteroland, Highscore, newScore, quoteScore;
	float TimeFade = 0;
	bool canUpdate = false;
	Player inputPlayer;
	#endregion

	#region Mono
	void Start ( )
	{
		inputPlayer = ReInput.players.GetPlayer(0);
	}

	void Update ( )
	{
		if ( !canUpdate )
		{
			return;
		}

		if ( inputPlayer.GetAxis("CoupSimple") != 0 )
        {
            AllPlayerPrefs.relance = true;
            GlobalManager.GameCont.Restart();

		}
		else if (inputPlayer.GetAxis("CoupDouble") == 1 || Input.GetKeyDown(KeyCode.Escape) )
        {
            AllPlayerPrefs.relance = false;
            
            GlobalManager.GameCont.Restart();
        }
	}
	#endregion

	#region Public Methods
	public override void OpenThis ( MenuTokenAbstract GetTok = null )
	{
		base.OpenThis ( GetTok );

        AllPlayerPrefs.saveData.Add(AllPlayerPrefs.NewData());
		canUpdate = true;
        //float distPlayer = GlobalManager.GameCont.Player.GetComponent<PlayerController>().totalDis;

      //  Highscore.text = "" + AllPlayerPrefs.saveData.listScore[0].finalScore;

		var e = new RenableAbstObj ( );
		e.Raise ( );

		System.Action <DeadBallEvent> checkDBE = delegate ( DeadBallEvent thisEvnt )
		{
		};
		System.Action <RenableAbstObj> checkEnable = delegate ( RenableAbstObj thisEvnt ) 
		{ 
		}; 
		System.Action <DeadBallParent> checkDBP = delegate ( DeadBallParent thisEvnt ) 
		{ 
		}; 

		GlobalManager.Event.UnRegister ( checkDBP );
		GlobalManager.Event.UnRegister ( checkDBE );
		GlobalManager.Event.UnRegister ( checkEnable );

		GlobalManager.Ui.ExtraHearts [ 0 ].enabled = false; 
		GlobalManager.Ui.ExtraHearts [ 1 ].enabled = false; 

		GameOverTok thisTok = GetTok as GameOverTok;
        GetComponent<CanvasGroup>().DOFade(0, 0);
		PointsGameOver.text = Mathf.RoundToInt( thisTok.totalDist ).ToString();
		YouGameOver.DOFade(0, 0);
		MadeGameOver.DOFade(0, 0);
		PointsGameOver.DOFade(0, 0);
        quoteScore.DOFade(0, 0);
        PressPunch.transform.GetComponent<CanvasGroup>().DOFade(0, 0);
        PressSteroland.transform.GetComponent<CanvasGroup>().DOFade(0, 0);
        YouGameOver.transform.DOScale(5, 0);
		MadeGameOver.transform.DOScale(5, 0);
		PointsGameOver.transform.DOScale(5, 0);
		BarGameOver.transform.DOScaleY(0, 0);

		PatternGameOver.transform.DOLocalMoveY(-60, 5f).SetEase(Ease.Linear).OnComplete(() => {
			PatternGameOver.transform.DOLocalMoveY(1092, 0);
		}).SetLoops(-1, LoopType.Restart);

		gameObject.GetComponent<CanvasGroup>().DOFade(1f, 1.5f).OnComplete(() =>
		{
			YouGameOver.DOFade(1, .25f);
			YouGameOver.transform.DOScale(1, .25f).OnComplete(()=> {
				MadeGameOver.DOFade(1, .25f);
				MadeGameOver.transform.DOScale(1, .25f).OnComplete(() =>
				{
					PointsGameOver.DOFade(1, .25f);
					BarGameOver.transform.DOScaleY(1.25f, .2f).OnComplete(() =>
					{
						BarGameOver.transform.DOScaleY(1, .05f);
					});
					PointsGameOver.transform.DOScale(1, .25f);

                    //Debug.Log(distPlayer);
                    //Debug.Log(AllPlayerPrefs.saveData.listScore[0].finalScore);

                    DOVirtual.DelayedCall(1f, () => {
                        PressPunch.transform.GetComponent<CanvasGroup>().DOFade(1, .5f);
                        PressSteroland.transform.GetComponent<CanvasGroup>().DOFade(1, .5f);
                        quoteScore.DOFade(1, 2f);
                    });

                    if (AllPlayerPrefs.finalScore >= AllPlayerPrefs.saveData.listScore[0].finalScore)
                    {
                        newScore.transform.DOScale(5, 0);
                        newScore.transform.DOScale(1, .3f);
                        newScore.GetComponent<CanvasGroup>().DOFade(0, 0);
                        newScore.GetComponent<CanvasGroup>().DOFade(1, .3f);
                        /*
                        DOVirtual.DelayedCall(1, () =>
                        {
                            newScore.transform.DOScale(5, .2f);
                            newScore.GetComponent<CanvasGroup>().DOFade(0, .2f);
                        });*/
                    }

				});
			});
		});
	}

	public override void CloseThis ( )
	{
		canUpdate = false;

		gameObject.GetComponent<CanvasGroup> ( ).DOFade ( 0, TimeFade ).OnComplete ( ( ) =>
		{
			TimeFade = 0.25f;
			base.CloseThis ( );
		} );
    }
	#endregion

	#region Private Methods
	protected override void InitializeUi()
	{
	}
	#endregion
}
