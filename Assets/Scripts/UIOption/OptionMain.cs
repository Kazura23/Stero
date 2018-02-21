using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired.UI.ControlMapper;

public class OptionMain : MonoBehaviour {

    public ControlMapper controlMapper;

	// Use this for initialization
	void Start () {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { OpenSound(); });
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { OpenGraph(); });
        transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { OpenCommand(); });
        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { Back(); });
    }
	
    private void OpenSound()
    {
        transform.parent.GetChild(1).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OpenGraph()
    {
        transform.parent.GetChild(2).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OpenCommand()
    {
        controlMapper.Open();
        gameObject.SetActive(false);
    }

    private void Back()
    {

    }

    public void BackCommand()
    {
        gameObject.SetActive(true);
        controlMapper.Close(true);
    }
}
