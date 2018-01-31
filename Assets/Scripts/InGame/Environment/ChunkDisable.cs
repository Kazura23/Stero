using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDisable : MonoBehaviour 
{
	List<GameObject> ThosObject;

	void Awake ( )
	{
		ThosObject = new List<GameObject> ( );
	}

	void OnEnable ( )
	{
		ThosObject.Clear ( );
	}

	public void ReEnableObject ( )
	{
		foreach (GameObject thisOjb in ThosObject)
		{
			if ( thisOjb != null )
			{
				thisOjb.SetActive ( true );
			}
		}
	}

	public void AddNewObj ( GameObject thisObj )
	{
		Debug.Log ( thisObj.name );
		ThosObject.Add ( thisObj );
	}
}
