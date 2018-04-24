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
        Smusic.onValueChanged.AddListener(delegate 
        {
            ChangeValueAudio(music, Smusic.value / Smusic.maxValue);
            PlayerPrefs.SetFloat("SMusic", Smusic.value);
        });

        var Sambian = transform.GetChild(1).GetComponent<Slider>();
        Sambian.value = Sambian.maxValue;
        Sambian.GetComponent<Slider>().onValueChanged.AddListener(delegate 
        {
            ChangeValueAudio(ambiance, Sambian.value / Sambian.maxValue);
            PlayerPrefs.SetFloat("SAmbiance", Sambian.value);
        });

        var Svoix = transform.GetChild(2).GetComponent<Slider>();
        Svoix.value = Svoix.maxValue;
        Svoix.onValueChanged.AddListener(delegate 
        {
            ChangeValueAudio(voix, Svoix.value / Svoix.maxValue);
            PlayerPrefs.SetFloat("SVoice", Svoix.value);
        });

        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { Back(); });
        LoadOptionSound(Smusic, Sambian, Svoix);
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

    private void LoadOptionSound(Slider Smusic, Slider Sambiant, Slider Svoice)
    {
        for (int i = 0; music != null && i < music.Length; i++)
        {
            music[i].volume = PlayerPrefs.GetFloat("SMusic", 1);
        }
        Smusic.value = PlayerPrefs.GetFloat("SMusic", 1);

        for (int i = 0; ambiance != null && i < ambiance.Length; i++)
        {
            ambiance[i].volume = PlayerPrefs.GetFloat("SAmbiance", 1);
        }
        Sambiant.value = PlayerPrefs.GetFloat("SAmbiance", 1);

        for (int i = 0; voix != null && i < voix.Length; i++)
        {
            voix[i].volume = PlayerPrefs.GetFloat("SVoice", 1);
        }
        Svoice.value = PlayerPrefs.GetFloat("SVoice", 1);
    }
}
