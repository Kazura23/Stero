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
	public int OnLine;

	bool detect = false;
	#endregion
	
	#region Mono
	#endregion
		
	#region Public
	#endregion
	
	#region Private
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player"&& !detect) 
		{
			detect = true;
			GlobalManager.GameCont.SpawnerChunck.NewSpawn ( InfoChunk );
			GlobalManager.GameCont.SpawnerChunck.AddNewChunk ( gameObject );

			PlayerController getPlayer = other.gameObject.GetComponent<PlayerController> ( );
			getPlayer.NbrLineLeft = (int)InfoChunk.NbrLaneDebut.x;
			getPlayer.currLine -= OnLine;
			getPlayer.NbrLineRight =  (int)InfoChunk.NbrLaneDebut.y;

			for ( int a = 0; a < ToDest.Count; a++ )
			{
				Destroy ( ToDest [ a ], 1 );
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
	public List <ChunkExit> ThoseExit;
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