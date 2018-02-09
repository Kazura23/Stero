using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemModif : MonoBehaviour 
{
	public ItemScriptable ThisItem;
	public ItemModif RightItem;
	public ItemModif LeftItem;

	Image getCurr;
	List<Image> getExtraH;
	Vector3 saveStartPos;
	Text getText;
	int savePrice;
	bool canText = true;

	void Start ()
	{
		if ( RightItem == null )
		{
			RightItem = GetComponent<ItemModif> ( );
		}

		if ( LeftItem == null )
		{
			LeftItem = GetComponent<ItemModif> ( );
		}

		getExtraH = GlobalManager.Ui.ExtraHearts;
		getCurr = GetComponent<Image> ( );
		savePrice = ThisItem.Price;

		if ( transform.Find ( "Cost Number" ) != null )
		{
			getText = transform.Find ( "Cost Number" ).GetComponent<Text> ( );
		}

		if ( getText == null )
		{
			canText = false;
		}

		saveStartPos = transform.position;
		ThisItem.CurrPos = saveStartPos;
	}

	void Update ()
	{
		if ( !canText )
		{
			return;
		}

		if ( ThisItem.ModifVie )
		{
			if ( getExtraH [ 1 ].enabled )
			{
				getText.text = "Empty";
			}
			else
			{
				if ( getExtraH [ 0 ].enabled )
				{
					ThisItem.Price = savePrice * 2;
				}
				else
				{
					ThisItem.Price = savePrice;
				}

				getText.text = ThisItem.Price.ToString ( );
			}
		}

		getCurr.sprite = ThisItem.GetSprite.sprite;
		getCurr.color = ThisItem.GetSprite.color;
	}

	public void ResetPrice ( )
	{
		ThisItem.Price = savePrice;
	}

	public void ResetPos ( )
	{
		transform.DOKill ( false );
		transform.DOMove ( saveStartPos, 0, true );
		ThisItem.CurrPos = saveStartPos;
	}
}
