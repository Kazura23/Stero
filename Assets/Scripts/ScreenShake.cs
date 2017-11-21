using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
	public static ScreenShake Singleton;

    Tween punchPos;
    Tween shakePos;
    float dir = 1;

	void Awake ()
	{
		if (ScreenShake.Singleton == null) {
			ScreenShake.Singleton = this;
		} else {
			Destroy (gameObject);
		}
	}


	void Update ( )
	{
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShakeEnemy();
        }
	}

	public void ShakeHitSimple ()
	{
        punchPos.Kill(true);

        dir *= -1; 
        //side = UnityEngine.Random.RandomRange(-2, 2);
        //transform.DOPunchRotation (Vector3.one * .5f, .3f, 3, 1);
        punchPos = transform.DOPunchPosition(new Vector3(1 * .85f * dir, 0, 1 * 1.5f), .5f, 2, 1);
    }

    public void ShakeHitDouble()
    {
        //transform.DOKill(true);
        //side = UnityEngine.Random.RandomRange(-2, 2);
        //transform.DOPunchRotation (Vector3.one * .5f, .3f, 3, 1);
        Camera.main.DOFieldOfView(50, .1f ).OnComplete(() => {
            Camera.main.DOFieldOfView(60, .1f);
        });
        //transform.DOPunchPosition(new Vector3(1 * .8f, 0, 1 * .8f), .25f, 3, 1);
    }

    public void ShakeEnemy()
    {
        punchPos.Kill(true);
        shakePos.Kill(true);
        //side = UnityEngine.Random.RandomRange(-2, 2);
        //transform.DOPunchRotation (Vector3.one * .5f, .3f, 3, 1);

        shakePos = transform.DOShakePosition(.15f, .3f, 15, 360);
        //transform.DOPunchPosition(new Vector3(1*1.5f, 0, 1*.5f), .25f, 4, 1);
    }

    public void ShakeMad()
    {
        transform.DOShakePosition(.4f, .65f, 22, 90);
    }

    public void ShakeGameOver()
    {


        //transform.DOKill(false);

        //transform.DOShakeRotation(1f, 2f, 22, 90);
        transform.DOShakePosition(1f, 2f, 22, 90);
    }

}