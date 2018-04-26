using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleScreen : UiParent 
{
    #region Variables

    public bool ready;
    public float delayBeforeReady;

	public AudioSource hurtSound;
	AudioSource punch;

    AudioSource introAudio;

	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.Title;
		}
	}
	#endregion
	
	
	#region Mono
	void Update()
	{
		if(Input.anyKeyDown && ready)
		{

			if(hurtSound != null){
				hurtSound.Play();
				punch.Play();
				GlobalManager.AudioMa.OpenAudio ( AudioType.Other, "PunchSuccess", false );
				introAudio.DOFade(0,1f);
				ScreenShake.Singleton.ShakeFall();
			}

			GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete( () =>
			{
				GlobalManager.Ui.CloseThisMenu ( );
			});
		}
	}
	#endregion
	
	
	#region Public

	#endregion
	
	
	#region Private
	protected override void InitializeUi()
	{
        DOVirtual.DelayedCall(delayBeforeReady, () => {
            ready = true;
            GlobalManager.Ui.PatternBackground.GetComponent<RainbowMove>().enabled = true;
            GlobalManager.Ui.PatternBackground.GetComponent<RainbowScale>().enabled = true;
			GlobalManager.Ui.GlobalBack.transform.Find("BackgroundColor").GetComponent<RainbowColor>().enabled = true;
        });
	}

	public override void OpenThis(MenuTokenAbstract GetTok = null)
	{
		GlobalManager.AudioMa.CloseAudio ( AudioType.Menu );
		introAudio = GameObject.Find("Intro Audio").GetComponent<AudioSource>();

		hurtSound = GameObject.Find("Intro Audio").GetComponentsInChildren<AudioSource>()[2];
		punch = GameObject.Find("Intro Audio").GetComponentsInChildren<AudioSource>()[3];

		GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOKill ( );
		GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(1, 0);

		base.OpenThis ( GetTok );
	}
	public override void CloseThis ( )
	{

		GlobalManager.AudioMa.OpenAudio ( AudioType.Menu, "", true, null );
		GlobalManager.Ui.PatternBackground.GetComponent<RainbowScale>().enabled = false;
        GlobalManager.Ui.PatternBackground.GetComponent<RainbowMove>().enabled = false;
        GlobalManager.Ui.GlobalBack.transform.Find("BackgroundColor").GetComponent<RainbowColor>().enabled = false;

        GlobalManager.Ui.InieUI();
		base.CloseThis();
	}
	#endregion
}
