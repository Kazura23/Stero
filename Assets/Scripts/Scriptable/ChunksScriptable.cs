using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Chunk", menuName = "Scriptable/Chunk", order = 3)]
public class ChunksScriptable : ScriptableObject 
{
	public int ChunkLevel;

	[Tooltip ("Nombre de chunk pour faire un niveau")]
	public int NbrChunkOneLvl;
	public bool ChunkAleat = true;

	[Header ("Chunks")]
	public GameObject WallEndChunk;
	public Vector2 SizeWall = new Vector2 ( 0, -3 );
	public GameObject WallOnLastChunk;
	public List<GameObject> TheseChunks;
	public List<ChunksScriptable> TransitionChunks;

	[Header ("Prefabs d'objet à instancier aléatoirement")]

	[Range( 0,100)]
	public int PourcSpawn;

	[Tooltip ("Tous les ennemis que l'on peut spawn dans ce chunk")]
	public List<GameObject> EnnemySpawnable;

	[Tooltip ("Obstacle ne pouvant pas être détruit")]
	public List<GameObject> ObstacleSpawnable;

	[Tooltip ("Obstacle pouvant être détruit")]
	public List<GameObject> ObstacleDestrucSpawnable;
	public List<GameObject> CoinSpawnable;
}
