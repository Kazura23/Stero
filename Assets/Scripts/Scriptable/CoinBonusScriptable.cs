using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "CoinBonus", menuName = "Scriptable/CoinBonus", order = 5)]
public class CoinBonusScriptable : ScriptableObject
{
    public List<GameObject> CoinSpawn;
}