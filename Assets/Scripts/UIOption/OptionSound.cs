using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSound : MonoBehaviour {

    public AudioSource[] music, ambiance, voix;

	// Use this for initialization
	void Start () {
        var Smusic = transform.GetChild(0).GetComponent<Slider>();
        Smusic.value = Smusic.maxValue;
        Smusic.onValueChanged.AddListener(delegate { ChangeValueAudio(music, Smusic.value / Smusic.maxValue); });

        var Sambian = transform.GetChild(1).GetComponent<Slider>();
        Sambian.value = Sambian.maxValue;
        Sambian.GetComponent<Slider>().onValueChanged.AddListener(delegate { ChangeValueAudio(ambiance, Sambian.value / Sambian.maxValue); });

        var Svoix = transform.GetChild(2).GetComponent<Slider>();
        Svoix.value = Svoix.maxValue;
        Svoix.onValueChanged.AddListener(delegate { ChangeValueAudio(voix, Svoix.value / Svoix.maxValue); });

        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { Back(); });
    }
	
    private void Back()
    {
        transform.parent.GetChild(0).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ChangeValueAudio(AudioSource[] audio, float volume)
    {
        for(int i = 0; audio != null && i < audio.Length; i++)
        {
            audio[i].volume = volume;
        }
    }
}
