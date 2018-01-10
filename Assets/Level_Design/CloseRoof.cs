using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRoof : MonoBehaviour {

    public GameObject Roof;
    public GameObject[] otherElement;

	Vector3 roofPos;
	List<Vector3> otherPos;

    public float timeBetweenClose = 5f;
    public float speed = 5f;

    private void Awake()
    {
        timeBetweenClose /= 100;
        speed /= 100;

		roofPos = Roof.transform.localPosition;

		for ( int a = 0; a < otherElement.Length; a++ )
		{
			otherPos.Add ( otherElement [ a ].transform.localPosition );	
		}
    }

	void OnEnable ( )
	{

		Roof.transform.localPosition = roofPos;

		for ( int a = 0; a < otherElement.Length; a++ )
		{
			otherElement [ a ].transform.localPosition = otherPos [ a ];;	
		}
	}

    IEnumerator CloseWallStart()
    {
		WaitForSeconds thisSec = new WaitForSeconds ( timeBetweenClose );
		Transform getTrans = Roof.transform;
		Transform getOtherTr;

		while ( getTrans.position.y > 0 )
        {
			yield return thisSec;

			getTrans.localPosition = new Vector3 ( getTrans.localPosition.x, getTrans.localPosition.y - speed, getTrans.localPosition.z );
		
            for ( int i = 0; i < otherElement.Length; i++ )
            {
				getOtherTr = otherElement [ i ].transform;
				getOtherTr.localPosition = new Vector3 ( getOtherTr.localPosition.x, getOtherTr.localPosition.y - speed, getOtherTr.localPosition.z );
            }
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
