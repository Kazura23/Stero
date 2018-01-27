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
			GlobalManager.GameCont.SpawnerChunck.NewSpawn ( InfoChunk );
			if (AddToList) 
			{
				Transform getParent = transform;
				while ( getParent.parent != null && getParent.parent.name != "Chuncks" )
				{
					getParent = getParent.parent;
				}

				if ( getParent.parent != null )
				{
					GlobalManager.GameCont.SpawnerChunck.AddNewChunk ( getParent.gameObject, OnScene, GarbChunk );
				}
				else
				{
					GlobalManager.GameCont.SpawnerChunck.AddNewChunk ( gameObject, OnScene, GarbChunk );
				}
			}

			PlayerController getPlayer = other.gameObject.GetComponent<PlayerController> ( );
			getPlayer.NbrLineLeft = (int)InfoChunk.NbrLaneDebut.x;
			getPlayer.currLine -= OnLine;
			getPlayer.NbrLineRight =  (int)InfoChunk.NbrLaneDebut.y;

			for ( int a = 0; a < ToDest.Count; a++ )
			{
				Destroy ( ToDest [ a ], 1 );
			}

			for ( int a = 0; a < ToDest.Count; a++ )
			{
				ToDest [ a ].SetActive ( false );
			}
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