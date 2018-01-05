using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRoof : MonoBehaviour {

    public GameObject Roof;
    public GameObject[] otherElement;

    public float timeBetweenClose = 5f;
    public float speed = 5f;

    private void Start()
    {
        timeBetweenClose /= 100;
        speed /= 100;
    }

    IEnumerator CloseWallStart()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenClose);

            Roof.transform.position = new Vector3(Roof.transform.position.x, Roof.transform.position.y - speed, Roof.transform.position.z);

            for(int i = 0; i < otherElement.Length; i++)
            {
                otherElement[i].transform.position = new Vector3(otherElement[i].transform.position.x, otherElement[i].transform.position.y - speed, otherElement[i].transform.position.z);
            }

            if (Roof.transform.position.y <= 0)
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
