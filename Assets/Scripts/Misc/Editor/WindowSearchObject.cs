using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum ResearcheType 
{
	Object_Prefab,
	Component,
	Reference,
	MissingComp,
	Tag,
	Layer,
	Name
}

public enum TypePlace
{
	OnScene,
	OnProject,
	OnObject
}

public class WindowSearchObject : EditorWindow
{
	ResearcheType thisType;

	string thisStringSearch;
	string specName;
	int thisNbr;
	int compDiff;
	int childDiff;
	bool CanRemove;

	int nbrObjProj;
	int aPageProj;
	List<bool> foldoutProj;
	List <int> bPageProj;
	bool childProj;
	static bool endSearchProj;
	static int MaxCountProj;
	static int CurrCountProj;
	static int MaxAssetToLoad;
	static int NbrAssetLoad;

	int nbrObjScene;
	int aPageScene;
	List <int> bPageScene;
	List<bool> foldoutScene;
	bool childScene;
	static bool endSearchScene;
	static int MaxCountScene;
	static int CurrCountScene;

	int nbrObjPref;
	int aPagePref;
	List <int> bPagePref;
	List<bool> foldoutPref;
	bool childPref;
	static bool endSearchObj;
	static int MaxCountObj;
	static int CurrCountObj;

	bool getChildren;
	bool foldListPref;
	bool foldSamePref;
	bool getProper;
	//bool foldComp;
	bool apply;
	static bool assetLoading;
	//bool replace;

	Object SpecificPath;
	Object objComp;
	List<GameObject> thispref;
	Vector2 scrollPosProj;
	Vector2 scrollPosScene;
	Vector2 scrollPosPref;

	static Dictionary<TypePlace, List <EditorCoroutine>> getAllCorou;
	List<List<GameObject>> AllObjectProject;
	List<List<GameObject>> AllObjectScene;
	List<List<GameObject>> InfoOnPrefab;
	List<objectInfo> CompInfo;
	List<Object> saveForUndo;

	void OnEnable ()
	{
		thisType = ResearcheType.Tag;

		thisStringSearch = string.Empty;
		SpecificPath = null;
		specName = string.Empty;

		objComp = null;
		thisNbr = 0;
		getChildren = true;

		childProj = true;
		childScene = true;
		childPref = true;

		endSearchProj = true;
		endSearchScene = true;
		endSearchObj = true;

		foldListPref = true;
		getProper = false;
		CanRemove = false;

		//foldComp = false;
		apply = false;
		assetLoading = false;
		//replace = true;

		aPageProj = 0;
		aPageScene = 0;
		aPagePref = 0;
		compDiff = 2;
		childDiff = 5;

		nbrObjScene = 0;
		nbrObjScene = 0;
		nbrObjPref = 0;

		CurrCountScene = 0;
		CurrCountObj = 0;
		CurrCountProj = 0;
		MaxCountProj = 0;
		MaxCountScene = 0;
		MaxCountObj = 0;

		scrollPosProj = Vector2.zero;
		scrollPosScene = Vector2.zero;
		scrollPosPref = Vector2.zero;

		bPageProj = new List<int> ( );
		bPageScene = new List<int> ( );
		bPagePref = new List<int> ( );

		foldoutProj = new List<bool> ( );
		foldoutScene = new List<bool> ( );
		foldoutPref = new List<bool> ( );

		getAllCorou = new Dictionary<TypePlace, List<EditorCoroutine>> ( );
		AllObjectProject = new List<List<GameObject>> ( );
		AllObjectScene = new List<List<GameObject>> ( );
		InfoOnPrefab = new List<List<GameObject>> ( );
		thispref = new List<GameObject> ( );
		CompInfo = new List<objectInfo> ( );
		saveForUndo = new List<Object> ( );
	}
	// chercher ref de l'obje en scene / projet & faire une recherche de pref 
	[MenuItem("CustomTools/ToolSearch")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow ( typeof( WindowSearchObject ) );
	}

	void OnGUI()
	{
#region Research Config
		List<List<GameObject>> getAllOnProj = AllObjectProject;
		List<List<GameObject>> getAllOnScene = AllObjectScene;
		List<List<GameObject>> getAllOnPrefab = InfoOnPrefab;
		List<int> bProj = bPageProj;
		List<int> bScene = bPageScene;
		List<int> bPref = bPagePref;

		List<bool> fPref = foldoutPref;
		List<bool> fScene = foldoutScene;
		List<bool> fProj = foldoutProj;
		InfoResearch currResearch = new InfoResearch ( );

		int getPourcVal;
		int a; 
		float sizeX = position.width;
		GUILayout.Label ("Get object(s)", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical ( );
		EditorGUILayout.BeginHorizontal ( );

		EditorGUI.BeginChangeCheck ( );

		thisType = ( ResearcheType ) EditorGUILayout.EnumPopup ( "Research Type:", thisType, GUILayout.Width ( sizeX / 2 ) );

		var buttonStyle = new GUIStyle( EditorStyles.miniButton );

		if ( getChildren )
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		if ( EditorGUI.EndChangeCheck ( ) )
		{
			StopAll ( );
			getAllOnProj.Clear ( );
			getAllOnScene.Clear ( );
			getAllOnPrefab.Clear ( );
			objComp = null;
			apply = false;

			thisStringSearch = string.Empty;
			specName = string.Empty;
			thisNbr = 0;
			compDiff = 1;
			childDiff = 1;

			aPageProj = 0;
			aPageScene = 0;
			aPagePref = 0;

			nbrObjScene = 0;
			nbrObjScene = 0;
			nbrObjPref = 0;

			endSearchProj = true;
			endSearchObj = true;
			endSearchScene = true;

			MaxCountScene = 0;
			CurrCountScene = 0;

			MaxCountProj = 0;
			CurrCountProj = 0;

			MaxCountProj = 0;
			CurrCountProj = 0;
			assetLoading = false;
		}

		if ( GUILayout.Button ( "Search On Children", buttonStyle, GUILayout.Width ( sizeX / 6 ) ) )
		{
			getChildren = !getChildren;
		}
		EditorGUILayout.EndHorizontal ( );

		EditorGUI.indentLevel = 1;

		switch (thisType) 
		{
		case ResearcheType.Tag:
			thisStringSearch = EditorGUILayout.TagField ( "This Tag :", thisStringSearch, GUILayout.Width ( sizeX / 2 ) );
			break;
		case ResearcheType.Layer:
			thisNbr = EditorGUILayout.LayerField ( "This Num Layer :", thisNbr, GUILayout.Width ( sizeX / 2 ) );
			thisStringSearch = thisNbr.ToString ( );
			break;
		case ResearcheType.Name:
			thisStringSearch = EditorGUILayout.TextField ( "This Name :", thisStringSearch, GUILayout.Width ( sizeX / 2 ) );
			break;
		case ResearcheType.Component:
			objComp = EditorGUILayout.ObjectField ( "This component", objComp, typeof( Object ), true, GUILayout.Width ( sizeX / 2 ) );
			break;
		case ResearcheType.Reference:
			EditorGUILayout.BeginHorizontal ( );
			objComp = EditorGUILayout.ObjectField ( "This Object ref", objComp, typeof( Object ), true, GUILayout.Width ( sizeX / 2 ) );
			getProper = EditorGUILayout.Toggle ( "Search on Properties", getProper ); 
			EditorGUILayout.EndHorizontal ( );
			break;
		case ResearcheType.Object_Prefab:
			EditorGUILayout.BeginVertical ( );
			EditorGUI.BeginChangeCheck ( );

			objComp = EditorGUILayout.ObjectField ( "This Object", objComp, typeof( Object ), true, GUILayout.Width ( sizeX / 2 ) );

			if ( EditorGUI.EndChangeCheck ( ) )
			{
				apply = false;
				CompInfo = new List<objectInfo> ( );
				List<objectInfo> getCI = CompInfo;

				if ( objComp != null )
				{
					try{
						foreach ( GameObject thisObj in SearchObject.GetComponentsInChildrenOfAsset ( ( GameObject ) objComp ) )
						{
							getCI.Add ( new objectInfo ( ) );
							getCI [ getCI.Count - 1 ].ThisObj = thisObj;
							getCI [ getCI.Count - 1 ].thoseComp = thisObj.GetComponents<Component> ( );
						}
					}
					catch
					{
						Debug.LogError ( "Cannont search this" );
					}
				}
			}

			foldSamePref = EditorGUILayout.Foldout ( foldSamePref, "Advanced Filter" );

			if ( foldSamePref )
			{
				EditorGUILayout.BeginVertical ( );

				EditorGUI.indentLevel = 2;
				specName = EditorGUILayout.TextField ( "Other Name ?", specName, GUILayout.Width ( sizeX / 2 ) );

				compDiff = ( int ) EditorGUILayout.Slider ( "Max component gap", compDiff, 0, 10, GUILayout.Width ( sizeX / 2 ) );
				childDiff = ( int ) EditorGUILayout.Slider ( "Max child gap", childDiff, 0, 500, GUILayout.Width ( sizeX / 2 ) );
				EditorGUI.indentLevel = 0;

				EditorGUILayout.EndVertical ( );
			}
			EditorGUILayout.EndVertical ( );
			break;
		}
		EditorGUILayout.EndVertical ( );

		EditorGUILayout.Space ( );

		currResearch.StringSearch = thisStringSearch;
		currResearch.NbrCompDiff = compDiff;
		currResearch.NbrChildDiff = childDiff;
		currResearch.OtherName = specName;

		if ( SpecificPath != null )
		{
			currResearch.FolderProject = AssetDatabase.GetAssetOrScenePath( SpecificPath );
		}
#endregion

#region ActionResearch
		EditorGUILayout.BeginHorizontal ( );
		EditorGUILayout.BeginVertical ( GUILayout.Width ( sizeX / 3 ) );

		if ( GUILayout.Button ( "Object On Scene",  GUILayout.Height ( 25 ) ) )
		{
			StopPlace( TypePlace.OnScene );

			aPageScene = 0;
			nbrObjScene = 0;
			MaxCountScene = 0;
			CurrCountScene = 0;
				
			bPageScene = new List<int> ( );

			bScene = bPageScene;
			fScene = foldoutScene;
			childScene = getChildren;

			getAllOnScene.Clear ( );
			AllObjectScene = new List<List<GameObject>> ( );
			getAllOnScene = AllObjectScene;

			endSearchScene = false;
			EditorCoroutine.start ( SearchObject.LoadAssetOnScenes ( getAllOnScene, thisType, objComp, getChildren, currResearch ), TypePlace.OnScene );
		}

		if ( !endSearchScene )
		{
			getPourcVal = ( int ) ( ( ( float ) CurrCountScene / ( float ) MaxCountScene ) * 100 );
			GUILayout.HorizontalSlider( getPourcVal, 0, 100 );

			if ( getAllOnScene.Count == 0 && GUILayout.Button ( "Stop search on Scene", EditorStyles.miniButton ) )
			{
				StopPlace( TypePlace.OnScene );
			} 
		}
		EditorGUILayout.EndVertical ( );

		EditorGUILayout.BeginVertical ( GUILayout.Width ( sizeX / 3 ) );

		if ( GUILayout.Button ( "Object On Project", GUILayout.Height ( 25 ) ) )
		{
			StopPlace( TypePlace.OnProject );

			nbrObjProj = 0;
			aPageProj = 0;
			MaxCountProj = 0;
			CurrCountProj = 0;

			bPageProj = new List<int> ( );
			foldoutProj = new List<bool> ( );

			fProj = foldoutProj;
			bProj = bPageProj;
			childProj = getChildren;

			getAllOnProj.Clear ( );
			AllObjectProject = new List<List<GameObject>> ( );
			getAllOnProj = AllObjectProject;

			endSearchProj = false;

			EditorCoroutine.start  ( SearchObject.LoadAssetsInProject ( getAllOnProj, thisType, objComp, getChildren, currResearch ), TypePlace.OnProject );
		}

		SpecificPath = EditorGUILayout.ObjectField ("Specific folder :", SpecificPath, typeof( Object ), true, GUILayout.Width ( sizeX / 3.1f ));

		if ( !endSearchProj )
		{
			EditorGUILayout.BeginHorizontal ( GUILayout.Width ( sizeX / 3.1f )  );
			string getText;
			if ( assetLoading )
			{
				getText = "Loading Asset";
				getPourcVal = ( int ) ( ( ( float ) NbrAssetLoad / ( float ) MaxAssetToLoad ) * 100 );
			}
			else
			{
				getText = "Searching Object";
				getPourcVal = ( int ) ( ( ( float ) CurrCountProj / ( float ) MaxCountProj ) * 100 );
			}

			EditorGUILayout.PrefixLabel ( getText );
			GUILayout.HorizontalSlider( getPourcVal, 0, 100 );
			//EditorGUILayout.PrefixLabel ( (( int ) ( ( ( float ) CurrCountProj / ( float ) MaxCountProj ) * 100 )).ToString()  );
			EditorGUILayout.EndHorizontal ( );

			if ( getAllOnProj.Count == 0 && GUILayout.Button ( "Stop search on Project", EditorStyles.miniButton ) )
			{
				StopPlace( TypePlace.OnProject );
			} 
		}
		EditorGUILayout.EndVertical ( );

		EditorGUILayout.BeginVertical ( GUILayout.Width ( sizeX / 3.15f ) );
		if ( GUILayout.Button ( "On Object(s)",  GUILayout.Height ( 25 ) ) && thispref != null )
		{
			StopPlace( TypePlace.OnObject );

			nbrObjPref = 0;
			aPagePref = 0;
			MaxCountObj = 0;
			CurrCountObj = 0;

			bPagePref = new List<int> ( );

			bPref = bPagePref;
			fPref = foldoutPref;
			childPref = getChildren;

			getAllOnPrefab.Clear ( );

			endSearchObj = false;
			EditorCoroutine.start  ( SearchObject.LoadOnPrefab ( getAllOnPrefab, thisType, objComp, thispref, getChildren, currResearch ), TypePlace.OnObject );
		}

		if ( nbrObjProj < getAllOnProj.Count )
		{
			getAllOnProj = AllObjectProject;

			for ( a = nbrObjProj; a < getAllOnProj.Count; a++ )
			{
				bProj.Add ( 0 );
				fProj.Add ( false );
			}

			nbrObjProj = getAllOnProj.Count - 1;
		}

		if ( nbrObjScene < getAllOnScene.Count )
		{
			getAllOnScene = AllObjectScene;

			for ( a = nbrObjScene; a < getAllOnScene.Count; a++ )
			{
				bScene.Add ( 0 );
				fScene.Add ( false );
			}

			nbrObjScene = getAllOnScene.Count - 1;
		}

		if ( nbrObjPref < getAllOnPrefab.Count )
		{
			getAllOnPrefab = InfoOnPrefab;

			for ( a = nbrObjPref; a < getAllOnPrefab.Count; a++ )
			{
				bPref.Add ( 0 );
				fPref.Add ( false );
			}

			nbrObjPref = getAllOnPrefab.Count - 1;
		}

		var list = thispref;
		int newCount = Mathf.Max ( 0, EditorGUILayout.IntField ( "Number Ref", list.Count ) );
		while ( newCount < list.Count )
		{
			list.RemoveAt( list.Count - 1 );
		}

		while ( newCount > list.Count )
		{
			list.Add ( null );
		}
		EditorGUILayout.BeginVertical ( );
		if ( thispref.Count > 0 )
		{
			foldListPref = EditorGUILayout.Foldout ( foldListPref, "Object List" );
		}

		if ( foldListPref )
		{
			for( a = 0; a < thispref.Count; a++)
			{
				thispref [ a ] = ( GameObject ) EditorGUILayout.ObjectField ( "This component", thispref [ a ], typeof( GameObject ), true );
			}
		}
		EditorGUILayout.EndVertical ( );
		if ( !endSearchObj )
		{
			getPourcVal = ( int ) ( ( ( float ) CurrCountObj / ( float ) MaxCountObj ) * 100 );

			GUILayout.HorizontalSlider( getPourcVal, 0, 10 );
			if ( getAllOnPrefab.Count == 0 && GUILayout.Button ( "Stop search on Object", EditorStyles.miniButton ) )
			{
				StopPlace( TypePlace.OnObject );
			} 
		}
		EditorGUILayout.EndVertical ( );
		EditorGUILayout.EndHorizontal();
#endregion

#region AfterResearch
		EditorGUILayout.Space ( );
		EditorGUILayout.Space ( );

		if ( thisType == ResearcheType.Object_Prefab  )
		{
			if ( apply )
			{
				EditorGUILayout.BeginHorizontal ( );
				if ( GUILayout.Button ( "Confirm", EditorStyles.miniButton ) )
				{
					modifPref ( getAllOnScene );
					modifPref ( getAllOnProj );
					modifPref ( getAllOnPrefab );

					//Undo.RecordObjects(saveForUndo.ToArray(), "test");

					apply = false;
				}

				if ( GUILayout.Button ( "Cancel", EditorStyles.miniButton ) )
				{
					apply = false;
				}
				EditorGUILayout.EndHorizontal ( );
			}
			else
			{
				EditorGUILayout.BeginHorizontal( );

				if ( ( getAllOnScene.Count > 0 || getAllOnProj.Count > 0 || getAllOnPrefab.Count > 0 ) && CompInfo.Count > 0 && endSearchObj && endSearchProj && endSearchScene )
				{
					if ( GUILayout.Button ( "Apply Update", EditorStyles.miniButton ))
					{
						apply = true;
					}

					if ( getAllOnProj.Count > 0 || getAllOnPrefab.Count > 0 )
					{
						EditorGUILayout.PrefixLabel ( "Not Safe on Projet" );
					}
					//replace = EditorGUILayout.Toggle ( "Replace Object", replace ); 
				}

				/*EditorGUILayout.BeginVertical ( );
				EditorGUI.indentLevel = 2;
				if ( CompInfo.Count > 0 )
				{	
					foldComp = EditorGUILayout.Foldout ( foldComp, "Component List"  );
				}

				if ( foldComp )
				{
					for( a = 0; a < CompInfo.Count; a++)
					{
						EditorGUILayout.BeginHorizontal( );
						EditorGUILayout.PrefixLabel ( CompInfo [ a ].ThisObj.name );
						//.GetType ( ).ToString ( )
						if ( GUILayout.Button ( "Remove From Update", EditorStyles.miniButton ) )
						{
							CompInfo.RemoveAt ( a );
							a--;
						}
						EditorGUILayout.EndHorizontal( );

					}
				}
				EditorGUI.indentLevel = 0;

				EditorGUILayout.EndVertical ( );*/
				EditorGUILayout.EndHorizontal();
			}
		}

		EditorGUILayout.BeginHorizontal (  GUILayout.Width ( sizeX ));
#region Scene Layout
		EditorGUILayout.BeginVertical( );
		if ( getAllOnScene.Count > 0 )
		{

			if ( endSearchScene )
			{
				if ( GUILayout.Button ( "Clear Scene", EditorStyles.miniButton ) )
				{
					AllObjectScene = new List<List<GameObject>> ( );
				}
			}
			else if ( GUILayout.Button ( "Stop search on Scene", EditorStyles.miniButton ) )
			{
				StopPlace( TypePlace.OnScene );
			}
	
			scrollPosScene = EditorGUILayout.BeginScrollView ( scrollPosScene );
			aPageScene = LayoutSearch( getAllOnScene, bScene, fScene, aPageScene, childScene );

			EditorGUILayout.EndScrollView ( );
		}
		EditorGUILayout.EndVertical();
#endregion

#region Project layout
		EditorGUILayout.BeginVertical ( );
		if ( getAllOnProj.Count > 0 )
		{
			if ( endSearchProj )
			{
				if ( GUILayout.Button ( "Clear Project", EditorStyles.miniButton ) )
				{
					AllObjectProject = new List<List<GameObject>> ( );
				}
			}
			else if ( GUILayout.Button ( "Stop search on Project", EditorStyles.miniButton ) )
			{
				StopPlace( TypePlace.OnProject );
			} 

			scrollPosProj = EditorGUILayout.BeginScrollView ( scrollPosProj );
			aPageProj = LayoutSearch( getAllOnProj, bProj, fProj, aPageProj, childProj );

			EditorGUILayout.EndScrollView ( );
		}
		EditorGUILayout.EndVertical();
#endregion

#region Pref Layout
		EditorGUILayout.BeginVertical( );
		if ( getAllOnPrefab.Count > 0 )
		{
			if ( endSearchObj )
			{
				if ( GUILayout.Button ( "Clear Object", EditorStyles.miniButton ) )
				{
					InfoOnPrefab = new List<List<GameObject>> ( );
				}
			}
			else if ( GUILayout.Button ( "Stop search on Object", EditorStyles.miniButton ) )
			{
				StopPlace( TypePlace.OnObject );
			} 

			scrollPosPref = EditorGUILayout.BeginScrollView ( scrollPosPref );
			aPagePref = LayoutSearch( getAllOnPrefab, bPref, fPref, aPagePref, childPref );
			EditorGUILayout.EndScrollView ( );
		}
		EditorGUILayout.EndVertical();
#endregion
		EditorGUILayout.EndHorizontal();
#endregion
	}

	int LayoutSearch ( List<List<GameObject>> listSearch, List<int> bPage, List<bool> fDout, int aPage, bool ifChild )
	{
		float sizeX = position.width;

		int a = 0; 
		int b;
		int isParent = 0;
		bool getParent = false;
		bool allResearch = false;
		int getindentLevel = 0;
		Transform currParent;
		Transform bigParent;

		if ( sizeX > 750 )
		{
			if ( AllObjectProject.Count > 0 )
			{
				a++;
			}
			if ( InfoOnPrefab.Count > 0 )
			{
				a++;
			}
			if ( AllObjectScene.Count > 0 )
			{
				a++;
			}

			if ( sizeX / a < 750 )
			{
				allResearch = true;
			}
		}
		else
		{
			allResearch = true;
		}

		if ( listSearch.Count > 11 )
		{
			EditorGUILayout.BeginHorizontal ( );
			EditorGUILayout.PrefixLabel ( "Page Parent : " + ( listSearch.Count / 10 ).ToString ( ) );
			aPage = EditorGUILayout.IntSlider ( aPage, 0, listSearch.Count / 10 );
			EditorGUILayout.EndHorizontal ( );
		}

		for ( a = aPage * 10; a < 10 * ( aPage + 1 ); a++ )
		{
			if ( a >= listSearch.Count )
			{
				break;
			}

			EditorGUI.indentLevel = 0;
			if ( listSearch [ a ].Count == 0 )
			{
				listSearch.RemoveAt ( a );
				continue;
			}
			else if ( listSearch [ a ] [ 0 ] == null )
			{
				while ( listSearch [ a ].Count > 0 && listSearch [ a ] [ 0 ] == null )
				{
					listSearch [ a ].RemoveAt ( 0 );
				}
				continue;
			}

			currParent = listSearch [ a ] [ 0 ].transform;

			getParent = false;

			if ( ifChild && listSearch [ a ] [ 0 ].transform.parent == null )
			{
				isParent = 1;

				if ( allResearch )
				{
					EditorGUILayout.BeginVertical ( );
				}
				else
				{
					EditorGUILayout.BeginHorizontal();
				}

				EditorGUILayout.ObjectField ( listSearch [ a ] [ 0 ], typeof ( GameObject ), true );

				if ( CanRemove && GUILayout.Button ( "Remove this Liss", EditorStyles.miniButton ) )
				{
					listSearch.RemoveAt ( a );
					continue;
				}

				if ( allResearch )
				{
					EditorGUILayout.EndVertical ( );
				}
				else
				{
					EditorGUILayout.EndHorizontal ( );
				}

				getParent = true;
				if ( listSearch [ a ].Count > 1 )
				{
					EditorGUI.indentLevel = 1;
					fDout [ a ] = EditorGUILayout.Foldout ( fDout [ a ], "Display Children : " + ( listSearch [ a ].Count - 1 ).ToString ( ) );
				}

				EditorGUI.indentLevel = 2;
			}
			else
			{
				isParent = 0;
				fDout [ a ] = true;
			}

			if ( fDout [ a ] )
			{
				EditorGUILayout.BeginVertical ( );

				if ( listSearch[a].Count > 11 )
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel( "Page Child : " + (listSearch[a].Count / 10).ToString() );
					bPage[a] = EditorGUILayout.IntSlider ( bPage[a], 0, listSearch[a].Count / 10 );
					EditorGUILayout.EndHorizontal ( );
				}
				else
				{
					bPage[a] = 0;
				}

				if ( isParent == 0 && currParent.parent != null )
				{
					currParent = currParent.parent;
					getindentLevel = EditorGUI.indentLevel;
					EditorGUI.indentLevel = getindentLevel + 2;

					if ( allResearch )
					{
						EditorGUILayout.BeginVertical ( );
					}
					else
					{
						EditorGUILayout.BeginHorizontal();
					}

					EditorGUILayout.ObjectField ( "Parent", currParent.gameObject, typeof ( GameObject ), true );
					bigParent = currParent;

					while ( bigParent.parent != null )
					{
						bigParent = bigParent.parent;
					}

					if ( bigParent != currParent )
					{
						EditorGUILayout.ObjectField ( "Base Parent", bigParent.gameObject, typeof ( GameObject ), true );
					}

					if ( allResearch )
					{
						EditorGUILayout.EndVertical ( );
					}
					else
					{
						EditorGUILayout.EndHorizontal ( );
					}
					EditorGUI.indentLevel = getindentLevel;
				}

				for ( b = bPage[a] * 10 + isParent; b < 10 * ( bPage[a] + 1 ) + 1; b++ )
				{
					if ( b >= listSearch[a].Count)
					{
						break;
					}

					if ( listSearch [ a ].Count == 0 )
					{
						listSearch.RemoveAt ( a );
						break;
					}
					else if ( listSearch [ a ] [ b ] == null )
					{
						while ( listSearch [ a ].Count > 0 && listSearch [ a ] [ 0 ] == null )
						{
							listSearch [ a ].RemoveAt ( 0 );
						}
						continue;
					}

					EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.ObjectField ( listSearch [ a ] [ b ], typeof( GameObject ), true );

					if ( CanRemove && GUILayout.Button ( "Remove From List", EditorStyles.miniButton ) )
					{
						listSearch [ a ].RemoveAt ( b );
						continue;
					}

					EditorGUILayout.EndHorizontal ( );

					if ( !getParent && currParent != listSearch [ a ] [ b ].transform.parent && listSearch [ a ] [ b ].transform.parent != null )
					{
						if ( b == 0 || listSearch [ a ] [ b - 1 ].transform.parent != listSearch [ a ] [ b ].transform.parent )
						{
							getindentLevel = EditorGUI.indentLevel;
							EditorGUI.indentLevel = getindentLevel + 2;
							EditorGUILayout.Space ( );
							EditorGUILayout.Space ( );

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.ObjectField ( "OtherParent", listSearch [ a ] [ b ].transform.parent.gameObject, typeof( GameObject ), true );
							bigParent = currParent;

							while ( bigParent.parent != null )
							{
								bigParent = bigParent.parent;
							}

							if ( bigParent != currParent )
							{
								EditorGUILayout.ObjectField ( "Base Parent", bigParent.gameObject, typeof ( GameObject ), true );
							}

							EditorGUILayout.EndHorizontal ( );
							EditorGUI.indentLevel = getindentLevel;
						}
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.Space ( );
				}
				EditorGUILayout.EndVertical ( );
			}

			EditorGUILayout.Space ( );
			EditorGUILayout.Space ( );
		}

		return aPage;
	}

	void modifPref ( List<List<GameObject>> listSearch )
	{
		List<objectInfo> allComp;
		GameObject[] listChild;
		Component [] m_List;
		Quaternion getCurrRot;
		Transform allCompTrans;
		Transform listChildTrans;
		Vector3 getCurr;
		GameObject getNewObj;
		GameObject getInsParent;
		Transform getBasePart;
		List<InfoParent> parentUpdate = new List<InfoParent> ( );

		int a;
		int b;
		int c;
		int d;
		int e;

		bool checkChild;
		bool checkParent;

		string getAssetPath;

		allComp = CompInfo;

		GameObject thisObj = ( GameObject ) objComp;

		for ( a = 0; a < listSearch.Count; a++ )
		{
			for ( b = 0; b < listSearch [ a ].Count; b++ )
			{
				if ( !listSearch [ a ] [ b ].Equals ( thisObj ) )
				{
					saveForUndo.Add ( listSearch [ a ] [ b ] );

					if ( listSearch [ a ] [ b ] == null )
					{
						listSearch [ a ].RemoveAt ( b );
						b--;
						continue;
					}
					getBasePart = listSearch [ a ] [ b ].transform;

					while ( getBasePart.parent != null )
					{
						getBasePart = getBasePart.parent;
					}

					getAssetPath = AssetDatabase.GetAssetPath ( getBasePart.gameObject );

					getCurr = listSearch [ a ] [ b ].transform.localPosition;
					getCurrRot = listSearch [ a ] [ b ].transform.localRotation;

					if ( getAssetPath != null && getAssetPath != string.Empty )
					{
						getInsParent = null;

						for ( c = 0; c < parentUpdate.Count; c++ )
						{
							if ( parentUpdate [ c ].ThisParent == getBasePart )
							{
								getInsParent = parentUpdate [ c ].ThisObj;
								break;
							}
						}

						if ( getInsParent == null )
						{
							getInsParent = ( GameObject ) Instantiate ( getBasePart.gameObject );
							parentUpdate.Add ( new InfoParent ( ) );
							parentUpdate [ parentUpdate.Count - 1 ].ThisObj = getInsParent;
							parentUpdate [ parentUpdate.Count - 1 ].ThisParent = getBasePart;
						}

						foreach ( Transform currT in getInsParent.GetComponentsInChildren<Transform>( true) )
						{
							if ( currT.name == listSearch [ a ] [ b ].name )
							{
								getNewObj = ( GameObject ) Instantiate ( thisObj, currT.parent );
								getNewObj.name = thisObj.name;
								getNewObj.transform.localPosition = getCurr;
								getNewObj.transform.localRotation = getCurrRot;

								DestroyImmediate ( currT.gameObject, true );
								break;
							}
						}
					}
					else
					{
						getNewObj = ( GameObject ) Instantiate ( thisObj, listSearch [ a ] [ b ].transform.parent );
						DestroyImmediate ( listSearch [ a ] [ b ].gameObject, true );

						getNewObj.name = thisObj.name;
						getNewObj.transform.localPosition = getCurr;
						getNewObj.transform.localRotation = getCurrRot;
					}
				}
				// opti sur l'instanciation / destruction d'object : vérifier que le prochain obj n'as pas le meme parent sinon ne pas détruire, etc
				// donner une option qui permet à l'utilisateur de : tout remplacer / choisir les composants a mettre a jour / choisir les fields a mettre a jour

				/*if ( replace )
				{
					if ( !listSearch [ a ].Equals ( allComp ) )
					{
						Debug.Log ( AssetDatabase.GetAssetPath ( listSearch [ a ] [ b ] ) );
						getCurr = listSearch [ a ] [ b ].transform.localPosition;
						getCurrRot = listSearch [ a ] [ b ].transform.localRotation;

						if ( getAssetPath != null && getAssetPath != string.Empty )
						{
						}
						else
						{
							getNewObj = ( GameObject ) Instantiate ( thisObj, listSearch [ a ] [ b ].transform.parent );
							DestroyImmediate ( listSearch [ a ] [ b ].gameObject, true );
						}

						getNewObj.name = thisObj.name;
						getNewObj.transform.localPosition = getCurr;
						getNewObj.transform.localRotation = getCurrRot;
					}
				}*/
				/*else
				{
					listChild = SearchObject.GetComponentsInChildrenOfAsset ( listSearch [ a ] [ b ] );

					for ( c = 0; c < allComp.Count; c++ )
					{
						checkChild = false;
						for ( d = 0; d < listChild.Length; d++ )
						{
							checkParent = false;
							if ( listChild [ d ].name.Length >= allComp [ c ].ThisObj.name.Length && allComp [ c ].ThisObj.name == listChild [ d ].name.Substring ( 0, allComp [ c ].ThisObj.name.Length ) )
							{
								allCompTrans = allComp [ c ].ThisObj.transform;
								listChildTrans = listChild [ d ].transform;

								if ( allCompTrans.parent == null )
								{
									checkParent = true;
								}
								else if ( allCompTrans.parent != null && listChildTrans.parent != null )
								{
									if ( listChildTrans.parent.name.Length >= allCompTrans.parent.name.Length && allCompTrans.parent.name == listChildTrans.parent.name.Substring ( 0, allCompTrans.parent.name.Length ) )
									{
										checkParent = true;
									}
								}

								if ( checkParent )
								{
									checkChild = true;
									EditorUtility.SetDirty ( listChild [ d ] );

									getCurr = listChildTrans.localPosition;
									getCurrRot = listChildTrans.localRotation;

									m_List = listChildTrans.GetComponents<Component>();

									for ( e = 0; e < allComp [ c ].thoseComp.Length; e++ )
									{
										if ( listChildTrans.GetComponent ( allComp [ c ].thoseComp [ e ].GetType ( ) ) == null )
										{
											listChildTrans.gameObject.AddComponent ( allComp [ c ].thoseComp [ e ].GetType ( ) );
										}

										UnityEditorInternal.ComponentUtility.CopyComponent ( allComp [ c ].thoseComp [ e ] );
										UnityEditorInternal.ComponentUtility.PasteComponentValues ( listChild [ d ].GetComponent ( allComp [ c ].thoseComp [ e ].GetType ( ) ) );
									}

									for ( e = 0; e < m_List.Length; e++ )
									{
										if ( allComp [ c ].ThisObj.GetComponent ( m_List [ e ].GetType ( ) ) == null )
										{
											DestroyImmediate ( listChild [ d ].GetComponent ( m_List [ e ].GetType ( ) ), true );
										}
									}

									listChildTrans.localPosition = getCurr;
									listChildTrans.localRotation = getCurrRot;
									break;
								}
							}
						}

						if ( !checkChild )
						{
							getCurr = allComp [ c ].ThisObj.transform.localPosition;
							getCurrRot = allComp [ c ].ThisObj.transform.localRotation;

							getNewObj = ( GameObject ) Instantiate ( allComp [ c ].ThisObj, listSearch [ a ] [ b ].transform );

							getNewObj.name = allComp [ c ].ThisObj.name;
							getNewObj.transform.localPosition = getCurr;
							getNewObj.transform.localRotation = getCurrRot;
							listChild = SearchObject.GetComponentsInChildrenOfAsset ( listSearch [ a ] [ b ] );
						}
					}
				}*/
			}
		}

		for ( a = 0; a < parentUpdate.Count; a++ )
		{
			PrefabUtility.ReplacePrefab ( parentUpdate [ a ].ThisObj, parentUpdate [ a ].ThisParent.gameObject, ReplacePrefabOptions.ReplaceNameBased );
			DestroyImmediate ( parentUpdate [ a ].ThisObj, true );
		}
	}

	public static void SetCorout ( EditorCoroutine thisCorou, TypePlace thisPlace )
	{
		List <EditorCoroutine> getList;
		if ( getAllCorou.TryGetValue ( thisPlace, out getList ) )
		{
			getList.Add ( thisCorou );
		}
		else
		{
			getList = new List<EditorCoroutine> ( );
			getList.Add ( thisCorou );
			getAllCorou.Add ( thisPlace, getList );
		}
	}
		
	public static void AddCount ( TypePlace thisPlace )
	{
		switch ( thisPlace )
		{
		case TypePlace.OnProject:
			CurrCountProj++;
			if ( CurrCountProj == MaxCountProj )
			{
				EndResearch ( thisPlace );
			}
			break;
		case TypePlace.OnScene:
			CurrCountScene++;
			if ( CurrCountScene == MaxCountScene )
			{
				EndResearch ( thisPlace );
			}
			break;
		case TypePlace.OnObject:
			CurrCountObj++;
			if ( CurrCountObj == MaxCountObj )
			{
				EndResearch ( thisPlace );
			}
			break;
		}
	}

	public static void AssetLoading ( bool LoadAsset, int maxToLoad )
	{
		NbrAssetLoad = 0;
		MaxAssetToLoad = maxToLoad;
		assetLoading = LoadAsset;
	}

	public static void PlusAssetLoaded ( )
	{
		NbrAssetLoad++;
	}

	public static void MaxCount ( int maxNbr, TypePlace thisPlace )
	{
		switch ( thisPlace )
		{
		case TypePlace.OnProject:
			MaxCountProj = maxNbr;
			break;
		case TypePlace.OnScene:
			MaxCountScene = maxNbr;
			break;
		case TypePlace.OnObject:
			MaxCountObj = maxNbr;
			break;
		}
	}

	public static void DeleteCorout ( EditorCoroutine thisCorou, TypePlace thisPlace )
	{
		List <EditorCoroutine> getList;
		if ( getAllCorou.TryGetValue ( thisPlace, out getList ) )
		{
			getList.Remove( thisCorou );
		}
	}

	public static void StopPlace ( TypePlace thisPlace )
	{
		List <EditorCoroutine> getList;
		if ( getAllCorou.TryGetValue ( thisPlace, out getList ) )
		{
			for ( int a = 0; a < getList.Count; a++ )
			{
				getList [ a ].stop ( );
			}
		}

		EndResearch ( thisPlace );
	}

	public static void EndResearch ( TypePlace thisPlace )
	{
		switch ( thisPlace )
		{
		case TypePlace.OnProject:
			CurrCountProj = 0;
			MaxCountProj = 0;
			endSearchProj = true;
			break;
		case TypePlace.OnScene:
			MaxCountScene = 0;
			CurrCountScene = 0;
			endSearchScene = true;
			break;
		case TypePlace.OnObject:
			MaxCountObj = 0;
			CurrCountObj = 0;
			endSearchObj = true;
			break;
		}
	}

	public void StopAll ( )
	{
		foreach ( List<EditorCoroutine> allCorou in getAllCorou.Values )
		{
			for ( int a = 0; a < allCorou.Count; a++ )
			{
				allCorou [ a ].stop ( );
			}
		}
	}
}

public class objectInfo
{
	public GameObject ThisObj;
	public Component[] thoseComp;
}

public class InfoParent 
{
	public Transform ThisParent;
	public GameObject ThisObj;
}

public class InfoResearch 
{
	public string FolderProject = "";
	public string StringSearch = "";
	public int NbrCompDiff = 2;
	public int NbrChildDiff = 2;
	public string OtherName = "";
	public bool TryGetProperty = false;
}

public class EditorCoroutine
{
	public static EditorCoroutine start( IEnumerator _routine, TypePlace thisPlace )
	{
		ThisPlace = thisPlace;
		EditorCoroutine coroutine = new EditorCoroutine(_routine);
		WindowSearchObject.SetCorout ( coroutine, thisPlace );
		thisEdit = coroutine;
		coroutine.start();
		return coroutine;
	}

	static TypePlace ThisPlace;
	static EditorCoroutine thisEdit;
	readonly IEnumerator routine;

	EditorCoroutine( IEnumerator _routine )
	{
		routine = _routine;
	}

	void start()
	{
		EditorApplication.update += update;
	}

	public void stop()
	{
		EditorApplication.update -= update;
	}

	void update()
	{
		if ( !routine.MoveNext ( ) )
		{
			WindowSearchObject.DeleteCorout ( thisEdit, ThisPlace );

			stop ( );
		}
	}
}