using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Option :  UiParent
{
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

	public override void OpenThis(MenuTokenAbstract GetTok = null)
	{
		base.OpenThis ( GetTok );
		openNewOption ( currMenu );
	}

	public override void CloseThis ( )
	{
		base.CloseThis();
	}

	void Update ()
	{
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

	public void openNewOption ( OptionMenu newOM )
	{
		if ( currMenu != null )
		{
			closeOptionMenu ( currMenu );
		}

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
}