using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu (fileName = "ItemScript", menuName = "Scriptable/ItemScript", order = 4)]
public class ItemScriptable : ScriptableObject
{
	#region Variables
	public Image GetSprite;
	public bool buyFLife = false;
	public bool ItemBought;
	public string ItemName;
	public int Price;
	public int SavePrice;
	public int NbrBought = 0;

	public VideoClip VideoShow;
	public bool UseSprite;
	public bool UseOtherSprite;
	public bool UseColor;
	public bool UseOtherColor;

	#region ColorSprite
	public Color ColorConfirm;
	public Color ColorSelected;
	public Color ColorUnSelected;

	public Color BoughtColorSelected;
	public Color BoughtColorUnSelected;
	#endregion

	#region Sprite
	public Sprite SpriteConfirm;
	public Sprite SpriteSelected;
	public Sprite SpriteUnselected;
	public Sprite BoughtSpriteUnselected;
	public Sprite BoughtSpriteSelected;
	#endregion

	public bool ModifVie;
	public bool StartBonus;
	public bool ModifSpecial;

	public int NombreVie;
	public SpecialAction SpecAction;

	public bool Selected;
	public bool BonusItem;

	#region SpecialSlowMot 
	[Range (0.1f, 1)]
	public float SlowTime = 1;
	#endregion

	#region SpecialSlowMot 
	public float DistTakeDB = 10;
	#endregion
	#endregion
	[Range (-1, 1)]
	public float MadnessMulti = 0.5f;
	[Range (-100, 100)]
	public float MinMadNeedPourc = 25;
	[Range (-100, 100)]
	public float MadnessUsePourc = 1;
	#region updateValue
	public Vector3 CurrPos;
	#endregion
}