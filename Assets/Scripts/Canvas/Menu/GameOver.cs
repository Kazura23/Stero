using System.Collections;
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
	public Transform MoneyMut;
	public int RatioScorePiece = 1000;
	public Text YouGameOver, MadeGameOver, PointsGameOver, PressPunch, PressSteroland, Highscore, quoteScore;
	public Text newScore;
	public Text CoinWin;
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

		
		GlobalManager.GameCont.textIntroObject.gameObject.SetActive(true);

        AllPlayerPrefs.SendAnalytics();
        AllPlayerPrefs.saveData.Add(AllPlayerPrefs.NewData());
        StaticRewardTarget.SaveReward();

		canUpdate = true;
        //float distPlayer = GlobalManager.GameCont.Player.GetComponent<PlayerController>().totalDis;

		GlobalManager.GameCont.SpawnerChunck.RemoveAll ( );
		Highscore.text = ( AllPlayerPrefs.saveData.listScore.Count > 0 ? AllPlayerPrefs.saveData.listScore [ 0 ].finalScore : 0 ).ToString ( );
		System.Action <DeadBallEvent> checkDBE = delegate ( DeadBallEvent thisEvnt )
		{
		};
		System.Action <DeadBallParent> checkDBP = delegate ( DeadBallParent thisEvnt ) 
		{ 
		}; 

		GlobalManager.Event.UnRegister ( checkDBP );
		GlobalManager.Event.UnRegister ( checkDBE );

		GlobalManager.Ui.ExtraHearts [ 0 ].enabled = false; 
		GlobalManager.Ui.ExtraHearts [ 1 ].enabled = false; 

		GameOverTok thisTok = GetTok as GameOverTok;
        GetComponent<CanvasGroup>().DOFade(0, 0);
		PointsGameOver.text = Mathf.RoundToInt( thisTok.totalDist ).ToString();
		CoinWin.text = "+ " + ((int)thisTok.totalDist / RatioScorePiece).ToString();
		CoinWin.transform.localScale = Vector3.zero;
		CoinWin.transform.localPosition = new Vector3(0,290,0);

		AllPlayerPrefs.SetIntValue(Constants.Coin, (int)thisTok.totalDist / RatioScorePiece, true);
			
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

		
		PointsGameOver.text = NbrString ( PointsGameOver.text );
		Highscore.text = NbrString ( Highscore.text );

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

					CoinWin.transform.DOScale(1, .25f).OnComplete(() => {
						CoinWin.transform.DOPunchScale((Vector3.one * .45f), .9f, 15, 1).OnComplete(() => {
							CoinWin.transform.DOScale(0, .5f);
							CoinWin.transform.DOMove(MoneyMut.gameObject.transform.position, .5f).OnComplete( () =>
							{
								GlobalManager.Ui.MoneyPoints.text = AllPlayerPrefs.GetIntValue(Constants.Coin).ToString();
								MoneyMut.GetComponent<Text>().text = GlobalManager.Ui.MoneyPoints.text;
							});
						});
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
		
        newScore.GetComponent<CanvasGroup>().DOFade(0, 0.1f);

		canUpdate = false;

		GlobalManager.Ui.ScoreString = "0";
		GlobalManager.Ui.ScorePoints.text = "0";

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
	#endregion
}
