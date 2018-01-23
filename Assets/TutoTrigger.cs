using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutoTrigger : MonoBehaviour {

    //public int numTrigger;
    public GameObject[] textMesh;

    public enum TutoType
    {
        None,
        Reset,
        Madness,
        Money,
        Ability
    }

    public TutoType typeTuto;

	// Use this for initialization
	void Start () {


        GameObject TutoObject = GameObject.Find("TextObject");

        Debug.Log(TutoObject);

        foreach (Transform trans in TutoObject.transform)
        {

            Debug.Log(trans);
            trans.GetComponent<MeshRenderer>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < textMesh.Length; i++)
            {
                textMesh[i].GetComponent<MeshRenderer>().enabled = true;
            }
            //Debug.Log("pro");
            if (typeTuto == TutoType.Reset)
            {
                //GlobalManager.Ui.GameParent.GetComponent<CanvasGroup>().DOFade(0, .1f);
                GlobalManager.Ui.Madness.GetComponent<CanvasGroup>().DOFade(0, .1f);
                GlobalManager.Ui.MoneyPoints.transform.parent.GetComponent<CanvasGroup>().DOFade(0, .1f);
                GlobalManager.Ui.MotionSlider.transform.GetComponent<CanvasGroup>().DOFade(0, .1f);
            }

            if (typeTuto == TutoType.Madness)
            {
                GlobalManager.Ui.ArrowTuto.enabled = true;
                GlobalManager.Ui.Madness.GetComponent<CanvasGroup>().DOFade(1, .1f);
            }

            if(typeTuto == TutoType.Money)
            {
                GlobalManager.Ui.MoneyPoints.transform.parent.GetComponent<CanvasGroup>().DOFade(1, .1f);
            }

            if(typeTuto == TutoType.Ability)
            {
                GlobalManager.Ui.MotionSlider.transform.GetComponent<CanvasGroup>().DOFade(1, .1f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (typeTuto == TutoType.Madness)
            {
                GlobalManager.Ui.ArrowTuto.enabled = false;
            }

        }
    }
}
