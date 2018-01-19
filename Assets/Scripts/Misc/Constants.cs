﻿using UnityEngine;
using System.Collections.Generic;

public static class Constants
{
	#region Tag
	public const string _PlayerTag = "Player";
    public const string _PunchTag = "Punch";
	public const string _NewDirec = "ModifDirect";
	public const string _EnnemisTag = "Enemy";
	public const string _ObjDeadTag = "ObjDead";
	public const string _ObsTag = "objectEnv";
	public const string _ObsSafe = "ObsPropSafe";
	public const string _ObsEnn = "ObsPunch";
	public const string _DebrisEnv = "Debris";
	public const string _UnTagg = "Untagged";
    public const string _MissileBazoo = "MissileBazooka";
	public const string _Balls = "Balls";
	public const string _ElemDash = "ElemDash";

	public const string _SAbleEnnemy = "SpawnableEnnemy";
	public const string _SAbleObs = "SpawnableObstacle";
	public const string _SAbleDestObs = "SpawnableDestObs";
	public const string _SAbleCoin = "SpawnableCoin";
	public const string _DebutFinChunk = "DebutFinChunk";
	public const string _ObsPropSafe = "ObsPropSafe";
	public const string _Intro = "Intro";
	public const string _ChocWave = "ChocWave";
	public const string _Ground = "Ground";
	#endregion

	#region PlayerPref
	public const string Coin = "Coins";
	public const string ItemBought = "Item_";
	public const string ChunkUnLock = "NewUnLock";
    #endregion


    #region Other
    public const int DefFov = 60;
	public const int LineDist = 6;
	public const float DB_Prepare = 2;
	public const string DefRankName = "E";
	public const string TutoName = "TutoStart";
	//public const float ChunkLengh = 470;
	#endregion
}
