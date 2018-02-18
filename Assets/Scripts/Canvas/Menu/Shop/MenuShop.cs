using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.EventSystems;
using Rewired;

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
    //private Sprite itemIcon;

    [HideInInspector]
	public CatShop currCatSeled;

	[HideInInspector]
	public ItemModif currItemSeled;

    [HideInInspector]
    public bool CanInput = true;

	Dictionary <string, ItemModif> allConfirm;
	List<ItemModif> allTempItem;
	SpecialAction getLastItm;
	GameObject fixBackShop;
	Transform saveParentAb;
	Transform saveParentBo;
	Text moneyNumberPlayer;
    Sprite itemSprite;
	Player inputPlayer;

    Tween shopTw1, shopTw2;

	bool catCurrSelected = true;
	bool waitInputH = false;
	bool waitInputV = false;
	bool waitImpCan = false;
	bool waitImpSub = false;
    bool transition = false;
	bool coupSimpl = false;
	#endregion

	#region Mono
	void Start ( )
	{
		inputPlayer = ReInput.players.GetPlayer(0);
	}

	void Update ( )
	{
		float getH = inputPlayer.GetAxis ( "Horizontal" );
		float getV = inputPlayer.GetAxis ( "Vertical" );

		if ( inputPlayer.GetAxis ( "CoupSimple" ) == 0 )
		{
			coupSimpl = true;
		}
			
        if (CanInput)
        {
            // Touche pour pouvoir selectionner les items
			if ( ( inputPlayer.GetAxis("CoupSimple") == 1 || Input.GetKeyDown ( KeyCode.Return ) ) && coupSimpl && !waitImpSub && !transition)
            {
				GlobalManager.Ui.CheckContr ( );
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
			else if (inputPlayer.GetAxis("CoupSimple") == 0)
            {
                waitImpSub = false;
            }

            // Touche pour sortir des items
			if ( ( inputPlayer.GetAxis("CoupDouble") == 1 || Input.GetKeyDown ( KeyCode.Escape ) ) && !waitImpCan && !transition)
            {
				GlobalManager.Ui.CheckContr ( );
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
			else if (inputPlayer.GetAxis("CoupDouble") == 0)
            {
                waitImpCan = false;
            }
        }


		// Navigation horizontale des catégories ou items
		if ( getH != 0 && !waitInputH && !transition )
		{
			GlobalManager.Ui.CheckContr ( );
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
		else if ( inputPlayer.GetAxis ( "Horizontal" ) == 0 )
		{
			waitInputH = false;
		}

		// Navigation vertocal des items
		if ( !catCurrSelected && ( getV == 1 || getV == -1 ) && !waitInputV )
		{
				waitInputV = true;
				NextItem ( ( int ) getH * 2 );
		}
		else if ( inputPlayer.GetAxis ( "Vertical" ) == 0 )
		{
			waitInputV = false;
		}
	}
	#endregion

	#region Public Methods
	public override void OpenThis ( MenuTokenAbstract GetTok = null )
	{
        if (GlobalManager.GameCont.canOpenShop)
        {
            GlobalManager.GameCont.canOpenShop = false;
            base.OpenThis(GetTok);

            GlobalManager.Ui.SlowMotion.transform.parent.SetParent(transform);
            GlobalManager.Ui.BonusLife.transform.parent.SetParent(transform);

			CanInput = false;
            //GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(0, 0);
			GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(1, .7f).OnComplete( ()=>
			{
				CanInput = true;
			});

            GlobalManager.Ui.OpenShop();

            transition = false;

            fixBackShop.SetActive(true);
			CheckSelectCat ( false );
			currCatSeled = DefCatSelected;
			CheckSelectCat ( true );

			CheckSelectItem ( false );
            currItemSeled = currCatSeled.DefautItem;
			CheckSelectItem ( true );
        }
    }

	public override void CloseThis ( )
	{
		if ( GlobalManager.Ui.SlowMotion.GetComponent<CanvasGroup> ( ) )
		{
			GlobalManager.Ui.SlowMotion.GetComponent<CanvasGroup> ( ).DOFade ( 0, 0.1f ).OnComplete ( ( ) =>
			{
				GlobalManager.Ui.SlowMotion.enabled = false;
			} );
		}
		else
		{
			GlobalManager.Ui.SlowMotion.enabled = false;
			GlobalManager.Ui.SlowMotion.gameObject.SetActive ( false );
		}

		GlobalManager.Ui.SlowMotion.transform.parent.SetParent ( saveParentAb );
		GlobalManager.Ui.BonusLife.transform.parent.SetParent ( saveParentBo );

        GlobalManager.Ui.CloseShop();

        GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(0, .3f).OnComplete(()=> {

            fixBackShop.SetActive(false);

            GlobalManager.GameCont.canOpenShop = true;

            base.CloseThis();

        });
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
        icon.sprite = itemSprite;
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
		int count = 0;
		bool buy = false;
		Dictionary <string, ItemModif> getAllBuy = allConfirm;
		List<ItemModif> getTempItem = allTempItem;
		ItemModif getThis;

		if ( currCatSeled.NameCat == "BONUS" && currItemSeled.ThisItem.ModifVie && GlobalManager.Ui.ExtraHearts [ 1 ].enabled )
		{
			return;	
		}
		else if ( currItemSeled.ThisItem.BonusItem && currItemSeled.ThisItem.SpecAction != getLastItm )
		{
			return;	
		}

		if ( AllPlayerPrefs.GetBoolValue ( getCons + currItemSeled.ThisItem.ItemName ) )
		{
			AllPlayerPrefs.SetStringValue ( getCons + currItemSeled.ThisItem.ItemName, "Confirm" );

            if ( getAllBuy.TryGetValue ( getCons, out getThis ) )
			{
				AllPlayerPrefs.SetStringValue ( getCons + getThis.ThisItem.ItemName, "ok" );

				if ( getThis.ThisItem.UseOtherSprite )
				{
					getThis.GetComponent<Image> ( ).sprite = currItemSeled.ThisItem.BoughtSpriteUnselected;
				}
				else
				{
					getThis.GetComponent<Image> ( ).sprite = currItemSeled.ThisItem.SpriteUnselected;
				}

				if ( getThis.ThisItem.UseColor )
				{
					if ( getThis.ThisItem.UseOtherColor )
					{
						getThis.GetComponent<Image> ( ).color = currItemSeled.ThisItem.BoughtColorUnSelected;
					}
					else
					{
						getThis.GetComponent<Image> ( ).color = currItemSeled.ThisItem.ColorUnSelected;
					}
				}

				getAllBuy.Remove ( getCons );
			}

			getThis = currItemSeled;
			getThis.GetComponent<Image> ( ).sprite = getThis.ThisItem.SpriteConfirm;

			if ( getThis.ThisItem.UseColor )
			{
				getThis.GetComponent<Image> ( ).color = getThis.ThisItem.ColorConfirm;
			}

			buy = true;

            SelectObject();

            GlobalManager.Ui.SelectShop();

			getLastItm = getThis.ThisItem.SpecAction;
            getAllBuy.Add ( getCons, getThis );
		}
		else 
		{
			bool checkProg = false;
			ItemModif currIT = currItemSeled;

			if ( currCatSeled.Progression )
			{
				if ( currIT.LeftItem.ThisItem.ItemBought || currIT.RightItem.ThisItem.ItemBought )
				{
					checkProg = true;
				}
			}
			else
			{
				checkProg = true;	
			}

			if ( checkProg && AllPlayerPrefs.GetIntValue ( Constants.Coin ) > currIT.ThisItem.Price )
			{
				buy = true;

                //moneyNumberPlayer.transform.DOScale(3, .25f);
				if ( currCatSeled.BuyForLife )
				{
                    getThis = currItemSeled;
                    //Debug.Log(getThis.GetComponentsInChildren<Text>()[0].text);
                    itemName = getThis.GetComponentsInChildren<Text>()[0].text;
                    itemSprite = getThis.GetComponentsInChildren<Image>()[4].sprite;

					ShopUnlock();

                    if ( getAllBuy.TryGetValue ( getCons, out getThis ) )
					{
						getAllBuy.Remove ( getCons );
					}

					getLastItm = currItemSeled.ThisItem.SpecAction;
					getAllBuy.Add ( getCons, currItemSeled );
					AllPlayerPrefs.SetStringValue ( getCons + currIT.ThisItem.ItemName );

                    //float ease = DOVirtual.EasedValue(0, 1,1,AnimationCurve.Linear);
                    currItemSeled.transform.GetChild(0).DOPunchScale(Vector3.one * 1.2f, .5f, 10, 1);
                    currItemSeled.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
				}
				else
				{
					for ( int a = 0; a < getTempItem.Count; a++ )
					{
						if ( getTempItem [ a ] == currItemSeled )
						{
							count++;
						}
					}

					if ( count > 3 )
					{
						return;
					}
					else if ( ( currCatSeled.NameCat == "UPGRADES" ) && !AllPlayerPrefs.GetBoolValue ( Constants.ItemBought + "ABILITIES" + currItemSeled.ThisItem.ItemName ) )
					{
						return;
					}

					getTempItem.Add ( currItemSeled );
				}

				AllPlayerPrefs.SetIntValue ( Constants.Coin, -currIT.ThisItem.Price );
			}
		}

		if ( buy )
		{
			if ( currCatSeled.NameCat == "ABILITIES" )
			{
				if ( GlobalManager.Ui.SlowMotion.GetComponent<CanvasGroup> ( ) )
				{
					GlobalManager.Ui.SlowMotion.GetComponent<CanvasGroup> ( ).DOFade ( 1, 0.1f );
				}
				else
				{
					GlobalManager.Ui.SlowMotion.gameObject.SetActive ( true );
				}

				GlobalManager.Ui.SlowMotion.color = new Color ( 1, 1, 1, 1 );
				GlobalManager.Ui.GameParent.gameObject.SetActive ( true );
				GlobalManager.Ui.SlowMotion.enabled = true;
				GlobalManager.Ui.SlowMotion.sprite = currItemSeled.transform.Find ( "Icon" ).GetComponent<Image> ( ).sprite;
			}
			else if ( currCatSeled.NameCat == "BONUS" && currItemSeled.ThisItem.ModifVie )
			{
				if ( GlobalManager.Ui.ExtraHearts [ 0 ].enabled )
				{
					SelectObject ( );
					GlobalManager.Ui.NewLife ( 2 );
				}
				else
				{
					SelectObject ( );
					GlobalManager.Ui.NewLife ( 1 );
				}
			}
			else if ( currCatSeled.NameCat == "UPGRADES" )
			{
				List<Text> getGameT = GlobalManager.GameCont.GetBonusText;
				Text currText = currItemSeled.transform.Find ( "Description" ).GetComponent<Text> ( );

				if ( !getGameT.Contains ( currText ) )
				{
					getGameT.Add ( currText );
				}

				currText.text = "LEVEL " + ( count + 2 ).ToString ( );
				currItemSeled.ThisItem.Price *= 2;
			}
		}

		moneyNumberPlayer.text = "" + AllPlayerPrefs.GetIntValue ( Constants.Coin );
	}
	#endregion

	#region Private Methods

    void SelectObject()
    {
        //itemIcon = currItemSeled.GetComponentsInChildren<Image>()[4].sprite;

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
		getLastItm = SpecialAction.Nothing;
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
			if ( AllPlayerPrefs.GetBoolValue ( getCons + currCatSeled.NameCat + checkAllItem [ a ].ThisItem.ItemName ) )
			{
				currItem = checkAllItem [ a ];

				try{
					getItemConf.Add ( getCons + currCatSeled.NameCat, currItem ); 
					getLastItm = currItem.ThisItem.SpecAction;
				}catch{
                    //Debug.Log("key same");
                }

				currItem.GetComponent<Image> ( ).sprite = currItem.ThisItem.SpriteConfirm;

				if ( currItem.ThisItem.UseColor )
				{
					currItem.GetComponent<Image> ( ).color = currItem.ThisItem.ColorConfirm;
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
		CanInput = false;

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
                textCategory.transform.DOMoveY(moleculeContainer.transform.position.y + 35, 0);
                textCategory.transform.DOMoveX(moleculeContainer.transform.position.x -90, 0);
                barCategory.transform.DOMoveX(moleculeContainer.transform.position.x - 90, 0);
                barCategory.transform.DOMoveY(moleculeContainer.transform.position.y + 35, 0);
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
				CanInput = true;
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
		CanInput = false;
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
			CanInput = true;
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

		thisItem.transform.DOMove ( thisItem.RightItem.ThisItem.CurrPos, 1, true );
		thisItem = thisItem.RightItem;

		while ( thisItem != currItemSeled )
		{
			thisItem.transform.DOMove ( thisItem.RightItem.ThisItem.CurrPos, 1, true );
			thisItem = thisItem.RightItem;

			NewItemSelect ( thisItem, false );
		}

		thisItem = currItemSeled;
		savePos = thisItem.ThisItem.CurrPos;

		do
		{
			thisItem.ThisItem.CurrPos = thisItem.RightItem.ThisItem.CurrPos;
			thisItem = thisItem.RightItem;
		} while ( thisItem != currItemSeled.LeftItem );

		currItemSeled.LeftItem.ThisItem.CurrPos = savePos;

		currItemSeled = currItemSeled.LeftItem;
		NewItemSelect ( currItemSeled, true );
    }

    void ItemRight()
    {
		ItemModif thisItem = currItemSeled;
		Vector3 savePos;
		thisItem.transform.DOMove ( thisItem.LeftItem.ThisItem.CurrPos, 1, true );
		thisItem = thisItem.LeftItem;

		while ( thisItem != currItemSeled )
		{
			thisItem.transform.DOMove ( thisItem.LeftItem.ThisItem.CurrPos, 1, true );
			thisItem = thisItem.LeftItem;

			NewItemSelect ( thisItem, false );
		}

		thisItem = currItemSeled;
		savePos = thisItem.ThisItem.CurrPos;

		do
		{
			thisItem.ThisItem.CurrPos = thisItem.LeftItem.ThisItem.CurrPos;
			thisItem = thisItem.LeftItem;
		} while ( thisItem != currItemSeled.RightItem );

		currItemSeled.RightItem.ThisItem.CurrPos = savePos;

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
			thisItem.ThisItem.Selected = true;

			if ( thisItem.ThisItem.ItemBought && thisItem.ThisItem.UseOtherColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.ThisItem.BoughtColorSelected;
			}
			else if ( thisItem.ThisItem.UseColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.ThisItem.ColorSelected;
			}

			if ( thisItem.ThisItem.ItemBought && thisItem.ThisItem.UseOtherSprite )
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.ThisItem.BoughtSpriteSelected;
			}
			else
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.ThisItem.SpriteSelected;
			}
		}
		else
		{
			thisItem.ThisItem.Selected = false;
			if ( thisItem.ThisItem.ItemBought && thisItem.ThisItem.UseOtherColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.ThisItem.BoughtColorUnSelected;
			}
			else if ( thisItem.ThisItem.UseColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.ThisItem.ColorUnSelected;
			}

			if ( thisItem.ThisItem.ItemBought && thisItem.ThisItem.UseOtherSprite )
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.ThisItem.BoughtSpriteUnselected;
			}
			else
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.ThisItem.SpriteUnselected;
			}
		}
	}
	#endregion
}
