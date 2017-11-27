using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SearchObject : MonoBehaviour 
{
	#region Variables

	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public static IEnumerator LoadAssetsInProject ( List<List<GameObject>> objectList, ResearcheType thisType, Object objComp, bool getChildren, InfoResearch thisResearch )
	{
		List<GameObject> asset = new List<GameObject> ( ); 

		TypePlace currPlace = TypePlace.OnProject;

		string guid;
		string assetPath;

		string[] GUIDs;
		string optionalPath = thisResearch.FolderProject;

		int a;

		if(optionalPath != "")
		{
			if(optionalPath.EndsWith("/"))
			{
				optionalPath = optionalPath.TrimEnd('/');
			}
			GUIDs = AssetDatabase.FindAssets("t:GameObject",new string[] { optionalPath });
		}
		else
		{
			GUIDs = AssetDatabase.FindAssets("t:GameObject");
		}

		for ( a = 0; a < GUIDs.Length; a++ )
		{
			guid = GUIDs [ a ];
			assetPath = AssetDatabase.GUIDToAssetPath ( guid );
			asset.Add ( AssetDatabase.LoadAssetAtPath ( assetPath, typeof( GameObject ) ) as GameObject );
		}

		if ( getChildren )
		{
			int countMax = 0;
			for ( a = 0; a < asset.Count; a++ )
			{
				countMax += GetComponentsInChildrenOfAsset ( asset [ a ] ).Length;
			}

			WindowSearchObject.MaxCount ( countMax, currPlace );

			for ( a = 0; a < asset.Count; a++ )
			{
				EditorCoroutine.start ( returnCurrObj ( currPlace, objectList, GetComponentsInChildrenOfAsset ( asset [ a ] ), thisType, objComp, thisResearch ), currPlace );
			}
		}
		else
		{
			EditorCoroutine.start ( returnCurrObj ( currPlace, objectList, asset.ToArray ( ), thisType, objComp, thisResearch ), currPlace );
		}

		yield return new WaitForEndOfFrame ( );
	}

	public static IEnumerator LoadAssetOnScenes ( List<List<GameObject>> getAllObj, ResearcheType thisType, Object objComp, bool getChildren, InfoResearch thisResearch )
	{
		GameObject[] objectList = UnityEngine.SceneManagement.SceneManager.GetActiveScene ( ).GetRootGameObjects ( );
		TypePlace currPlace = TypePlace.OnScene;

		if ( getChildren )
		{
			int a;
			int countMax = 0;
			for ( a = 0; a < objectList.Length; a++ )
			{
				countMax += GetComponentsInChildrenOfAsset ( objectList [ a ] ).Length;
			}

			WindowSearchObject.MaxCount ( countMax, currPlace );

			for ( a = 0; a < objectList.Length; a ++ )
			{
				EditorCoroutine.start ( returnCurrObj ( currPlace, getAllObj, GetComponentsInChildrenOfAsset ( objectList [ a ] ), thisType, objComp, thisResearch ), currPlace );
			}
		}
		else
		{
			EditorCoroutine.start ( returnCurrObj ( currPlace, getAllObj, objectList, thisType, objComp, thisResearch ), currPlace );
		}

		yield return new WaitForEndOfFrame ( );
	}

	public static IEnumerator LoadOnPrefab ( List<List<GameObject>> objectList, ResearcheType thisType, Object objComp, List<GameObject> thisPref, bool getChildren, InfoResearch thisResearch )
	{
		TypePlace currPlace = TypePlace.OnObject;
		int a;

		if ( getChildren )
		{
			int countMax = 0;
			for ( a = 0; a < thisPref.Count; a++ )
			{
				countMax += GetComponentsInChildrenOfAsset ( thisPref [ a ] ).Length;
			}

			WindowSearchObject.MaxCount ( countMax, currPlace );

			for ( a = 0; a < thisPref.Count; a++ )
			{
				EditorCoroutine.start ( returnCurrObj ( currPlace, objectList, GetComponentsInChildrenOfAsset ( thisPref [ a ] ), thisType, objComp, thisResearch ), currPlace );
			}
		}
		else 
		{
			EditorCoroutine.start ( returnCurrObj ( currPlace, objectList, thisPref.ToArray ( ), thisType, objComp, thisResearch ), currPlace );
		}

		yield return new WaitForEndOfFrame ( );
	}
	#endregion

	#region Private Methods
	static IEnumerator returnCurrObj ( TypePlace thisPlace, List<List<GameObject>> CurrList, GameObject[] objectList, ResearcheType thisType, Object objComp, InfoResearch thisResearch )
	{
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );

		List<GameObject> objTagList = new List<GameObject>();
		Component [] components;
		Component [] componentsPref;

		GameObject getPref;

		string thisStringSearch = thisResearch.StringSearch;
		int diffComp = thisResearch.NbrCompDiff;
		int diffChil = thisResearch.NbrChildDiff;
		string OtherName = thisResearch.OtherName;
		bool getProper = false;

		if ( thisType == ResearcheType.SamePref )
		{
			if ( objComp == null )
			{
				WindowSearchObject.CancelResearch ( );

				yield break;
			}

			getProper = thisResearch.TryGetProperty;
			getPref = ( GameObject ) objComp;
			componentsPref = getPref.GetComponents<Component> ( );
		}
		else
		{
			getPref = objectList [ 0 ];
			componentsPref = getPref.GetComponents<Component> ( );
		}

		string getSearch = thisStringSearch;

		int a;
		int b;
		int maxNbr = objectList.Length;
		for ( a = 0; a < objectList.Length; a++ )
		{
			WindowSearchObject.AddCount ( thisPlace );
			yield return thisF;

			if ( objectList [ a ] == null )
			{
				continue;
			}
			switch (thisType) 
			{
			case ResearcheType.Tag:
				if ( getSearch == string.Empty || objectList[a].tag == getSearch )
				{
					objTagList.Add ( objectList [ a ] );
				}
				break;
			case ResearcheType.Name:
				if ( objectList[a].name == getSearch )
				{
					objTagList.Add ( objectList [ a ] );
				}
				break;
			case ResearcheType.Layer:
				if ( objectList[a].layer == int.Parse ( getSearch ) )
				{
					objTagList.Add ( objectList [ a ] );
				}
				break;
			case ResearcheType.Component:
				components = objectList[a].GetComponents<Component> ( );

				if ( objComp == null )
				{
					WindowSearchObject.CancelResearch ( );

					yield break;
				}

				for ( b = 0; b < components.Length; b++ )
				{
					if ( components [ b ] != null && components [ b ].GetType ( ) == objComp.GetType ( )  )
					{
						objTagList.Add ( objectList[a] );
						break;
					}
				}
				break;
			case ResearcheType.MissingComp :
				components = objectList[a].GetComponents<Component> ( );

				for ( b = 0; b < components.Length; b++ )
				{
					if ( components [ b ] == null )
					{
						objTagList.Add ( objectList[a] );
						break;
					}
				}
				break;
			case ResearcheType.SearchRef:
				components = objectList [ a ].GetComponents<Component> ( );

				for ( b = 0; b < components.Length; b++ )
				{
					if ( components [ b ] == null )
					{
						continue;
					}

					yield return thisF;

					if ( components [ b ].GetType ( ).GetFields ( ).Length > 0 )
					{
						foreach ( var field in components[b].GetType ( ).GetFields ( ) )
						{
							yield return thisF;

							try 
							{
								if ( field.GetValue ( components [ b ] ) == objComp )
								{
									objTagList.Add ( objectList [ a ] );
									break;
								}
							}
							catch{
							}
						}
					}
					else if ( getProper )
					{
						foreach ( var field in components[b].GetType ( ).GetProperties ( ) )
						{
							yield return thisF;

							try 
							{
								if ( field.GetValue ( components [ b ], null ) == objComp )
								{
									objTagList.Add ( objectList [ a ] );
									break;
								}
							}
							catch
							{
							}
						}
					}
				}
				break;
			case ResearcheType.SamePref:
				components = objectList [ a ].GetComponents<Component> ( );

				if ( objectList [ a ].Equals ( getPref ) )
				{
					break;
				}

				if ( Mathf.Abs ( componentsPref.Length - components.Length ) <= diffComp && Mathf.Abs ( GetComponentsInChildrenOfAsset( getPref ).Length - GetComponentsInChildrenOfAsset ( objectList [ a ] ).Length ) <= diffChil )
				{
					if ( OtherName != "" )
					{
						if ( objectList [ a ].name.Length >= OtherName.Length && objectList [ a ].name.Substring ( 0, OtherName.Length ) == OtherName )
						{
							objTagList.Add ( objectList [ a ] );
						}
					}
					else if ( objectList [ a ].name.Length >= getPref.name.Length && objectList [ a ].name.Substring ( 0, getPref.name.Length ) == getPref.name )
					{
						objTagList.Add ( objectList [ a ] );
					}
				}
				break;
			}
		}

		if ( objTagList.Count > 0 )
		{
			CurrList.Add ( objTagList );
		}

		yield return thisF;
	}

	public static GameObject[] GetComponentsInChildrenOfAsset( GameObject go  )
	{
		List<GameObject> tfs = new List<GameObject>();

		CollectChildren( tfs, go.transform );
		return tfs.ToArray();
	}

	static void CollectChildren( List<GameObject> transforms, Transform tf)
	{
		transforms.Add ( tf.gameObject );

		foreach(Transform child in tf)
		{
			CollectChildren ( transforms, child );
		}
	}
	#endregion
}
