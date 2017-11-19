using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "ListChunk", menuName = "Scriptable/ListChunk", order = 5)]
public class ListChunkScriptable : ScriptableObject 
{
	public List<GameObject> SpawnEnable;
}
