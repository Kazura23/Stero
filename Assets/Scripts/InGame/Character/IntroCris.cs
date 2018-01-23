using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCris : MonoBehaviour 
{
	// Update is called once per frame
	void OnEnable () 
	{
		GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Intro", false);
		GameObject musicObject = GlobalManager.GameCont.musicObject;

		// Démarrage de la musique du Hub amplifiée après Stéro
		musicObject.GetComponent<AudioSource>().volume = 0.0004f;
		musicObject.GetComponent<AudioLowPassFilter>().enabled = true;
		musicObject.GetComponent<AudioDistortionFilter>().enabled = true;
	}
}
