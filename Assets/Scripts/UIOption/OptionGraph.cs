using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using Rewired.UI.ControlMapper;

public class OptionGraph : MonoBehaviour {

    public Vector2Int[] resolutionValue;
    public ControlMapper mapper;

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
        dropShadow.onValueChanged.AddListener(delegate { ChangeOmbreQuality(dropShadow.value); });

        var dropTexture = transform.GetChild(3).GetComponent<Dropdown>();
        dropTexture.ClearOptions();
        dropTexture.AddOptions(new List<string> { "Very High", "High", "Medium", "Low" });
        dropTexture.onValueChanged.AddListener(delegate { ChangeTextureQuality(dropTexture.value); });

        var dropVSync = transform.GetChild(4).GetComponent<Dropdown>();
        dropVSync.ClearOptions();
        dropVSync.AddOptions(new List<string> { "Don't Sync", "Every V Blank", "Every Second V Blank"});
        dropVSync.value = 1;
        dropVSync.onValueChanged.AddListener(delegate { ChangeVSync(dropVSync.value); });

        transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { Back(); });
    }
	
    private void ChangeResolution(int rang, bool fullSize)
    {
        Screen.SetResolution(resolutionValue[rang].x, resolutionValue[rang].y, fullSize);
    }

    private void ChangeOmbreQuality(int shadowResolution)
    {
        //QualitySettings.SetQualityLevel(); a voir
        QualitySettings.shadowResolution = (ShadowResolution)shadowResolution;
        Debug.Log("ok ombre");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //transform.GetChild(1).GetComponent<Dropdown>().Select();
            transform.GetChild(1).GetComponent<Dropdown>().Show();
        }
    }
}
