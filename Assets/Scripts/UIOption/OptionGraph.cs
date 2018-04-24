using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using Rewired.UI.ControlMapper;

public class OptionGraph : MonoBehaviour {

    public Vector2Int[] resolutionValue;

	// Use this for initialization
	void Start () {
        var toggleFullSize = transform.GetChild(0).GetComponent<Toggle>();
        toggleFullSize.isOn = Screen.fullScreen;

        var dropReso = transform.GetChild(1).GetComponent<Dropdown>();
        dropReso.ClearOptions();
        dropReso.AddOptions(StrReso());
        dropReso.onValueChanged.AddListener(delegate { ChangeResolution(dropReso.value, toggleFullSize.isOn); });

        var dropShadow = transform.GetChild(2).GetComponent<Dropdown>();
        dropShadow.ClearOptions();
        dropShadow.AddOptions(new List<string> { "Very High", "High", "Medium", "Low"});
        dropShadow.onValueChanged.AddListener(delegate 
        {
            ChangeOmbreQuality(dropShadow.value);
            PlayerPrefs.SetInt("QOmbre", dropShadow.value);
        });

        var dropTexture = transform.GetChild(3).GetComponent<Dropdown>();
        dropTexture.ClearOptions();
        dropTexture.AddOptions(new List<string> { "Very High", "High", "Medium", "Low" });
        dropTexture.onValueChanged.AddListener(delegate 
        {
            ChangeTextureQuality(dropTexture.value);
            PlayerPrefs.SetInt("QTexture", dropTexture.value);
        });

        var dropVSync = transform.GetChild(4).GetComponent<Dropdown>();
        dropVSync.ClearOptions();
        dropVSync.AddOptions(new List<string> { "Don't Sync", "Every V Blank", "Every Second V Blank"});
        dropVSync.value = 1;
        dropVSync.onValueChanged.AddListener(delegate 
        {
            ChangeVSync(dropVSync.value);
            PlayerPrefs.SetInt("V-Sync", dropVSync.value);
        });

        transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { Back(); });
        LoadOptionGraph(dropShadow, dropTexture, dropVSync);
    }
	
    private void ChangeResolution(int rang, bool fullSize)
    {
        Screen.SetResolution(resolutionValue[rang].x, resolutionValue[rang].y, fullSize);
    }

    private void ChangeOmbreQuality(int shadowResolution)
    {
        //QualitySettings.SetQualityLevel(); a voir
        QualitySettings.shadowResolution = (ShadowResolution)shadowResolution;
    }

    private void ChangeTextureQuality(int level)
    {
        QualitySettings.masterTextureLimit = level;
    }

    private void ChangeVSync(int level) // 0 à 4 (0 don't sync)
    {
        QualitySettings.vSyncCount = level;
    }

    private void Back()
    {
        transform.parent.GetChild(0).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private List<string> StrReso()
    {
        var newList = new List<string>();
        for(int i = 0; resolutionValue != null && i < resolutionValue.Length; i++)
        {
            newList.Add(resolutionValue[i].x + " / " + resolutionValue[i].y);
        }
        return newList;
    }

    private void LoadOptionGraph(Dropdown shadow, Dropdown texture, Dropdown vsync)
    {
        ChangeOmbreQuality(PlayerPrefs.GetInt("QOmbre", 0));
        shadow.value = PlayerPrefs.GetInt("QOmbre", 0);

        ChangeTextureQuality(PlayerPrefs.GetInt("QTexture", 0));
        texture.value = PlayerPrefs.GetInt("QTexture", 0);

        ChangeVSync(PlayerPrefs.GetInt("V-Sync", 0));
        vsync.value = PlayerPrefs.GetInt("V-Sync", 0);
    }
}
