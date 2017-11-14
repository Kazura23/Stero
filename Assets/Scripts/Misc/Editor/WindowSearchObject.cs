﻿using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WindowSearchObject : EditorWindow
{
	ResearcheType thisType;

	string thisStringSearch;
	string SpecificPath;
	int thisNbr;
	int compDiff;

	int aPageProj;
	List<bool> foldoutProj;
	List <int> bPageProj;
	bool childProj;

	int aPageScene;
	List <int> bPageScene;
	List<bool> foldoutScene;
	bool childScene;

	int aPagePref;
	List <int> bPagePref;
	List<bool> foldoutPref;
	bool childPref;

	bool getChildren;
	bool foldListPref;
	bool foldComp;
	bool apply;

	Object objComp;
	List<GameObject> thispref;
	Vector2 scrollPosProj;
	Vector2 scrollPosScene;
	Vector2 scrollPosPref;

	List<List<GameObject>> AllObjectProject;
	List<List<GameObject>> AllObjectScene;
	List<List<GameObject>> InfoOnPrefab;
	List<objectInfo> CompInfo;
	void OnEnable ()
	{
		thisType = ResearcheType.Tag;

		thisStringSearch = string.Empty;
		SpecificPath = string.Empty;

		objComp = null;
		thisNbr = 0;
		getChildren = true;

		childProj = true;
		childScene = true;
		childPref = true;
		foldListPref = true;
		foldComp = false;
		apply = false;

		aPageProj = 0;
		aPageScene = 0;
		aPagePref = 0;
		compDiff = 2;

		scrollPosProj = Vector2.zero;
		scrollPosScene = Vector2.zero;
		scrollPosPref = Vector2.zero;

		bPageProj = new List<int> ( );
		bPageScene = new List<int> ( );
		bPagePref = new List<int> ( );

		foldoutProj = new List<bool> ( );
		foldoutScene = new List<bool> ( );
		foldoutPref = new List<bool> ( );

		AllObjectProject = new List<List<GameObject>> ( );
		AllObjectScene = new List<List<GameObject>> ( );
		InfoOnPrefab = new List<List<GameObject>> ( );
		thispref = new List<GameObject> ( );
		CompInfo = new List<objectInfo> ( );
	}
	// chercher ref de l'obje en scene / projet & faire une recherche de pref 
	[MenuItem("CustomTools/SearchTags")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(WindowSearchObject));
	}

	void OnGUI()
	{
		List<List<GameObject>> getAllOnProj = AllObjectProject;
		List<List<GameObject>> getAllOnScene = AllObjectScene;
		List<List<GameObject>> getAllOnPrefab = InfoOnPrefab;
		List<int> bProj = bPageProj;
		List<int> bScene = bPageScene;
		List<int> bPref = bPagePref;

		List<bool> fPref = foldoutPref;
		List<bool> fScene = foldoutScene;
		List<bool> fProj = foldoutProj;

		int a; 
		GUILayout.Label ("Get Specific object", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();
		EditorGUI.BeginChangeCheck ( );
		thisType = (ResearcheType)EditorGUILayout.EnumPopup("Research Type:", thisType);
		if ( EditorGUI.EndChangeCheck ( ) )
		{
			AllObjectScene = new List<List<GameObject>> ( );
			AllObjectProject = new List<List<GameObject>> ( );
			InfoOnPrefab = new List<List<GameObject>> ( );
			objComp = null;
			thisStringSearch = string.Empty;
			thisNbr = 0;
			compDiff = 2;
			apply = false;
		}

		switch (thisType) 
		{
		case ResearcheType.Tag:
			thisStringSearch = EditorGUILayout.TagField ( "Search This Tag :", thisStringSearch );
			break;
		case ResearcheType.Layer:
			thisNbr = EditorGUILayout.LayerField ( "Search This Number Layer :", thisNbr );
			thisStringSearch = thisNbr.ToString ( );
			break;
		case ResearcheType.Name:
			thisStringSearch = EditorGUILayout.TextField ( "Search This Name :", thisStringSearch );
			break;
		case ResearcheType.Component:
			objComp = EditorGUILayout.ObjectField ( "This component", objComp, typeof( Object ), true );
			break;
		case ResearcheType.SamePref:
			EditorGUILayout.BeginVertical ( );
			EditorGUI.BeginChangeCheck ( );
			objComp = EditorGUILayout.ObjectField ( "This Object", objComp, typeof( Object ), true );

			if ( EditorGUI.EndChangeCheck ( ) )
			{
				apply = false;
				CompInfo = new List<objectInfo> ( );
				List<objectInfo> getCI = CompInfo;

				if ( objComp != null )
				{
					foreach ( GameObject thisObj in SearchObject.GetComponentsInChildrenOfAsset ( ( GameObject ) objComp ) )
					{
						getCI.Add ( new objectInfo ( ) );
						getCI [ getCI.Count - 1 ].ThisObj = thisObj;
						getCI[ getCI.Count - 1 ].thoseComp = thisObj.GetComponents<Component> ( );
					}
				}
			}

			compDiff = (int) EditorGUILayout.Slider ( "Max Number Component Different" ,compDiff, 0, 10 );

			EditorGUILayout.EndVertical ( );

			break;
		}

		var buttonStyle = new GUIStyle( EditorStyles.miniButton );

		if ( getChildren )
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		if ( GUILayout.Button ( "Search On Children", buttonStyle ) )
		{
			getChildren = !getChildren;
		}
		EditorGUILayout.EndHorizontal();

		if ( GUILayout.Button ( "Object On Scene" ) )
		{
			aPageScene = 0;
			bPageScene = new List<int> ( );
			bScene = bPageScene;
			fScene = foldoutScene;
			childScene = getChildren;

			AllObjectScene = SearchObject.LoadAssetOnScenes ( thisType, objComp, thisStringSearch, getChildren, compDiff );
			getAllOnScene = AllObjectScene;

			for ( a = 0; a < getAllOnScene.Count; a++ )
			{
				bScene.Add ( 0 );
				fScene.Add ( false );
			}
		}

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "Object On Object" ) )
		{
			bPageProj = new List<int> ( );
			foldoutProj = new List<bool> ( );
			fProj = foldoutProj;
			bProj = bPageProj;

			aPageProj = 0;
			childProj = getChildren;

			AllObjectProject = SearchObject.LoadAssetsInProject ( thisType, objComp, thisStringSearch, getChildren, SpecificPath, compDiff );
			getAllOnProj = AllObjectProject;

			for ( a = 0; a < getAllOnProj.Count; a++ )
			{
				bProj.Add ( 0 );
				fProj.Add ( false );
			}
		}

		SpecificPath = EditorGUILayout.TextField ("On Specific folder :", SpecificPath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "Object On Prefabs" ) && thispref != null )
		{
			aPagePref = 0;
			bPagePref = new List<int> ( );
			bPref = bPagePref;
			fPref = foldoutPref;
			childPref = getChildren;

			InfoOnPrefab = SearchObject.LoadOnPrefab ( thisType, objComp, thispref, thisStringSearch, getChildren, compDiff );
			getAllOnPrefab = InfoOnPrefab;

			for ( a = 0; a < getAllOnPrefab.Count; a++ )
			{
				bPref.Add ( 0 );
				fPref.Add ( false );
			}
		}
			
		var list = thispref;
		int newCount = Mathf.Max(0, EditorGUILayout.IntField("Number Ref", list.Count));
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
			foldListPref = EditorGUILayout.Foldout ( foldListPref, "Object List"  );
		}

		if ( foldListPref )
		{
			for( a = 0; a < thispref.Count; a++)
			{
				thispref [ a ] = ( GameObject ) EditorGUILayout.ObjectField ( "This component", thispref [ a ], typeof( GameObject ), true );
			}
		}
		EditorGUILayout.EndVertical ( );
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ( );
		EditorGUILayout.Space ( );

		if ( thisType == ResearcheType.SamePref  )
		{
			if ( apply )
			{
				EditorGUILayout.BeginHorizontal ( );
				if ( GUILayout.Button ( "Confirm", EditorStyles.miniButton ) )
				{
					modifPref ( getAllOnScene );
					modifPref ( getAllOnProj );
					modifPref ( getAllOnPrefab );

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
				if ( ( getAllOnScene.Count > 0 || getAllOnProj.Count > 0 || getAllOnPrefab.Count > 0 ) && CompInfo.Count > 0 && GUILayout.Button ( "Apply Update", EditorStyles.miniButton ) )
				{
					apply = true;
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

		EditorGUILayout.BeginHorizontal ( );
		#region Scene Layout
		if ( getAllOnScene.Count > 0 )
		{
			scrollPosScene = EditorGUILayout.BeginScrollView ( scrollPosScene );

			if ( GUILayout.Button ( "Clear Scene", EditorStyles.miniButton ) )
			{
				AllObjectScene = new List<List<GameObject>> ( );
			}

			aPageScene = LayoutSearch( getAllOnScene, bScene, fScene, aPageScene, childScene );
			EditorGUILayout.EndScrollView ( );
		}
		#endregion

		#region Project layout
		if ( getAllOnProj.Count > 0 )
		{
			scrollPosProj = EditorGUILayout.BeginScrollView ( scrollPosProj );

			if ( GUILayout.Button ( "Clear Project", EditorStyles.miniButton ) )
			{
				AllObjectProject = new List<List<GameObject>> ( );
			}

			aPageProj = LayoutSearch( getAllOnProj, bProj, fProj, aPageProj, childProj );
			EditorGUILayout.EndScrollView ( );
		}
		#endregion

		#region Pref Layout
		if ( getAllOnPrefab.Count > 0 )
		{
			scrollPosPref = EditorGUILayout.BeginScrollView ( scrollPosPref );

			if ( GUILayout.Button ( "Clear Pref", EditorStyles.miniButton ) )
			{
				InfoOnPrefab = new List<List<GameObject>> ( );
			}

			aPagePref = LayoutSearch( getAllOnPrefab, bPref, fPref, aPagePref, childPref );
			EditorGUILayout.EndScrollView ( );
		}

		#endregion
		EditorGUILayout.EndHorizontal();
	}

	int LayoutSearch ( List<List<GameObject>> listSearch, List<int> bPage, List<bool> fDout, int aPage, bool ifChild )
	{
		int a; 
		int b;
		int isParent = 0;
		bool getParent = false;
		int getindentLevel = 0;
		Transform currParent;

		if ( listSearch.Count > 11 )
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel( "Page Parent : " + (listSearch.Count / 10).ToString() );
			aPage = EditorGUILayout.IntSlider ( aPage, 0, listSearch.Count / 10 );
			EditorGUILayout.EndHorizontal();
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
				EditorGUILayout.ObjectField ( listSearch [ a ] [ 0 ], typeof ( GameObject ), true );

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

				if ( isParent == 0 )
				{
					currParent = currParent.parent;
					getindentLevel = EditorGUI.indentLevel;
					EditorGUI.indentLevel = getindentLevel + 2;

					EditorGUILayout.ObjectField ( "Parent", currParent.gameObject, typeof ( GameObject ), true );
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

					if ( thisType == ResearcheType.SamePref && GUILayout.Button ( "Remove From List", EditorStyles.miniButton ) )
					{
						listSearch [ a ].RemoveAt ( b );
					}

					EditorGUILayout.EndHorizontal ( );

					if ( !getParent && currParent != listSearch [ a ] [ b ].transform.parent )
					{
						if ( b == 0 || listSearch [ a ] [ b - 1 ].transform.parent != listSearch [ a ] [ b ].transform.parent )
						{
							if ( listSearch [ a ] [ b - 1 ].transform.parent != listSearch [ a ] [ b ].transform.parent )
							{
								getindentLevel = EditorGUI.indentLevel;
								EditorGUI.indentLevel = getindentLevel + 2;
								EditorGUILayout.Space ( );
								EditorGUILayout.Space ( );

								EditorGUILayout.ObjectField ( "OtherParent", listSearch [ a ] [ b ].transform.parent.gameObject, typeof( GameObject ), true );
								EditorGUI.indentLevel = getindentLevel;
							}
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
		Vector3 getCurr;
		GameObject getNewObj;

		int a;
		int b;
		int c;
		int d;
		int e;

		bool checkChild;

		allComp = CompInfo;

		for ( a = 0; a < listSearch.Count; a++ )
		{
			for ( b = 0; b < listSearch [ a ].Count; b++ )
			{
				listChild = SearchObject.GetComponentsInChildrenOfAsset ( listSearch [ a ] [ b ] );

				for ( c = 0; c < allComp.Count; c++ )
				{
					checkChild = false;
					for ( d = 0; d < listChild.Length; d++ )
					{
						if ( allComp [ c ].ThisObj.name == listChild [ d ].name && ( ( allComp [ c ].ThisObj.transform.parent == null && listChild [ d ].transform.parent == null ) || allComp [ c ].ThisObj.transform.parent.name == listChild [ d ].transform.parent.name ) )
						{
							checkChild = true;
							EditorUtility.SetDirty ( listChild [ d ] );

							getCurr = listChild [ d ].transform.localPosition;
							getCurrRot = listChild [ d ].transform.localRotation;

							m_List = listChild [ d ].GetComponents<Component>();

							for ( e = 0; e < allComp [ c ].thoseComp.Length; e++ )
							{
								if ( listChild [ d ].GetComponent ( allComp [ c ].thoseComp [ e ].GetType ( ) ) == null )
								{
									listChild [ d ].gameObject.AddComponent ( allComp [ c ].thoseComp [ e ].GetType ( ) );
								}

								UnityEditorInternal.ComponentUtility.CopyComponent ( allComp [ c ].thoseComp [ e ] );
								UnityEditorInternal.ComponentUtility.PasteComponentValues ( listChild [ d ].GetComponent ( allComp [ c ].thoseComp [ e ].GetType ( ) ) );
							}

							for ( e = 0; e < m_List.Length; e++ )
							{
								if ( allComp [ c ].ThisObj.GetComponent ( m_List [ e ].GetType ( ) ) == null )
								{
									DestroyImmediate ( listChild [ d ].GetComponent ( m_List [ c ].GetType ( ) ), true );
								}
							}

							listChild [ d ].transform.localPosition = getCurr;
							listChild [ d ].transform.localRotation = getCurrRot;
							break;
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
			}
		}
	}
}


public class objectInfo
{
	public GameObject ThisObj;
	public Component[] thoseComp;
}
