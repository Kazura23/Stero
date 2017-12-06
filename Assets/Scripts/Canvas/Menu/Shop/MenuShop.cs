using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.EventSystems;

public class MenuShop : UiParent 
{
	#region Variables
	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.Shop;
		}
	}

	//Object par défaut sélectionner a l'ouverture du shop
	public CatShop DefCatSelected;
    [Header("ALL INFO")]

    public Image iconCategory;
    public Text textCategory;
    public Image barCategory;
    public Image moleculeCategory;
    public GameObject moleculeContainer;
    public Image backgroundColor;
    public string[] quoteShop;

    public GameObject UnlockObject;
    private string itemName;
    private Sprite itemIcon;

    [HideInInspector]
	public CatShop currCatSeled;

	[HideInInspector]
	public ItemModif currItemSeled;

    [HideInInspector]
    public bool CanInput = true;

	Dictionary <string, ItemModif> allConfirm;
	List<ItemModif> allTempItem;
	GameObject fixBackShop;
	Transform saveParentAb;
	Transform saveParentBo;
	Text moneyNumberPlayer;

    Tween shopTw1, shopTw2;

	bool catCurrSelected = true;
	bool waitInputH = false;
	bool waitInputV = false;
	bool waitImpCan = false;
	bool waitImpSub = false;
    bool transition = false;
	#endregion

	#region Mono
	void Update ( )
	{
		float getH = Input.GetAxis ( "Horizontal" );
		float getV = Input.GetAxis ( "Vertical" );

        if (CanInput)
        {
            // Touche pour pouvoir selectionner les items
            if (Input.GetAxis("Submit") == 1 && !waitImpSub && !transition)
            {
                transition = true;
                waitImpSub = true;
                if (!catCurrSelected)
                {
                    transition = false;
                    BuyItem();
                }
                else
                {
                    ChangeToItem(true);
                }
            }
            else if (Input.GetAxis("Submit") == 0)
            {
                waitImpSub = false;
            }

            // Touche pour sortir des items
            if (Input.GetAxis("Cancel") == 1 && !waitImpCan && !transition)
            {
                waitImpCan = true;
                transition = true;

                if (!catCurrSelected)
                {
                    ChangeToItem(false);
                    ChangeToCat();
                }
                else
                {
                    GlobalManager.Ui.CloseThisMenu();
                }
            }
            else if (Input.GetAxis("Cancel") == 0)
            {
                waitImpCan = false;
            }
        }


		// Navigation horizontale des catégories ou items
		if ( getH != 0 && !waitInputH && !transition )
		{
			waitInputH = true;

            if ( catCurrSelected )
			{
				if ( getH > 0 )
				{
					NextCat ( true );
				}
				else
				{
					NextCat ( false );
				}
			}
			else if ( getH == 1 || getH == -1 )
			{
				NextItem ( ( int ) getH );
			}
			else
			{
				waitInputH = false;
			}
		}
		else if ( Input.GetAxis ( "Horizontal" ) == 0 )
		{
			waitInputH = false;
		}

		// Navigation vertocal des items
		if ( !catCurrSelected && ( getV == 1 || getV == -1 ) && !waitInputV )
		{
				waitInputV = true;
				NextItem ( ( int ) getH * 2 );
		}
		else if ( Input.GetAxis ( "Vertical" ) == 0 )
		{
			waitInputV = false;
		}
	}
	#endregion

	#region Public Methods
	public override void OpenThis ( MenuTokenAbstract GetTok = null )
	{
		base.OpenThis ( GetTok );

		GlobalManager.Ui.SlowMotion.transform.parent.SetParent ( transform );
		GlobalManager.Ui.BonusLife.transform.parent.SetParent ( transform );

        transition = false;

        fixBackShop.SetActive ( true );
		currCatSeled = DefCatSelected;
		if ( currItemSeled != currCatSeled.DefautItem )
		{
			CheckSelectItem ( false );
		}

		currItemSeled = currCatSeled.DefautItem;
		CheckSelectItem ( true );
    }

	public override void CloseThis ( )
	{
		GlobalManager.Ui.SlowMotion.transform.parent.SetParent ( saveParentAb );
		GlobalManager.Ui.BonusLife.transform.parent.SetParent ( saveParentBo );

        fixBackShop.SetActive ( false );
		base.CloseThis (  );
	}

	// Nouvelle selection de catégorie
	public void NextCat ( bool right )
	{
		CheckSelectCat ( false );

		if ( right )
		{
			currCatSeled = currCatSeled.RightCategorie;
		}
		else
		{
			currCatSeled = currCatSeled.LeftCategorie;
		}

		CheckSelectCat ( true );
	}

	// Nouvelle selection d'item
	// -1 = gauche _ 1 droite _ 2 haut _ -2 bas
	public void NextItem ( int thisDir )
	{
        DOVirtual.DelayedCall(.1f, () => {
            CheckSelectItem ( false );
            switch (thisDir)
            {
                case -1:
                    ItemLeft();
                    break;
                case 1:
                    ItemRight();
                    break;
            }
        });
		
		CheckSelectItem ( true );
	}

    public void ShopUnlock()
    {
        backgroundColor.DOFade(.95f, .1f);

        backgroundColor.transform.SetParent(currItemSeled.transform.parent.parent);
        UnlockObject.transform.SetParent(currItemSeled.transform.parent.parent);
        
        currItemSeled.GetComponentsInChildren<Text>()[2].text = "SOLD!";

        CanInput = false;

        Image bg = UnlockObject.GetComponentsInChildren<Image>()[0];
        bg.DOFade(0, 0);
        bg.DOFade(1, .25f);
        bg.transform.DOScaleY(0, 0);
        bg.transform.DOScaleY(1, .25f);

        Image icon = UnlockObject.GetComponentsInChildren<Image>()[1];
        icon.transform.DOScale(3, 0);
        icon.transform.DOScale(1, .25f);
        icon.DOFade(0, 0);
        icon.DOFade(1, .25f);

        DOVirtual.DelayedCall(.3f, () =>
        {

            Text text = UnlockObject.GetComponentsInChildren<Text>()[0];
            text.text = itemName;

            text.GetComponent<CanvasGroup>().DOFade(1, .2f);
            text.transform.DOLocalMoveY(0, 0);
            text.transform.DOLocalMoveX(-200, 0);
            text.transform.DOLocalMoveX(0, .2f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(.75f, () =>
                {
                    text.GetComponent<CanvasGroup>().DOFade(0, .15f);
                    text.transform.DOLocalMoveY(-100, .15f).OnComplete(() =>
                    {

                        text.text = "UNLOCKED";
                        text.transform.DOLocalMoveY(100, 0f);
                        text.GetComponent<RainbowColor>().colors[1] = new Color32(0xc5, 0xcf, 0x65, 0xFF);
                        text.GetComponent<CanvasGroup>().DOFade(1, .15f);
                        text.transform.DOLocalMoveY(0, .15f).OnComplete(() =>
                        {



                            DOVirtual.DelayedCall(.75f, () =>
                            {
                                text.GetComponent<CanvasGroup>().DOFade(0, .15f);
                                text.transform.DOLocalMoveY(-100, .15f).OnComplete(() =>
                                {

                                    text.text = quoteShop[UnityEngine.Random.Range(0, quoteShop.Length)];

                                    text.transform.DOLocalMoveY(100, 0f);
                                    text.GetComponent<CanvasGroup>().DOFade(1, .15f);
                                    text.transform.DOLocalMoveY(0, .15f).OnComplete(() =>
                                    {


                                        text.GetComponent<RainbowColor>().colors[1] = new Color32(0xF4, 0x6C, 0x6E, 0xFF);

                                        DOVirtual.DelayedCall(.75f, () =>
                                        {
                                            text.GetComponent<CanvasGroup>().DOFade(0, .15f);
                                            text.transform.DOLocalMoveY(-100, .15f).OnComplete(() =>
                                            {

                                                backgroundColor.DOFade(0f, .3f);
                                                bg.DOFade(0, .3f);
                                                icon.DOFade(0, .3f).OnComplete(()=> {
                                                    CanInput = true;
                                                });


                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }
	// achete ou confirme un item
	public void BuyItem ( )
	{
		string getCons = Constants.ItemBought + currCatSeled.NameCat;
		bool buy = false;
		Dictionary <string, ItemModif> getAllBuy = allConfirm;
		ItemModif getThis;

		if ( currCatSeled.NameCat == "BONUS" && currItemSeled.ModifVie && GlobalManager.Ui.ExtraHearts [ 1 ].enabled )
		{
			return;	
		}

		if ( AllPlayerPrefs.GetBoolValue ( getCons + currItemSeled.ItemName ) )
		{
			AllPlayerPrefs.SetStringValue ( getCons + currItemSeled.ItemName, "Confirm" );

            Debug.Log("Confirm?");

            if ( getAllBuy.TryGetValue ( getCons, out getThis ) )
			{
				AllPlayerPrefs.SetStringValue ( getCons + getThis.ItemName, "ok" );

                if ( getThis.UseOtherSprite )
				{
					getThis.GetComponent<Image> ( ).sprite = currItemSeled.BoughtSpriteUnselected;
				}
				else
				{
					getThis.GetComponent<Image> ( ).sprite = currItemSeled.SpriteUnselected;
				}

				if ( getThis.UseColor )
				{
					if ( getThis.UseOtherColor )
					{
						getThis.GetComponent<Image> ( ).color = currItemSeled.BoughtColorUnSelected;
					}
					else
					{
						getThis.GetComponent<Image> ( ).color = currItemSeled.ColorUnSelected;
					}
				}

				getAllBuy.Remove ( getCons );
			}

			getThis = currItemSeled;
			getThis.GetComponent<Image> ( ).sprite = getThis.SpriteConfirm;

			if ( getThis.UseColor )
			{
				getThis.GetComponent<Image> ( ).color = getThis.ColorConfirm;
			}

			buy = true;

            SelectObject();

            GlobalManager.Ui.SelectShop();

            getAllBuy.Add ( getCons, getThis );
		}
		else 
		{
			bool checkProg = false;
			ItemModif currIT = currItemSeled;

			if ( currCatSeled.Progression )
			{
				if ( currIT.LeftItem.ItemBought || currIT.RightItem.ItemBought )
				{
					checkProg = true;
				}
			}
			else
			{
				checkProg = true;	
			}

			if ( checkProg && AllPlayerPrefs.GetIntValue ( Constants.Coin ) > currIT.Price )
			{
				buy = true;

                //moneyNumberPlayer.transform.DOScale(3, .25f);

                AllPlayerPrefs.SetIntValue ( Constants.Coin, -currIT.Price );

				if ( currCatSeled.BuyForLife )
				{
                    ShopUnlock();

					if ( getAllBuy.TryGetValue ( getCons, out getThis ) )
					{
						getAllBuy.Remove ( getCons );
					}
					getAllBuy.Add ( getCons, currItemSeled );
					AllPlayerPrefs.SetStringValue ( getCons + currIT.ItemName );

                    //float ease = DOVirtual.EasedValue(0, 1,1,AnimationCurve.Linear);
                    currItemSeled.transform.GetChild(0).DOPunchScale(Vector3.one * 1.2f, .5f, 10, 1);
                    currItemSeled.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
				}
				else
				{
					allTempItem.Add ( currItemSeled );
				}
			}
		}

		if ( buy )
		{
			if ( currCatSeled.NameCat == "ABILITIES" )
			{
				GlobalManager.Ui.SlowMotion.sprite = currItemSeled.transform.Find ( "Icon" ).GetComponent<Image> ( ).sprite;
			}
			else if ( currCatSeled.NameCat == "BONUS" && currItemSeled.ModifVie )
			{
				if ( GlobalManager.Ui.ExtraHearts [ 0 ].enabled )
				{

                    SelectObject();
                    GlobalManager.Ui.ExtraHearts [ 1 ].enabled = true;
				}
				else
				{

                    SelectObject();
                    GlobalManager.Ui.ExtraHearts [ 0 ].enabled = true;
				}
			}
		}

		moneyNumberPlayer.text = "" + AllPlayerPrefs.GetIntValue ( Constants.Coin );
	}
	#endregion

	#region Private Methods

    void SelectObject()
    {
        itemIcon = currItemSeled.GetComponentsInChildren<Image>()[4].sprite;

        itemName = currItemSeled.GetComponentsInChildren<Text>()[0].text;

        shopTw1.Kill(true);
        shopTw2.Kill(true);

        shopTw1 = currItemSeled.transform.GetChild(0).transform.DOScale(1, 0);
        shopTw2 = currItemSeled.transform.GetChild(0).transform.GetComponent<Image>().DOFade(1, .1f).OnComplete(() => {
            shopTw1 = currItemSeled.transform.GetChild(0).transform.DOScale(1.4f, .4f);
            shopTw2 = currItemSeled.transform.GetChild(0).transform.GetComponent<Image>().DOFade(0, .4f);
        });
    }

	protected override void InitializeUi()
	{
		currCatSeled = DefCatSelected;
		currItemSeled = currCatSeled.DefautItem;

		saveParentAb = GlobalManager.Ui.SlowMotion.transform.parent.parent;
		saveParentBo = GlobalManager.Ui.BonusLife.transform.parent.parent;

        fixBackShop = transform.parent.Find ( "GlobalBackGround/Shop" ).gameObject;
		moneyNumberPlayer = fixBackShop.transform.Find ( "MoneyMutation/MoneyNumber" ).GetComponent<Text> ( );

		moneyNumberPlayer.text = "" + AllPlayerPrefs.GetIntValue(Constants.Coin);

		ItemModif[] checkAllItem = GetComponentsInChildren<ItemModif> ( true );
		ItemModif currItem;

		string getCons = Constants.ItemBought;

		Dictionary <string, ItemModif> getItemConf = new Dictionary<string, ItemModif> ( );
		allTempItem = new List<ItemModif> ( );

		for ( int a = 0; a < checkAllItem.Length; a++ )
		{
			if ( AllPlayerPrefs.GetBoolValue ( getCons + currCatSeled.NameCat + checkAllItem [ a ].ItemName ) )
			{
				currItem = checkAllItem [ a ];

				try{
					getItemConf.Add ( getCons + currCatSeled.NameCat, currItem ); 

				}catch{Debug.Log("key same");}

				currItem.GetComponent<Image> ( ).sprite = currItem.SpriteConfirm;

				if ( currItem.UseColor )
				{
					currItem.GetComponent<Image> ( ).color = currItem.ColorConfirm;
				}
			}
		}

		CheckSelectItem ( true );

		allConfirm = getItemConf;
		GlobalManager.GameCont.AllModifItem = getItemConf;
		GlobalManager.GameCont.AllTempsItem = allTempItem;
    }

	//Changement de catégorie a item et inversement
	void ChangeToItem ( bool goItem )
	{
        CatShop thisShop = currCatSeled;

        if ( goItem && catCurrSelected ) // Changement de cat a item
		{
			catCurrSelected = false;

            currItemSeled = thisShop.DefautItem;

            iconCategory.DOFade(0, .1f);
            textCategory.DOFade(0, .1f);
            barCategory.DOFade(0, .1f);

            moleculeContainer.transform.DORotate(new Vector3(moleculeContainer.transform.localEulerAngles.x, moleculeContainer.transform.localEulerAngles.y, -130),1f);
            moleculeContainer.transform.DOLocalMoveX(transform.localPosition.x -539, 1f);
            moleculeContainer.transform.DOLocalMoveY(transform.localPosition.y - 10, 1f);

            moleculeContainer.transform.DOScale(1.25f, 1f).OnComplete(()=> {
                transition = false;
                thisShop.GetComponent<Image>().DOFade(1, 0.1f);
                iconCategory.transform.DORotate(Vector3.zero, 0);
                textCategory.transform.DORotate(new Vector3(0,0,423), 0);
                barCategory.transform.DORotate(new Vector3(0,0,423), 0);
                iconCategory.transform.DOMoveX(thisShop.transform.position.x + 200, 0);
                iconCategory.transform.DOMoveY(thisShop.transform.position.y ,0);
                textCategory.transform.DOMoveY(moleculeContainer.transform.position.y + 300, 0);
                textCategory.transform.DOMoveX(moleculeContainer.transform.position.x -90, 0);
                barCategory.transform.DOMoveX(moleculeContainer.transform.position.x - 90, 0);
                barCategory.transform.DOMoveY(moleculeContainer.transform.position.y + 300, 0);
                iconCategory.DOFade(1, .25f);
                textCategory.DOFade(1, .25f);
                barCategory.DOFade(1, .25f);
            });

            //On remet les molécules de couleur au gris
            foreach (Transform cat in DefCatSelected.transform.parent)
            {
                cat.GetComponent<Image>().DOFade(0, 0.1f);
            }

            //Seul le premier item est centré
           // thisShop.transform.GetChild(0).DOLocalMove(new Vector2(-280, 600), 0);
           // thisShop.transform.GetChild(1).DOLocalMove(new Vector2(-525, 895), 0);

            DOVirtual.DelayedCall(1f, () => 
			{
				ItemModif thisItem = currItemSeled;

				thisItem = thisItem.RightItem;
				while ( thisItem != currItemSeled )
				{
					NewItemSelect ( thisItem, false );
					thisItem = thisItem.RightItem;
				}

				NewItemSelect ( currItemSeled, true );
            });
        }
		else if ( !goItem && !catCurrSelected ) // Changement de item a cat
		{
			catCurrSelected = true;
		}
	}

    void ChangeToCat()
    {
        CatShop thisShop = currCatSeled;

        //Debug.Log(thisShop.transform.GetChild(0));

        iconCategory.DOFade(0, .05f);
        textCategory.DOFade(0, .05f);
        barCategory.DOFade(0, .05f);
        moleculeContainer.transform.DORotate(Vector3.zero, .5f);
        moleculeContainer.transform.DOScale(1, .5f);
        moleculeContainer.transform.DOLocalMove(Vector2.zero, .5f).OnComplete(()=> {
            transition = false;

            iconCategory.transform.DORotate(Vector3.zero, 0);
            textCategory.transform.DORotate(Vector3.zero, 0);
            barCategory.transform.DORotate(Vector3.zero, 0);
            iconCategory.DOFade(1, .1f);
            textCategory.DOFade(1, .1f);
            barCategory.DOFade(1, .1f);

            if (textCategory.text == "ABILITIES")
            {
                textCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 55, 0);
                barCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 55, 0);
                iconCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 40, 0);
            }
            else
            {
                textCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
                barCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
                iconCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
            }
            iconCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 160, 0);
            textCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);
            barCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);
        });

        //On remet les molécules à leur couleur initiale
        foreach (Transform cat in DefCatSelected.transform.parent)
        {
            cat.GetComponent<Image>().DOFade(1, 0.1f);
        }

		ItemModif thisItem = currItemSeled;

		thisItem.GetComponent<CanvasGroup> ( ).DOKill ( );
		thisItem.GetComponent<CanvasGroup> ( ).DOFade ( 0, 0 );
		thisItem.ResetPos ( );
		thisItem = thisItem.RightItem;

		while ( thisItem != currItemSeled )
		{
			thisItem.GetComponent<CanvasGroup> ( ).DOKill ( );
			thisItem.GetComponent<CanvasGroup> ( ).DOFade ( 0, 0 );
			thisItem.ResetPos ( );
			thisItem = thisItem.RightItem;
		}
    }

	// Selection d'une nouvelle catégorie
	void CheckSelectCat ( bool selected )
	{
		CatShop thisShop = currCatSeled;

		if ( selected )
		{
			CheckSelectItem ( false );
			currItemSeled = thisShop.DefautItem;
			CheckSelectItem ( true );
			thisShop.Selected = true;


            DOVirtual.DelayedCall(.1f, () => {
                iconCategory.GetComponent<Image>().DOFade(1, .1f);
                textCategory.DOFade(1, .1f);
                barCategory.transform.GetChild(0).transform.DOLocalMoveX(200, 0);
                barCategory.transform.GetChild(0).transform.DOLocalMoveX(0, .6f);

                textCategory.text = thisShop.NameCat;
                iconCategory.sprite = thisShop.OtherRefSprite;
                

                thisShop.GetComponent<Image>().transform.DOScale(1.25f, .2f);
                //thisShop.GetComponent<Image>().DOFade(1f, .05f);
                //iconCategory.GetComponent<Image>().sprite = thisShop.OtherRefSprite;
                

                barCategory.transform.GetChild(0).GetComponent<Image>().DOColor(thisShop.ColorSelected, 0);

                if (textCategory.text == "ABILITIES")
                {
                    textCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 55, 0);
                    barCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 55, 0);
                    iconCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 40, 0);
                }
                else
                {
                    textCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
                    barCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
                    iconCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
                }
                iconCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 160, 0);
                textCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);
                barCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);
            });
            /*
            iconCategory.transform.DOLocalMoveY(transform.localPosition.y + 10, .25f).OnComplete(() => {
                iconCategory.transform.DOLocalMoveY(transform.localPosition.y - 10, .25f);
            }).SetLoops(-1,LoopType.Restart);*/

            //iconCategory.transform.DOKill();


            if ( thisShop.UseColor )
			{
				thisShop.GetComponent<Image> ( ).color = thisShop.ColorSelected;
			}

			if ( thisShop.UseSprite )
			{
				//thisShop.GetComponent<Image> ( ).sprite = thisShop.SpriteSelected;
			}
		}
		else
		{
			thisShop.Selected = false;
            //iconCategory.transform.DOKill();

            
                iconCategory.GetComponent<Image>().DOFade(0, .1f);
            textCategory.DOFade(0, .1f);
            //iconCategory.GetComponent<RainbowMove>().enabled = false;
            thisShop.GetComponent<Image>().transform.DOScale(.8f, .2f);
           // thisShop.GetComponent<Image>().DOFade(0, .2f);

            if ( thisShop.UseColor )
			{
				//thisShop.GetComponent<Image> ( ).color = thisShop.ColorUnSelected;
			}
			if ( thisShop.UseSprite )
			{
				//thisShop.GetComponent<Image> ( ).sprite = thisShop.SpriteUnSelected;
			}
		}
	}

    void ItemLeft()
    {
		ItemModif thisItem = currItemSeled;
		Vector3 savePos;

		thisItem.transform.DOMove ( thisItem.RightItem.CurrPos, 1, true );
		thisItem = thisItem.RightItem;

		while ( thisItem != currItemSeled )
		{
			thisItem.transform.DOMove ( thisItem.RightItem.CurrPos, 1, true );
			thisItem = thisItem.RightItem;

			NewItemSelect ( thisItem, false );
		}

		thisItem = currItemSeled;
		savePos = thisItem.CurrPos;

		do
		{
			thisItem.CurrPos = thisItem.RightItem.CurrPos;
			thisItem = thisItem.RightItem;
		} while ( thisItem != currItemSeled.LeftItem );

		currItemSeled.LeftItem.CurrPos = savePos;

		currItemSeled = currItemSeled.LeftItem;
		NewItemSelect ( currItemSeled, true );
    }

    void ItemRight()
    {
		ItemModif thisItem = currItemSeled;
		Vector3 savePos;
		thisItem.transform.DOMove ( thisItem.LeftItem.CurrPos, 1, true );
		thisItem = thisItem.LeftItem;

		while ( thisItem != currItemSeled )
		{
			thisItem.transform.DOMove ( thisItem.LeftItem.CurrPos, 1, true );
			thisItem = thisItem.LeftItem;

			NewItemSelect ( thisItem, false );
		}

		thisItem = currItemSeled;
		savePos = thisItem.CurrPos;

		do
		{
			thisItem.CurrPos = thisItem.LeftItem.CurrPos;
			thisItem = thisItem.LeftItem;
		} while ( thisItem != currItemSeled.RightItem );

		currItemSeled.RightItem.CurrPos = savePos;

        //thisItem.transform.DOLocalMove(new Vector2(-448, 800), .5f);
		currItemSeled = currItemSeled.RightItem;
		NewItemSelect ( currItemSeled, true );
    }

	void NewItemSelect ( ItemModif thisItem, bool selected )
	{
		if ( selected )
		{
			thisItem.GetComponent<CanvasGroup>().DOFade(1, 1);
			thisItem.transform.DOScale(.75f, 0);
		}
		else
		{
			thisItem.GetComponent<CanvasGroup>().DOFade(0.5f, 1);
			thisItem.transform.DOScale(.5f, 0);
		}
	}

	// Selection d'un nouvelle item
	void CheckSelectItem ( bool selected )
	{
        ItemModif thisItem = currItemSeled;

		if ( selected )
		{
			thisItem.Selected = true;

			if ( thisItem.ItemBought && thisItem.UseOtherColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.BoughtColorSelected;
			}
			else if ( thisItem.UseColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.ColorSelected;
			}

			if ( thisItem.ItemBought && thisItem.UseOtherSprite )
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.BoughtSpriteSelected;
			}
			else
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.SpriteSelected;
			}
		}
		else
		{
			thisItem.Selected = false;
			if ( thisItem.ItemBought && thisItem.UseOtherColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.BoughtColorUnSelected;
			}
			else if ( thisItem.UseColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.ColorUnSelected;
			}

			if ( thisItem.ItemBought && thisItem.UseOtherSprite )
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.BoughtSpriteUnselected;
			}
			else
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.SpriteUnselected;
			}
		}
	}
	#endregion
}
