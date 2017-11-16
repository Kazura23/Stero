using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour 
{

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Constants._PlayerTag)
        {
            // AllPlayerPrefs.piece += piece;

            GlobalManager.Ui.TakeCoin();

            GetComponent<SphereCollider>().enabled = false;

            Debug.Log(GetComponent<MeshRenderer>().material.name);

            if (GetComponent<MeshRenderer>().material.name == "gold_piece_BC")
            { 
                AllPlayerPrefs.SetIntValue(Constants.Coin, 1);
                
            }
            else
            {
                AllPlayerPrefs.SetIntValue(Constants.Coin, 5);
            }

            GlobalManager.Ui.MoneyPoints.text = "" + AllPlayerPrefs.GetIntValue(Constants.Coin);

            transform.DOLocalRotate(new Vector3(0, 2000, 0),1f,RotateMode.FastBeyond360);

            Destroy(this.gameObject, 1f);
            
        }
    }
}
