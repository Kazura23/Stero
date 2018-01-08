using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWall : MonoBehaviour {

    public GameObject WallRight;
    public GameObject WallLeft;

    public float timeBetweenClose = 5f ;
    public float speed = 5f ;

    private void Start()
    {
        timeBetweenClose /= 100;
        speed /= 100;
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
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("hey");
			StartCoroutine ( CloseWallStart ( ) );
        }
    }
}
