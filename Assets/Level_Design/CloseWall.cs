using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWall : MonoBehaviour {

    public GameObject WallRight;
    public GameObject WallLeft;

    public float timeBetweenClose = 5f ;
    public float speed = 5f ;

	Vector3 wallRPos;
	Vector3 wallLPos;

    private void Awake()
    {
        timeBetweenClose /= 100;
        speed /= 100;

		wallRPos = WallRight.transform.localPosition;
		wallLPos = WallLeft.transform.localPosition;
    }

	void OnEnable ( )
	{
		WallRight.transform.localPosition = wallRPos;
		WallLeft.transform.localPosition = wallLPos;
	}

    IEnumerator CloseWallStart()
    {
		WaitForSeconds thisSec = new WaitForSeconds ( timeBetweenClose );
		Transform wRight = WallRight.transform;
		Transform wLeft = WallLeft.transform;

		while(wRight.position.x > wLeft.position.x)
        {
			yield return thisSec;

			wRight.localPosition = new Vector3 ( wRight.localPosition.x - speed, wRight.localPosition.y, wRight.localPosition.z );

			wLeft.localPosition = new Vector3 ( wLeft.localPosition.x + speed, wLeft.localPosition.y, wLeft.localPosition.z );
        }
    }

    void OnTriggerEnter(Collider other)
    {
		if ( other.gameObject.tag == "Player" )
        {
			StartCoroutine ( CloseWallStart ( ) );
        }
    }
}
