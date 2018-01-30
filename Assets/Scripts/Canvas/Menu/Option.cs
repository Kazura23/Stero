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

	public Slider MusicSlider;
	public Slider SonSlider;
	public Slider VoiceSlider;

	public Text Credit;

	Dictionary<AudioType, int> VolumeAudio;

	int currMusic;
	int currSon;
	int currVoice;

	OptionMenu currMenu;
	Player inputPlayer;
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
	}
	#endregion

	protected override void InitializeUi()
	{
		VolumeAudio = GlobalManager.AudioMa.VolumeAudio;

		MusicSlider.maxValue = 100;
		SonSlider.maxValue = 100;
		VoiceSlider.maxValue = 100;

		MusicSlider.value = AllPlayerPrefs.GetIntValueForSong ( "MusicVolume" );
		SonSlider.value = AllPlayerPrefs.GetIntValueForSong ( "SonVolume" );
		VoiceSlider.value = AllPlayerPrefs.GetIntValueForSong ( "VoiceVolume" );

		currMusic = ( int ) MusicSlider.value;
		currSon = ( int ) SonSlider.value;
		currVoice = ( int ) VoiceSlider.value;

		Credit.enabled = false;
		currMenu = OptionMenu.Son;
	}

	#region public
	public override void OpenThis(MenuTokenAbstract GetTok = null)
	{
		base.OpenThis ( GetTok );
		GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(1, .75f);
		openNewOption ( currMenu );
	}

	public override void CloseThis ( )
	{
		base.CloseThis();
	}

	#region private
	public void closeOptionMenu ( OptionMenu getOM )
	{
		switch ( getOM )
		{
		case OptionMenu.Son:
			MusicSlider.enabled = false;
			SonSlider.enabled = false;
			VoiceSlider.enabled = false;
			break;
		case OptionMenu.Credits:
			Credit.DOFade ( 0, 0 );
			Credit.enabled = false;
			break;
		}
	}
	#endregion

	public void openNewOption ( OptionMenu newOM )
	{
		closeOptionMenu ( currMenu );

		currMenu = newOM;

		switch ( newOM )
		{
		case OptionMenu.Son:
			MusicSlider.enabled = true;
			SonSlider.enabled = true;
			VoiceSlider.enabled = true;
			break;
		case OptionMenu.Credits:
			Credit.enabled = true;
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