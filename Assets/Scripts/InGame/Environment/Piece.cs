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

            GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "Coin", false);

            GetComponent<CapsuleCollider>().enabled = false;

            if (GetComponent<MeshRenderer>().material.name == "GoldCoin (Instance)")
            { 
                AllPlayerPrefs.SetIntValue(Constants.Coin, 1);
            }
            else
            {
                AllPlayerPrefs.SetIntValue(Constants.Coin, 5);
            }
            //AllPlayerPrefs.scoreWhithoutDistance += point;
            // Ajouter score
            AllPlayerPrefs.piece++;
            GlobalManager.Ui.MoneyPoints.text = "" + AllPlayerPrefs.GetIntValue(Constants.Coin);

			transform.DOLocalRotate ( new Vector3 ( 0, 2000, 0 ), 1f, RotateMode.FastBeyond360 );

			GetComponent<MeshRenderer>().enabled = false;

			DOVirtual.DelayedCall(2, () =>
			{
				GetComponent<MeshRenderer>().enabled = true;
				GetComponent<CapsuleCollider>().enabled = true;
			});
		}
    }
}
