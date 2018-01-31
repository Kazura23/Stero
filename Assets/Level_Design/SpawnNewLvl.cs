using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnNewLvl : MonoBehaviour 
{
	#region Variable
	public NewChunkInfo InfoChunk;

	[HideInInspector]
	public List<GameObject> ToDest;
	[HideInInspector]
	public List<GameObject> ToDisable;

	[HideInInspector]
	public List<GameObject> GarbChunk;

	[HideInInspector]
	public int OnLine;

	[HideInInspector]
	public bool AddToList = true;
	[HideInInspector]
	public bool OnScene = true;

	bool detect = false;
	#endregion
	
	#region Mono
	void OnEnable ( )
	{
		detect = false;
	}

	#endregion
		
	#region Public
	#endregion
	
	#region Private
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Constants._PlayerTag && !detect ) 
		{
			detect = true;
			if (AddToList) 
			{
				Transform getParent = transform;
				int count = 0;
				while ( count < 50 && getParent.parent.tag != Constants._ChunkParent )
				{
					count++;
					getParent = getParent.parent;
				}

				GlobalManager.GameCont.SpawnerChunck.AddNewChunk ( getParent.gameObject, OnScene, GarbChunk );
			}

			GlobalManager.GameCont.SpawnerChunck.NewSpawn ( InfoChunk );

			for ( int a = 0; a < ToDisable.Count; a++ )
			{
				if ( ToDisable [ a ] != null )
				{
					ToDisable [ a ].SetActive ( false );
				}
			}
			ToDisable.Clear ( );

			for ( int a = 0; a < ToDest.Count; a++ )
			{
				Destroy ( ToDest [ a ], 1 );
			}
			ToDest.Clear ( );

			PlayerController getPlayer = other.gameObject.GetComponent<PlayerController> ( );
			getPlayer.NbrLineLeft = (int)InfoChunk.NbrLaneDebut.x;
			getPlayer.currLine -= OnLine;
			getPlayer.NbrLineRight =  (int)InfoChunk.NbrLaneDebut.y;
		}
	}
	#endregion
}
	
[System.Serializable]
public class NewChunkInfo 
{
	[Tooltip ("X = nombre de lane a gauche et Y à droite ( ne pas inclure la ligne ou est attaché le script )")]
	public Vector2 NbrLaneDebut, NbrLaneFin;
	public bool calWall = true;
	public List <ChunkExit> ThoseExit;
	[HideInInspector]
	public List<GameObject> GarbageChunk;
}

[System.Serializable]
public class ChunkExit
{
	public int LaneParent = 0;
	public int Verticalite = 0;
	public Vector2 OtherNbrFin = new Vector2 ( -1, -1 );
	public Transform LevelParent;
	public ListChunkScriptable LCS;
	public GameObject WallEndChunk;
}