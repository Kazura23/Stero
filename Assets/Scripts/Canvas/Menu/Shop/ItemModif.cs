using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemModif : MonoBehaviour 
{
	#region Variables
	public bool buyFLife = false;
	public bool ItemBought;
	public string ItemName;
	public int Price;

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

	public ItemModif RightItem;
	public ItemModif LeftItem;

	public bool ModifVie;
	public bool StartBonus;
	public bool ModifSpecial;

	public int NombreVie;
	public SpecialAction SpecAction;

	public bool Selected;
	public bool AddItem;

	#region SpecialSlowMot 
	public float SlowMotion = 1;
	public float SpeedSlowMot = 1;
	public float SpeedDeacSM = 3;
	public float ReduceSlider;
	public float RecovSlider;
	#endregion

	#region SpecialSlowMot 
	public float DistTakeDB = 10;
	#endregion
	#endregion

	#region updateValue
	public Vector3 CurrPos;
	List<Image> getExtraH;
	Vector3 saveStartPos;
	Text getText;
	int savePrice;
	bool canText = true;

	void Start ()
	{
		getExtraH = GlobalManager.Ui.ExtraHearts;
		savePrice = Price;

		if ( transform.Find ( "Cost Number" ) != null )
		{
			getText = transform.Find ( "Cost Number" ).GetComponent<Text> ( );
		}

		if ( getText == null )
		{
			canText = false;
		}

		saveStartPos = transform.position;
		CurrPos = saveStartPos;
	}

	void Update ()
	{
		if ( !canText )
		{
			return;
		}

		if ( ModifVie )
		{
			if ( getExtraH [ 1 ].enabled )
			{
				getText.text = "Empty";
			}
			else
			{
				if ( getExtraH [ 0 ].enabled )
				{
					Price = savePrice * 2;
				}
				else
				{
					Price = savePrice;
				}

				getText.text = Price.ToString ( );
			}
		}
	}

	public void ResetPos ( )
	{
		transform.DOMove ( saveStartPos, 0, true );
		CurrPos = saveStartPos;
	}
	#endregion
}
