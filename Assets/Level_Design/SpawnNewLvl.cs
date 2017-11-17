using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnNewLvl : MonoBehaviour 
{
	#region Variable
	public NewChunkInfo InfoChunk;

	[HideInInspector]
	public List<GameObject> ToDest;

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
			for ( int a = 0; a < ToDest.Count; a++ )
			{
				Destroy ( ToDest [ a ] );
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
	public Transform LevelParent;
	public ListChunkScriptable LCS;
}