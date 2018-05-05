using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

[CustomEditor (typeof (ItemScriptable))]
public class EditItemMod : Editor
{
	#region Variables
	// bool var;
	SerializedProperty ItemName;
	SerializedProperty Price;
	SerializedProperty GetSprite;
	SerializedProperty SavePrice;

	SerializedProperty ColorConfirm;
	SerializedProperty ColorSelected;
	SerializedProperty ColorUnSelected;
	SerializedProperty BoughtColorSelected;
	SerializedProperty BoughtColorUnSelected;

	SerializedProperty SpriteConfirm;
	SerializedProperty SpriteSelected;
	SerializedProperty SpriteUnselected;
	SerializedProperty BoughtSpriteSelected;
	SerializedProperty BoughtSpriteUnselected;
	SerializedProperty VideoShow;

	SerializedProperty AddValueStat;

	SerializedProperty SpecAction;
	SerializedProperty SlowMotion;
	SerializedProperty MadnessUse;
	SerializedProperty DeadBallDist;
	SerializedProperty MadnessMulti;
	SerializedProperty MinMadNeedPourc;

	GUIContent Confirm;
	GUIContent Select;
	GUIContent UnSelect;
	GUIContent BuySelect;
	GUIContent BuyUnSelect;
	#endregion

	#region Public Methods
	public void OnEnable ( )
	{
		Confirm = new GUIContent ("Confirm");
		Select = new GUIContent ("Select");;
		UnSelect = new GUIContent ("UnSelect");;
		BuySelect = new GUIContent ("Buy Select");;
		BuyUnSelect = new GUIContent ("Buy UnSelect");;

		ItemName = serializedObject.FindProperty ("ItemName");
		Price = serializedObject.FindProperty ("Price");
		GetSprite = serializedObject.FindProperty ("GetSprite");
		SavePrice = serializedObject.FindProperty ("SavePrice");

		ColorConfirm = serializedObject.FindProperty ("ColorConfirm");
		ColorSelected = serializedObject.FindProperty ("ColorSelected");
		ColorUnSelected = serializedObject.FindProperty ("ColorUnSelected");
		BoughtColorSelected = serializedObject.FindProperty ("BoughtColorSelected");
		BoughtColorUnSelected = serializedObject.FindProperty ("BoughtColorUnSelected");

		SpriteConfirm = serializedObject.FindProperty ("SpriteConfirm");
		SpriteSelected = serializedObject.FindProperty ("SpriteSelected");
		SpriteUnselected = serializedObject.FindProperty ("SpriteUnselected");
		BoughtSpriteSelected = serializedObject.FindProperty ("BoughtSpriteSelected");
		VideoShow = serializedObject.FindProperty ("VideoShow");

		BoughtSpriteUnselected = serializedObject.FindProperty ("BoughtSpriteUnselected");

		SpecAction = serializedObject.FindProperty ("SpecAction");
		SlowMotion = serializedObject.FindProperty ("SlowTime");
		MadnessUse = serializedObject.FindProperty ("MadnessUsePourc");
		DeadBallDist = serializedObject.FindProperty ("DistTakeDB");

		AddValueStat = serializedObject.FindProperty ("BonusItem");

		MadnessMulti = serializedObject.FindProperty ("MadnessMulti");
		MinMadNeedPourc = serializedObject.FindProperty ("MinMadNeedPourc");
	}

	public override void OnInspectorGUI ( )
	{
		EditorGUILayout.LabelField ("Inspector Item Info", EditorStyles.boldLabel);
		ItemScriptable myTarget = (ItemScriptable)target;

		serializedObject.Update ( );
		EditorGUILayout.PropertyField (ItemName);
		EditorGUILayout.PropertyField (Price);
		EditorGUILayout.PropertyField (SavePrice);
		EditorGUILayout.PropertyField (GetSprite);
		EditorGUILayout.PropertyField (VideoShow);

		#region MainButton

		// Si l'item est acheté
		if (AllPlayerPrefs.GetBoolValue (Constants.ItemBought + myTarget.ItemName))
		{
			myTarget.ItemBought = true;
		}
		else
		{
			myTarget.ItemBought = false;
		}
		EditorGUILayout.Space ( );

		// verification de l'utilisation de la couleur
		var buttonStyle = new GUIStyle (EditorStyles.miniButtonLeft);
		if (myTarget.UseColor)
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		EditorGUILayout.BeginHorizontal ( );
		// Bouton d'utilisation de la couleur
		if (GUILayout.Button ("UseColor", buttonStyle))
		{
			myTarget.UseColor = !myTarget.UseColor;
		}

		// verification de l'utilisation de sprite
		buttonStyle = new GUIStyle (EditorStyles.miniButtonRight);
		if (myTarget.UseSprite)
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		// Bouton d'utilisation de sprite
		if (GUILayout.Button ("UseSprite", buttonStyle))
		{
			myTarget.UseSprite = !myTarget.UseSprite;
		}
		EditorGUILayout.EndHorizontal ( );
		#endregion

		#region Couleur
		// Ajout des différentes couleurs
		if (myTarget.UseColor)
		{
			EditorGUI.indentLevel = 1;

			EditorGUILayout.Space ( );
			EditorGUILayout.BeginHorizontal ( );

			buttonStyle = new GUIStyle (EditorStyles.miniButton);
			if (myTarget.UseOtherColor)
			{
				buttonStyle.normal.textColor = Color.green;
			}
			else
			{
				buttonStyle.normal.textColor = Color.red;
			}

			if (GUILayout.Button ("UseOtherColor", buttonStyle))
			{
				myTarget.UseOtherColor = !myTarget.UseOtherColor;
			}

			if (myTarget.UseSprite)
			{
				EditorGUILayout.Space ( );

				buttonStyle = new GUIStyle (EditorStyles.miniButton);
				if (myTarget.UseOtherSprite)
				{
					buttonStyle.normal.textColor = Color.green;
				}
				else
				{
					buttonStyle.normal.textColor = Color.red;
				}

				if (GUILayout.Button ("UseOtherSprite", buttonStyle))
				{
					myTarget.UseOtherSprite = !myTarget.UseOtherSprite;
				}
			}
			EditorGUILayout.EndHorizontal ( );

			EditorGUILayout.Space ( );
			EditorGUILayout.LabelField ("Color Information", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField (ColorConfirm, Confirm);
			EditorGUILayout.PropertyField (ColorSelected, Select);
			EditorGUILayout.PropertyField (ColorUnSelected, UnSelect);

			if (myTarget.GetSprite != null)
			{
				if (myTarget.UseOtherColor)
				{
					EditorGUI.indentLevel = 2;

					EditorGUILayout.Space ( );

					EditorGUILayout.PropertyField (BoughtColorSelected, BuySelect);
					EditorGUILayout.PropertyField (BoughtColorUnSelected, BuyUnSelect);

					if (myTarget.ItemBought)
					{
						if (myTarget.Selected)
						{
							myTarget.GetSprite.color = myTarget.BoughtColorSelected;
						}
						else
						{
							myTarget.GetSprite.color = myTarget.BoughtColorUnSelected;
						}
					}
				}
				else
				{
					if (myTarget.Selected)
					{
						myTarget.GetSprite.color = myTarget.ColorSelected;
					}
					else
					{
						myTarget.GetSprite.color = myTarget.ColorUnSelected;
					}
				}
			}
		}
		#endregion

		#region Sprite
		if (myTarget.GetSprite != null)
		{
			if (myTarget.SpriteConfirm == null)
			{
				myTarget.SpriteConfirm = myTarget.GetSprite.sprite;
			}
			if (myTarget.SpriteSelected == null)
			{
				myTarget.SpriteSelected = myTarget.GetSprite.sprite;
			}
			if (myTarget.SpriteUnselected == null)
			{
				myTarget.SpriteUnselected = myTarget.GetSprite.sprite;
			}

			if (myTarget.BoughtSpriteSelected == null)
			{
				myTarget.BoughtSpriteSelected = myTarget.GetSprite.sprite;
			}
			if (myTarget.BoughtSpriteUnselected == null)
			{
				myTarget.BoughtSpriteUnselected = myTarget.GetSprite.sprite;
			}
		}

		if (myTarget.UseSprite)
		{
			EditorGUI.indentLevel = 1;

			EditorGUILayout.Space ( );

			if (!myTarget.UseColor)
			{
				buttonStyle = new GUIStyle (EditorStyles.miniButton);
				if (myTarget.UseOtherSprite)
				{
					buttonStyle.normal.textColor = Color.green;
				}
				else
				{
					buttonStyle.normal.textColor = Color.red;
				}

				if (GUILayout.Button ("UseOtherSprite", buttonStyle))
				{
					myTarget.UseOtherSprite = !myTarget.UseOtherSprite;
				}
				EditorGUILayout.Space ( );
			}

			EditorGUILayout.LabelField ("Sprite Information", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField (SpriteConfirm, Confirm);
			EditorGUILayout.PropertyField (SpriteSelected, Select);
			EditorGUILayout.PropertyField (SpriteUnselected, UnSelect);

			if (myTarget.UseOtherSprite)
			{
				EditorGUI.indentLevel = 2;

				EditorGUILayout.Space ( );

				EditorGUILayout.PropertyField (BoughtSpriteSelected, BuySelect);
				EditorGUILayout.PropertyField (BoughtSpriteUnselected, BuyUnSelect);

				if (myTarget.ItemBought && myTarget.GetSprite != null)
				{
					if (myTarget.Selected)
					{
						myTarget.GetSprite.sprite = myTarget.BoughtSpriteSelected;
					}
					else
					{
						myTarget.GetSprite.sprite = myTarget.BoughtSpriteUnselected;
					}
				}

			}
			else if (myTarget.GetSprite != null)
			{
				if (myTarget.Selected)
				{
					myTarget.GetSprite.sprite = myTarget.SpriteSelected;
				}
				else
				{
					myTarget.GetSprite.sprite = myTarget.SpriteUnselected;
				}
			}

			EditorGUILayout.Space ( );
		}
		#endregion

		EditorGUI.indentLevel = 0;

		EditorGUILayout.LabelField ("Around Information", EditorStyles.boldLabel);

		EditorGUILayout.Space ( );
		EditorGUILayout.LabelField ("Modification", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal ( );

		#region Caract
		buttonStyle = new GUIStyle (EditorStyles.miniButton);
		if (myTarget.ModifVie)
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		if (GUILayout.Button ("Add One Life", buttonStyle))
		{
			myTarget.ModifVie = !myTarget.ModifVie;
		}

		if (myTarget.StartBonus)
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		if (GUILayout.Button ("StartBonus", buttonStyle))
		{
			myTarget.StartBonus = !myTarget.StartBonus;
		}

		/*if ( myTarget.ModifVie )
		{
			myTarget.NombreVie = EditorGUILayout.IntField ( "NombreVie", myTarget.NombreVie );

			if ( myTarget.NombreVie <= 0 )
			{
				myTarget.NombreVie = 1;
			}
		}*/
		EditorGUILayout.EndHorizontal ( );

		buttonStyle = new GUIStyle (EditorStyles.miniButton);
		if (myTarget.ModifSpecial)
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}
		EditorGUILayout.BeginHorizontal ( );
		#endregion

		#region SpecialAction
		if (GUILayout.Button ("ModifSpecial", buttonStyle))
		{
			myTarget.ModifSpecial = !myTarget.ModifSpecial;
		}

		if (myTarget.ModifSpecial)
		{
			EditorGUILayout.PropertyField (SpecAction);
		}
		else
		{
			myTarget.SpecAction = SpecialAction.Nothing;
		}

		EditorGUILayout.EndHorizontal ( );

		EditorGUI.indentLevel = 1;
		if (myTarget.SpecAction != SpecialAction.Nothing)
		{
			EditorGUILayout.PropertyField (AddValueStat);

			EditorGUILayout.PropertyField (MadnessUse);
			EditorGUILayout.PropertyField (MinMadNeedPourc);

			if (myTarget.SpecAction != SpecialAction.SlowMot)
			{
				EditorGUILayout.PropertyField (MadnessMulti);
			}
		}

		if (myTarget.SpecAction == SpecialAction.SlowMot)
		{
			EditorGUILayout.PropertyField (SlowMotion);
		}
		else if (myTarget.SpecAction == SpecialAction.DeadBall)
		{
			EditorGUILayout.PropertyField (DeadBallDist);
		}

		#endregion

		serializedObject.ApplyModifiedProperties ( );

		EditorGUI.indentLevel = 0;
	}
	#endregion

	#region Private Methods
	#endregion
}