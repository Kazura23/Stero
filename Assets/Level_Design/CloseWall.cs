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
        while(true)
        {
            yield return new WaitForSeconds(timeBetweenClose) ;

            WallRight.transform.position = new Vector3(WallRight.transform.position.x - speed, WallRight.transform.position.y, WallRight.transform.position.z);

            WallLeft.transform.position = new Vector3(WallLeft.transform.position.x + speed, WallLeft.transform.position.y, WallLeft.transform.position.z);

            if (WallRight.transform.position.x <= WallLeft.transform.position.x)
            {
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("hey");
            StartCoroutine(CloseWallStart());
        }
    }
}
