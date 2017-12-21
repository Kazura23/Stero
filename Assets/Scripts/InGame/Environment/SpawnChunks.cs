using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnChunks : MonoBehaviour 
{
	#region Variable
	public List<ChunksScriptable> ChunksInfo;
	public Vector3 DefaultPos;

	[HideInInspector]
	public int currLevel = 0;

	[HideInInspector]
	public bool StartBonus = false;

	[HideInInspector]
	public int EndLevel = 0;

	List<ChunkCombineSpawnble> LvlChunksInfo;
	List<GameObject> getSpawnChunks;
	List<GameObject> otherSpawn;

	ChunksScriptable thisChunk;
	GameObject WallOnLastChunk;
	Transform thisT;
	int currNbrCh = 0;
	int currChunk = 0;
	int CurrRandLvl = 0;
	int saveLvlForStart = 0;
	bool randAllChunk = false;
	bool transitChunk = false;
	int minLevel = 0;
	#endregion
	
	#region Mono
	#endregion
		
	#region Public
	public void RemoveAll ( )
	{
		int a;

		for ( a = 0; a < otherSpawn.Count; a++ )
		{
			if ( otherSpawn [ a ] != null )
			{
				Destroy ( otherSpawn [ a ] );
			}
		}

		for ( a = 0; a < getSpawnChunks.Count; a++ )
		{
			if ( getSpawnChunks [ a ] != null )
			{
				Destroy ( getSpawnChunks [ a ] );
			}
		}
	}

	public void InitChunck ( )
	{
		getSpawnChunks = new List<GameObject> ( );
		otherSpawn = new List<GameObject> ( );
		thisT = transform;

		List<ChunksScriptable> getChunks = ChunksInfo;
		List<List<GetSpawnable>> getSpawnable = new List<List<GetSpawnable>> ( );
		List<ChunkCombineSpawnble> chunkOrder = new List<ChunkCombineSpawnble> ( );

		Transform[] getChildrenChunk;

		int a;
		int b;
		int c;
		int currChunkLvl;

		for ( a = 0; a < getChunks.Count; a++ )
		{
			getSpawnable.Add ( new List<GetSpawnable> ( ) );

			currChunkLvl = getChunks [ a ].ChunkLevel;
			while ( chunkOrder.Count - 1 < currChunkLvl )
			{
				chunkOrder.Add ( new ChunkCombineSpawnble ( ) );
				chunkOrder [ chunkOrder.Count - 1 ].ChunkScript = new List<ChunksScriptable> ( );
				chunkOrder [ chunkOrder.Count - 1 ].SpawnAble = new List<GetSpawnable> ( );
			}

			chunkOrder [ currChunkLvl ].ChunkScript.Add ( getChunks [ a ] );
			for ( b = 0; b < getChunks [ a ].TheseChunks.Count; b++ )
			{
				getSpawnable [ a ].Add ( new GetSpawnable ( ) );
				getSpawnable [ a ] [ b ].getCoinSpawnable = new List<GameObject> ( );
				getSpawnable [ a ] [ b ].getEnnemySpawnable = new List<GameObject> ( );
				getSpawnable [ a ] [ b ].getObstacleDestrucSpawnable = new List<GameObject> ( );
				getSpawnable [ a ] [ b ].getObstacleSpawnable = new List<GameObject> ( );

				getChildrenChunk = getChunks [ a ].TheseChunks [ b ].GetComponentsInChildren<Transform> ( true );

				for ( c = 0; c < getChildrenChunk.Length; c++ )
				{
					switch ( getChildrenChunk[c].tag )
					{
					case Constants._SAbleCoin:
						getSpawnable [ a ] [ b ].getCoinSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleDestObs:
						getSpawnable [ a ] [ b ].getObstacleDestrucSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleObs:
						getSpawnable [ a ] [ b ].getObstacleSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleEnnemy:
						getSpawnable [ a ] [ b ].getEnnemySpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					}
				}
			}

			chunkOrder [ currChunkLvl ].SpawnAble = getSpawnable [ a ];
		}

		LvlChunksInfo = chunkOrder;
		bool checkZero = false;
			
		while ( chunkOrder [ currLevel ].ChunkScript.Count == 0 )
		{
			currLevel++;

			if ( currLevel > chunkOrder.Count )
			{
				if ( !checkZero )
				{
					checkZero = true;
					currLevel = 0;
				}
				else
				{
					return;
				}
			}
		}

		minLevel = currLevel;

		CurrRandLvl = Random.Range ( 0, chunkOrder [ currLevel ].ChunkScript.Count );

		thisChunk = chunkOrder [ currLevel ].ChunkScript [ CurrRandLvl ];
	}

	public void NewSpawn ( NewChunkInfo sourceSpawn )
	{
		List<GameObject> getSpc = getSpawnChunks;

		spawnAfterThis ( sourceSpawn );

		if ( getSpc.Count > 2 )
		{
			Destroy ( getSpc [ 0 ] );
			getSpc.RemoveAt ( 0 );
		}

		if ( thisChunk.NbrChunkOneLvl < currNbrCh )
		{
			if ( StartBonus )
			{
				if ( saveLvlForStart >= EndLevel )
				{
					currLevel = saveLvlForStart;
					StartBonus = false;
				}

				saveLvlForStart++;
			}
			newLevel ( );
		}
	}

	public void FirstSpawn ( )
	{
		randAllChunk = false;
		currNbrCh = 0;
		currLevel = 0;
		saveLvlForStart = 0;
		transitChunk = false;

		List<GameObject> getSpc = getSpawnChunks;
		bool doubleFirst = false;

		if ( !StartBonus )
		{
			currLevel = minLevel;
		}

		while ( getSpc.Count > 0 )
		{
			doubleFirst = true;
			Destroy ( getSpc [ 0 ] );
			getSpc.RemoveAt ( 0 );
		}

		if ( doubleFirst )
		{
			StartCoroutine ( waitSpawn ( ) );
		}
		else
		{
			spawnAfterThis ( );

			if ( thisChunk.NbrChunkOneLvl < currNbrCh )
			{
				newLevel ( );
			}
		}
	}

	public void AddNewChunk ( GameObject thisSpawn )
	{
		getSpawnChunks.Add ( thisSpawn );
	}
	#endregion
	
	#region Private
	IEnumerator waitSpawn ( )
	{
		yield return new WaitForEndOfFrame ( );

		spawnAfterThis ( );

		if ( thisChunk.NbrChunkOneLvl < currNbrCh )
		{
			newLevel ( );
		}
	}

	void newLevel ( )
	{
		ChunksScriptable getCunk = thisChunk;
		List<GameObject> getSpc = getSpawnChunks;
		Transform getChunkT = getSpc [ getSpc.Count - 1 ].transform;
		GameObject thisObj;
		List<ChunkCombineSpawnble> chunkOrder = LvlChunksInfo;

		if ( getCunk.WallOnLastChunk != null )
		{
			thisObj = ( GameObject ) Instantiate ( getCunk.WallOnLastChunk, thisT );
			thisObj.transform.position = getChunkT.position;
			thisObj.transform.localPosition += thisObj.transform.up * thisObj.GetComponent<MeshRenderer> ( ).bounds.size.y / 2;
		}

		if ( getCunk.TransitionChunks.Count == 0 || transitChunk )
		{
			transitChunk = false;
			currLevel++;

			if ( currLevel >= chunkOrder.Count || randAllChunk )
			{
				randAllChunk = true;

				if ( chunkOrder.Count > 1 )
				{
					currLevel = Random.Range ( 1, LvlChunksInfo.Count );
				}
				else
				{
					currLevel = 0;
				}
			}

			while ( chunkOrder [ currLevel ].ChunkScript.Count == 0 )
			{
				currLevel++;

				if ( currLevel > chunkOrder.Count )
				{
					return;
				}
			}

			CurrRandLvl = Random.Range ( 0, LvlChunksInfo [ currLevel ].ChunkScript.Count );
		}
		else
		{
			transitChunk = true;
		}

		currNbrCh = 0;
	}

	void spawnAfterThis ( NewChunkInfo sourceSpawn = null )
	{
		List<ChunkCombineSpawnble> chunkOrder = LvlChunksInfo;
		GameObject thisSpawn;
		Transform getChunkT;
		ChunksScriptable getChunk;
		GetSpawnable getSble;
		GameObject currWall;

		if ( !transitChunk )
		{
			Debug.Log ( CurrRandLvl + " / " + chunkOrder [ currLevel ].ChunkScript.Count );
				
			getChunk = chunkOrder [ currLevel ].ChunkScript [ CurrRandLvl ];
			getSble = chunkOrder [ currLevel ].SpawnAble [ CurrRandLvl ];
		}
		else
		{
			getChunk = chunkOrder [ currLevel ].ChunkScript [ CurrRandLvl ].TransitionChunks [ Random.Range ( 0, chunkOrder [ currLevel ].ChunkScript [ CurrRandLvl ].TransitionChunks.Count ) ];
			getSble = new GetSpawnable ( );

			getSble.getEnnemySpawnable = getChunk.EnnemySpawnable;
			getSble.getObstacleSpawnable = getChunk.ObstacleSpawnable;
			getSble.getObstacleDestrucSpawnable = getChunk.ObstacleDestrucSpawnable;
			getSble.getCoinSpawnable = getChunk.CoinSpawnable;
		}

		thisChunk = getChunk;

		if ( sourceSpawn != null )
		{
			List<VertNCSI> getNewChunk = new List<VertNCSI> ( );
			List<NewChunkSaveInf> getCurrNew;
			List<ToDestChunk> allNewChunk = new List<ToDestChunk> ( );
			List<GameObject> getThoseChunk = getChunk.TheseChunks;

			NewChunkSaveInf getOtherNC;
			SpawnNewLvl currSL;

			int a;
			int b;
			int c;
			int randChunk = Random.Range ( 0, 2 );
			int getInd;
			int diffLine;
			int vertChunk;

			for ( a = 0; a < sourceSpawn.ThoseExit.Count; a++ )
			{
				currChunk = currNbrCh;

				if ( getChunk.ChunkAleat || currChunk >= getThoseChunk.Count )
				{
					currChunk = Random.Range ( 0, getThoseChunk.Count );
				}

				if ( sourceSpawn.ThoseExit.Count > 1 && sourceSpawn.ThoseExit [ a ].LCS != null && sourceSpawn.ThoseExit [ a ].LCS.SpawnEnable.Count > 0 )
				{
					thisSpawn = sourceSpawn.ThoseExit [ a ].LCS.SpawnEnable [ Random.Range ( 0, sourceSpawn.ThoseExit [ a ].LCS.SpawnEnable.Count ) ];
				}
				else
				{
					thisSpawn = getThoseChunk [ currChunk ];
				}

				if ( thisSpawn != null )
				{
					thisSpawn = ( GameObject ) Instantiate ( thisSpawn, thisT );

					currSL = thisSpawn.GetComponentInChildren<SpawnNewLvl> ( );
					currSL.OnLine = sourceSpawn.ThoseExit [ a ].LaneParent;
					getChunkT = thisSpawn.transform;
					getChunkT.rotation = sourceSpawn.ThoseExit [ a ].LevelParent.rotation;
					getChunkT.position = sourceSpawn.ThoseExit [ a ].LevelParent.position;
					vertChunk = sourceSpawn.ThoseExit [ a ].Verticalite;

					getInd = -1;
					for ( b = 0; b < getNewChunk.Count; b++ )
					{
						if ( getNewChunk [ b ].Vert == vertChunk )
						{
							getInd = b;
							break;
						}
					}

					if ( getInd == -1 )
					{
						getNewChunk.Add ( new VertNCSI ( ) );
						getInd = getNewChunk.Count - 1;
						getNewChunk [ getInd ].AllInfNewChunk = new List<NewChunkSaveInf> ( );
						getNewChunk [ getInd ].Vert = vertChunk;
					}

					if ( sourceSpawn.ThoseExit [ a ].OtherNbrFin == new Vector2 ( -1, -1 ) )
					{
						sourceSpawn.ThoseExit [ a ].OtherNbrFin = sourceSpawn.NbrLaneFin;
					}

					getCurrNew = getNewChunk [ getInd ].AllInfNewChunk;
					getCurrNew.Add ( new NewChunkSaveInf ( ) );

					vertChunk = getCurrNew.Count - 1;
					getCurrNew [ vertChunk ].SpawnNL = currSL;
					getCurrNew [ vertChunk ].ThisObj = getChunkT.gameObject;
					getCurrNew [ vertChunk ].NbrLaneDebut = currSL.InfoChunk.NbrLaneDebut;
					getCurrNew [ vertChunk ].CurrLane = sourceSpawn.ThoseExit [ a ].LaneParent;
					getCurrNew [ vertChunk ].CurrVert = sourceSpawn.ThoseExit [ a ].Verticalite;
					getCurrNew [ vertChunk ].WallEndChunk = sourceSpawn.ThoseExit [ a ].WallEndChunk;

					allNewChunk.Add ( new ToDestChunk ( ) );
					allNewChunk [ allNewChunk.Count - 1 ].ThisSL = currSL;
					allNewChunk [ allNewChunk.Count - 1 ].ThisObj = thisSpawn;

					if (sourceSpawn.ThoseExit.Count > 1) 
					{
						otherSpawn.Add ( thisSpawn );
					} 
					else 
					{
						currSL.AddToList = false;
						getSpawnChunks.Add (thisSpawn);
					}

				}
			}

			// add the other chunk on current chunk in order to destroye them later
			for ( a = 0; a < allNewChunk.Count; a++ )
			{
				for ( b = 0; b < allNewChunk.Count; b++ )
				{
					if ( a != b )
					{
						allNewChunk [ a ].ThisSL.ToDest.Add ( allNewChunk [ b ].ThisObj );
					}
				}
			}

			// re calculate the order by lane parent
			for ( a = 0; a < getNewChunk.Count; a++ )
			{
				getCurrNew = getNewChunk [ a ].AllInfNewChunk;
				b = getCurrNew.Count;
				while ( b > 1)
				{
					b--;

					if ( getCurrNew [ b ].CurrLane < getCurrNew [ b - 1 ].CurrLane )
					{
						getOtherNC = getCurrNew [ b - 1 ];
						getCurrNew [ b - 1 ] = getCurrNew [ b ];
						getCurrNew [ b ] = getOtherNC;
						b = getCurrNew.Count;
					}
				}
			}

			// check the space between each chunks
			for ( a = 0; a < getNewChunk.Count; a++ )
			{
				getCurrNew = getNewChunk [ a ].AllInfNewChunk;
				if ( randChunk == 0 )
				{
					for ( b = 0; b < getCurrNew.Count - 1; b++ )
					{
						diffLine = ( int ) ( getCurrNew [ b ].NbrLaneDebut.y + getCurrNew [ b + 1 ].NbrLaneDebut.x - Mathf.Abs ( getCurrNew [ b + 1 ].CurrLane - getCurrNew [ b ].CurrLane ) );

						if ( diffLine >= 0 )
						{
							diffLine++;
							getCurrNew [ b + 1 ].ThisObj.transform.localPosition += new Vector3 ( Constants.LineDist * diffLine, 0, 0 );
							getCurrNew [ b + 1 ].CurrLane += diffLine;

							c = b;
							while ( c < getCurrNew.Count - 1 )
							{
								if ( getCurrNew [ c ].CurrLane > getCurrNew [ c + 1 ].CurrLane )
								{
									getOtherNC = getCurrNew [ c + 1 ];
									getCurrNew [ c + 1 ] = getCurrNew [ c ];
									getCurrNew [ c ] = getOtherNC;
									c = b;
								}

								c++;
							}

							b--;
						}
					}
				}
				else
				{
					for ( b = getCurrNew.Count - 1; b > 0; b-- )
					{
						diffLine = ( int ) ( getCurrNew [ b ].NbrLaneDebut.y + getCurrNew [ b - 1 ].NbrLaneDebut.x - Mathf.Abs ( getCurrNew [ b - 1 ].CurrLane - getCurrNew [ b ].CurrLane ) );

						if ( diffLine >= 0 )
						{
							diffLine++;
							getCurrNew [ b - 1 ].ThisObj.transform.localPosition -= new Vector3 ( Constants.LineDist * diffLine, 0, 0 );
							getCurrNew [ b - 1 ].CurrLane -= diffLine;

							c = b;
							while ( c > 0 )
							{
								if ( getCurrNew [ c ].CurrLane < getCurrNew [ c - 1 ].CurrLane )
								{
									getOtherNC = getCurrNew [ c - 1 ];
									getCurrNew [ c - 1 ] = getCurrNew [ c ];
									getCurrNew [ c ] = getOtherNC;
									c = b;
								}

								c--;
							}

							b++;
						}
					}
				}
			}

			for ( a = 0; a < getNewChunk.Count; a++ )
			{
				getCurrNew = getNewChunk [ a ].AllInfNewChunk;

				// check if there is spaces and place wall if yes
				for ( b = 0; b < getCurrNew.Count; b++ )
				{
					if ( getCurrNew [ b ].WallEndChunk != null )
					{
						currWall = getCurrNew [ b ].WallEndChunk;
					}
					else
					{
						currWall = getChunk.WallEndChunk;
					}

					if ( b == 0 )
					{
						diffLine = ( int ) ( getCurrNew [ b ].NbrLaneDebut.x + Mathf.Abs ( getCurrNew [ b ].CurrLane ) - sourceSpawn.ThoseExit [ a ].OtherNbrFin.x );
						while ( diffLine < 0 )
						{
							thisSpawn = ( GameObject ) Instantiate ( currWall, getCurrNew [ b ].ThisObj.transform );
							thisSpawn.transform.localPosition = new Vector3 ( Constants.LineDist * ( diffLine - getCurrNew [ b ].NbrLaneDebut.x ) - getChunk.SizeWall.x, 0, getChunk.SizeWall.y );
							thisSpawn.transform.localEulerAngles = new Vector3 ( 0, 90, 0 );
							diffLine++;
						}
					}

					if ( b == getNewChunk [ a ].AllInfNewChunk.Count - 1 )
					{
						diffLine = ( int ) ( getCurrNew [ b ].NbrLaneDebut.y + Mathf.Abs ( getCurrNew [ b ].CurrLane ) - sourceSpawn.ThoseExit [ a ].OtherNbrFin.x );
						diffLine = -diffLine;

						while ( diffLine > 0 )
						{
							thisSpawn = ( GameObject ) Instantiate ( currWall, getCurrNew [ b ].ThisObj.transform );
							thisSpawn.transform.localPosition = new Vector3 ( Constants.LineDist * ( diffLine + getCurrNew [ b ].NbrLaneDebut.y ) + getChunk.SizeWall.x, 0, getChunk.SizeWall.y );
							thisSpawn.transform.localEulerAngles = new Vector3 ( 0, 90, 0 );
							diffLine--;
						}
					}

					if ( b < getNewChunk [ a ].AllInfNewChunk.Count - 1 )
					{
						diffLine = ( int ) ( 1 + getCurrNew [ b ].NbrLaneDebut.y + Mathf.Abs ( getCurrNew [ b + 1 ].NbrLaneDebut.x ) - Mathf.Abs ( getCurrNew [ b + 1 ].CurrLane - getCurrNew [ b ].CurrLane ) );
						diffLine = -diffLine;

						while ( diffLine != 0 )
						{
							thisSpawn = ( GameObject ) Instantiate ( currWall, getCurrNew [ b ].ThisObj.transform );
							thisSpawn.transform.localPosition = new Vector3 ( Constants.LineDist * ( diffLine + getCurrNew [ b ].NbrLaneDebut.y ) + getChunk.SizeWall.x, 0, getChunk.SizeWall.y );
							thisSpawn.transform.localEulerAngles = new Vector3 ( 0, 90, 0 );

							if ( diffLine > 0 )
							{
								diffLine--;
							}
							else
							{
								diffLine++;
							}
						}
					}
				}
			}
		}
		else
		{
			List<GameObject> getSpc = getSpawnChunks;

			if ( getChunk.ChunkAleat )
			{
				currChunk = Random.Range ( 0, getChunk.TheseChunks.Count );
				thisSpawn = getChunk.TheseChunks [ currChunk ];
			}
			else
			{
				currChunk = currNbrCh;
				thisSpawn = getChunk.TheseChunks [ currNbrCh ];
			}

			if ( thisSpawn != null )
			{
				thisSpawn = ( GameObject ) Instantiate ( thisSpawn, thisT );

				getChunkT = thisSpawn.transform;
				getChunkT.rotation = Quaternion.identity;
				getChunkT.position = DefaultPos;

				getSpc.Add ( thisSpawn );
			}
		}

		spawnElements ( getSble.getCoinSpawnable, getChunk.CoinSpawnable );
		spawnElements ( getSble.getEnnemySpawnable, getChunk.EnnemySpawnable );
		spawnElements ( getSble.getObstacleSpawnable, getChunk.ObstacleSpawnable );
		spawnElements ( getSble.getObstacleDestrucSpawnable, getChunk.ObstacleDestrucSpawnable );

		currNbrCh++;

		/*
		GameObject getObj;
		NewChunkInfo getNew = thisSpawn.GetComponentInChildren<SpawnNewLvl> ( ).InfoChunk;
		int getNbr;
		int calNbr;

		if ( Random.Range ( 0, 2 ) == 0 )
		{
			getNbr = -Random.Range ( 0, ( int ) thisLvl.NbrLaneFin.x + 1 );
			thisPos = new Vector3 ( thisPos.x + Constants.LineDist * getNbr, thisPos.y, thisPos.z );
		}
		else
		{
			getNbr = Random.Range ( 0, ( int ) thisLvl.NbrLaneFin.y + 1 );
			thisPos = new Vector3 ( thisPos.x + Constants.LineDist * getNbr, thisPos.y, thisPos.z );
		}

		calNbr = ( int ) thisLvl.NbrLaneFin.x - getNbr;

		if ( getNew.NbrLaneDebut.x < calNbr )
		{
			for ( a = calNbr; a > getNew.NbrLaneDebut.x; a-- )
			{
				getObj = ( GameObject ) Instantiate ( getChunks [ currLevel ].WallEndChunk, thisLvl.transform );
				getObj.transform.position = new Vector3 ( thisPos.x + a * 6, thisPos.y, thisPos.z );
			}
		}

		calNbr = ( int ) thisLvl.NbrLaneFin.x + getNbr;
		if ( getNew.NbrLaneDebut.y > calNbr )
		{
			for ( a = calNbr; a < getNew.NbrLaneDebut.y; a++ )
			{
				Instantiate ( getChunks [ currLevel ].WallEndChunk, thisLvl.transform );
			}
		}*/
	}

	void spawnElements ( List<GameObject> spawnerElem, List<GameObject> elemSpawnable )
	{
		GameObject thisObj;
		int rand = thisChunk.PourcSpawn;

		int a;

		for ( a = 0; a < spawnerElem.Count; a++ )
		{
			if ( Random.Range ( 0, 100 ) <= rand )
			{
				thisObj = ( GameObject ) Instantiate ( elemSpawnable [ Random.Range ( 0, elemSpawnable.Count ) ], spawnerElem [ a ].transform );
				thisObj.transform.localScale = new Vector3 ( 1, 1, 1 );
				thisObj.transform.localPosition = Vector3.zero;
			}
		}
	}
	#endregion
}

public class ChunkCombineSpawnble 
{
	public List<ChunksScriptable> ChunkScript;
	public List<GetSpawnable> SpawnAble;
}

public class GetSpawnable
{
	public List<GameObject> getEnnemySpawnable;
	public List<GameObject> getObstacleSpawnable;
	public List<GameObject> getObstacleDestrucSpawnable;
	public List<GameObject> getCoinSpawnable;
}

public class VertNCSI 
{
	public List<NewChunkSaveInf> AllInfNewChunk;
	public int Vert;
}

public class NewChunkSaveInf 
{
	public GameObject ThisObj;
	public GameObject WallEndChunk;
	public Vector2 NbrLaneDebut;
	public Vector2 SizeWall;
	public SpawnNewLvl SpawnNL;
	public int CurrLane;
	public int CurrVert;
}

public class ToDestChunk 
{
	public GameObject ThisObj;
	public SpawnNewLvl ThisSL;
}