using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleScreen : UiParent 
{
    #region Variables

    public bool ready;
    public float delayBeforeReady;

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
			GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(0, .3f).OnComplete( () =>
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

		GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOKill ( );
		GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(1, 0);

		base.OpenThis ( GetTok );
	}
	public override void CloseThis ( )
	{
		introAudio.DOFade(0,.4f);
		GlobalManager.AudioMa.OpenAudio ( AudioType.Menu, "", true, null );
		GlobalManager.Ui.PatternBackground.GetComponent<RainbowScale>().enabled = false;
        GlobalManager.Ui.PatternBackground.GetComponent<RainbowMove>().enabled = false;
        GlobalManager.Ui.GlobalBack.transform.Find("BackgroundColor").GetComponent<RainbowColor>().enabled = false;

        GlobalManager.Ui.InieUI();
		base.CloseThis();
	}
	#endregion
}
