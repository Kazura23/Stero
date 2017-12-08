using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour 
{
    public int point = 100;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Constants._PlayerTag)
        {

            GlobalManager.Ui.TakeCoin();

            GetComponent<CapsuleCollider>().enabled = false;

            Debug.Log(GetComponent<MeshRenderer>().material.name);

            if (GetComponent<MeshRenderer>().material.name == "gold_piece_BC")
            { 
                AllPlayerPrefs.SetIntValue(Constants.Coin, 1);
                
            }
            else
            {
                AllPlayerPrefs.SetIntValue(Constants.Coin, 5);
            }
            AllPlayerPrefs.scoreWhithoutDistance += point;
            AllPlayerPrefs.piece++;
            GlobalManager.Ui.MoneyPoints.text = "" + AllPlayerPrefs.GetIntValue(Constants.Coin);

            transform.DOLocalRotate(new Vector3(0, 2000, 0),1f,RotateMode.FastBeyond360);

            Destroy(this.gameObject, 1f);
            
        }
    }
}
