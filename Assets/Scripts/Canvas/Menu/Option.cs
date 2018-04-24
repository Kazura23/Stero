using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Rewired;

public class Option :  UiParent
{
	#region Variables
	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.Option;
		}
	}

	[System.Serializable]
	struct OptionObj
	{
		public OptionMenu ThisOption;
		public GameObject ThisObj;
	}
	
	[SerializeField] OptionObj[] GetOptionObj;

	public Slider MusicSlider;
	public Slider SonSlider;
	public Slider VoiceSlider;

	public Text Credit;
	
	int currMusic;
	int currSon;
	int currVoice;

	OptionMenu currMenu;
	Player inputPlayer;

	int indexOption = 0;
	bool checkAxis = false;
	#endregion

	#region mono
	void Start ( )
	{
		inputPlayer = ReInput.players.GetPlayer(0);
	}

	void Update ()
	{
		if ( Input.GetKeyDown ( KeyCode.Escape ) )
		{
			GlobalManager.Ui.CloseThisMenu ( );
		}

		float getV = inputPlayer.GetAxis ( "Vertical" );

		if ( currMusic != MusicSlider.value )
		{
			currMusic = ( int ) MusicSlider.value;
		}
		else if ( currSon != SonSlider.value )
		{
			currSon = ( int ) SonSlider.value;
		}
		else if ( currVoice != VoiceSlider.value )
		{
			currVoice = ( int ) VoiceSlider.value;
		}

		if ( getV < 0.2f && getV > -0.2f )
		{
			checkAxis = false;
		}

		if ( !checkAxis )
		{
			if ( getV > 0.9f )
			{
				checkAxis = true;
				openNewOption(-1);	
			}
			else if ( getV < -0.9f)
			{
				checkAxis = true;
				openNewOption(1);	
			}
		}
		
	}
	#endregion

	protected override void InitializeUi()
	{
		MusicSlider.maxValue = 100;
		SonSlider.maxValue = 100;
		VoiceSlider.maxValue = 100;

		MusicSlider.value = AllPlayerPrefs.GetIntValueForSong ( "MusicVolume" );
		SonSlider.value = AllPlayerPrefs.GetIntValueForSong ( "SonVolume" );
		VoiceSlider.value = AllPlayerPrefs.GetIntValueForSong ( "VoiceVolume" );

		currMusic = ( int ) MusicSlider.value;
		currSon = ( int ) SonSlider.value;
		currVoice = ( int ) VoiceSlider.value;

		currMenu = GetOptionObj[indexOption].ThisOption;
	}

	#region public
	public override void OpenThis(MenuTokenAbstract GetTok = null)
	{
		base.OpenThis ( GetTok );
		GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(1, .75f);
		openNewOption ( 0 );
	}

	public override void CloseThis ( )
	{
		base.CloseThis();
	}

	#region private
	public void closeOptionMenu ( )
	{
		//Debug.Log(GetOptionObj[indexOption].ThisObj.transform.GetChild(0).gameObject);
		Transform getOpTrans = GetOptionObj[indexOption].ThisObj.transform.GetChild(0);

		getOpTrans.GetChild(0).GetComponent<Image>().DOKill();
		getOpTrans.GetChild(0).GetComponent<Image>().DOFillAmount(0,.05f);
		
		getOpTrans.GetComponent<CanvasGroup>().DOFade(0,.05f);
		getOpTrans.GetComponent<CanvasGroup>().DOKill();


		switch ( currMenu )
		{
		case OptionMenu.Son:
			break;
		case OptionMenu.Credits:
			Credit.DOFade ( 0, 0 );
			break;
		}
	}
	#endregion

	public void openNewOption ( int newIndex )
	{
		closeOptionMenu ( );
		
		indexOption += newIndex;

		if ( indexOption > GetOptionObj.Length - 1 )
		{
			indexOption = 0;
		}
		else if ( indexOption < 0 )
		{
			indexOption = GetOptionObj.Length - 1;
		}

		Transform getOpTrans = GetOptionObj[indexOption].ThisObj.transform.GetChild(0);
		getOpTrans.GetChild(0).GetComponent<Image>().DOKill();
		getOpTrans.GetChild(0).GetComponent<Image>().DOFillAmount(1,.15f);
		
		getOpTrans.GetComponent<CanvasGroup>().DOKill();
		getOpTrans.GetComponent<CanvasGroup>().DOFade(1,.15f);

		currMenu = GetOptionObj[indexOption].ThisOption;

		GetOptionObj[indexOption].ThisObj.transform.GetChild(0).gameObject.SetActive(true);
		
		switch ( currMenu )
		{
		case OptionMenu.Son:
			break;
		case OptionMenu.Credits:
			Credit.DOFade ( 1, 0.2f ).OnComplete ( ( ) =>
			{
				Credit.transform.DOScale ( new Vector3 ( 5, 5, 5 ), 0 ).OnComplete ( ( ) =>
				{
					Credit.transform.DOScale ( 1, 0.2f );
				} );
			} );
			break;
		}
	}
	#endregion
}
