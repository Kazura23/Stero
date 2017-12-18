﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class GameController : ManagerParent
{
	#region Variables
	public List<FxList> AllFx;
	public List<ChunkLock> ChunkToUnLock; 
	public Transform GarbageTransform;
	public MeshDesctruc MeshDest;
	[HideInInspector]
	public GameObject Player;
	public SpawnChunks SpawnerChunck;
    public bool Intro;

	[HideInInspector]
	public Dictionary <string, ItemModif> AllModifItem;
	[HideInInspector]
	public List <ItemModif> AllTempsItem;

    public Tween soundFootSteps;
	bool checkStart = false;
    bool isStay = true, isReady = false, relance = false;
    [HideInInspector]
    public bool introFinished;
    private int chooseOption = 2;
    public Vector3[] moveRotate = new Vector3[5];
    public GameObject[] tabGameObject = new GameObject[5];
    public float delayRotate = 5;
    public Transform textMeshs;


	bool restartGame = false;
    public bool canOpenShop = true;

    bool GameStarted = false;
    #endregion

    #region Mono
    void Update ( )
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			GlobalManager.Ui.OpenThisMenu(MenuType.Pause);
		}
        if (!checkStart && isStay && !isReady)
        {
			
            switch (chooseOption)
            {
            case 0: // Options

                    //Debug.Log("Options");

                
                //GameStartedUpdate();
                break;

			case 1: // Leaderboards
                //Debug.Log("Leaderboards");

                    break;

            case 2: //Start game

                   //Debug.Log("Start");

				if (Input.GetKeyDown(KeyCode.W)&& ! restartGame)
                {
                    StartGame();
                    isStay = false;
                    AnimationStartGame();
                }

                break;
		
			case 3: // Shop
                //Debug.Log("Shop");

                    if (Input.GetKeyDown(KeyCode.W) && GlobalManager.GameCont.canOpenShop)
                        GlobalManager.Ui.OpenThisMenu(MenuType.Shop);
                    break;

            case 4:  // Quitter
                //Debug.Log("Quit");
                break;
            }
            if (!checkStart && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChooseRotate(false);
            }else if (!checkStart && Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChooseRotate(true);
            }
        }else if (isReady && Input.GetKeyDown(KeyCode.W) && isStay )
        {
            Player.GetComponent<PlayerController>().GetPunchIntro();

			if ( restartGame )
			{
				Player.GetComponent<PlayerController>().StopPlayer = false;
				restartGame = false;
			}

            Debug.Log("PunchIntro");
        }
        
	}


    #endregion

    #region Public Methods
    public void ActiveGame()
    {
        GameStartedUpdate();
    }

    public void StartGame ( )
	{
        //Debug.Log("Start");
        AllPlayerPrefs.ResetStaticVar();
		if ( Player == null )
		{
			Player = GameObject.FindGameObjectWithTag("Player");
			Player.GetComponent<PlayerController> ( ).InitPlayer ( );
		}

		Player.GetComponent<PlayerController> ( ).ResetPlayer ( );
		Player.GetComponent<PlayerController> ( ).ThisAct = SpecialAction.Nothing;
		Intro = true;

		if ( restartGame )
		{
			Player.transform.DOMoveZ(3, 1).OnComplete(() =>
			{
				isStay = true;
				Intro = false;
			});
		}


		SetAllBonus ( );

		GameStarted = true;
		checkStart = false;

		SpawnerChunck.FirstSpawn ( );

        Camera.main.GetComponent<RainbowRotate>().time = 2;
        Camera.main.GetComponent<RainbowMove>().time = 1;
		GlobalManager.Ui.CloseThisMenu ( );

		if ( !GlobalManager.AudioMa.IsAudioLaunch ( AudioType.MusicBackGround ) ) 
		{ 
			setMusic ( ); 
		} 
    }

	public GameObject FxInstanciate ( Vector3 thisPos, string fxName, Transform parentObj = null, float timeDest = 0.35f )
	{
		List<FxList> getAllFx = AllFx;
		GameObject getObj;

		for ( int a = 0; a < getAllFx.Count; a++ )
		{
			if ( getAllFx [ a ].FxName == fxName )
			{
				getObj = getAllFx [ a ].FxObj;

				if ( parentObj != null )
				{
					getObj = ( GameObject ) Instantiate ( getObj, parentObj );
				}
				else
				{
					getObj = ( GameObject ) Instantiate ( getObj );
				}

				getObj.transform.position = thisPos;

				Destroy ( getObj, timeDest );

				return getObj;
			}
		}

		return null;
	}

    public void Restart () 
	{
        
        AllPlayerPrefs.ResetStaticVar();
		SceneManager.LoadScene ( "ProtoAlex", LoadSceneMode.Single );

        GlobalManager.Ui.DashSpeedEffect(false);
        SpawnerChunck.RemoveAll ( );
        checkStart = false;

        if (AllPlayerPrefs.relance)
        {
			restartGame = true;
            isReady = true;
            GameStarted = true;

			Player.GetComponent<PlayerController>().StopPlayer = false;
			Camera.main.GetComponent<RainbowRotate>().time = .4f;
			Camera.main.GetComponent<RainbowMove>().time = .2f;

			soundFootSteps = DOVirtual.DelayedCall(GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed / GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed - GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed / 25, () => {
				//Debug.Log("here");
				int randomSound = UnityEngine.Random.Range(0, 6);

				GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "FootSteps_" + (randomSound + 1), false);
				//J'ai essayé de jouer le son FootSteps_1 pour voir, mais ça marche
				//Debug.Log("Audio");
			}).SetLoops(-1, LoopType.Restart);
            //GameStartedUpdate();
           // StartCoroutine(TrashFunction());
        }
        else
        {


            isReady = false;
            GameStarted = false;
            Debug.Log(isReady + " " + GameStarted);
        }
        //GameStarted = false;
    }   
    
	public void UnLockChunk ( ChunksScriptable thisScript, GameObject ThisChunk ) 
	{ 
		thisScript.TheseChunks.Add ( ThisChunk ); 

		AllPlayerPrefs.SetStringValue ( Constants.ChunkUnLock + ThisChunk.name ); 
	} 

    private IEnumerator TrashFunction()
    {
        yield return new WaitForSeconds(0.5f); //=> attendre 0.5 seconde ok (mais code deguelasse)
        GameStartedUpdate();
    }

    #endregion

    #region Private Methods
	void setMusic ( ) 
	{ 
		GlobalManager.AudioMa.OpenAudio ( AudioType.MusicBackGround, "", false, setMusic ); 
	} 

    private void AnimationStartGame() // don't forget freeze keyboard when animation time
    {
        Player.transform.DORotate(new Vector3(0, 90, 0), 2).OnComplete(()=> 
            {
                //animation seringue + son
                Player.transform.DORotate(new Vector3(-65, 0, 0), 2).OnComplete(()=> 
                {
                    // activation shader + son
                    /*for(int i = 0; i < textMeshs.childCount; i++) // voir si active la liste des text mesh ou un par un
                    {
                        textMeshs.GetChild(i).gameObject.SetActive(true);
                    }*/
                    //Player.GetComponentInChildren<RainbowRotate>().enabled = true;
                    Player.transform.DORotate(Vector3.zero, 1).OnComplete(()=> 
                    {
                        GlobalManager.AudioMa.OpenAudio(AudioType.Other, "MrStero_Intro", false);

                        Player.transform.GetChild(3).DOLocalMoveY(0.312f, 1).OnComplete(() =>
                        {
                            //Player.GetComponentInChildren<RainbowMove>().enabled = true;


                            Player.transform.DOMoveZ(3, 1).OnComplete(() =>
                            {
                                isReady = true;
                                isStay = true;
                                //Player.GetComponent<PlayerController>().StopPlayer = false;
                                //Debug.Log("anime fonctionnelle");
                            });
                        });
                    });
                });
            });
    }

    private IEnumerator TimerRotate()
    {
        yield return new WaitForSeconds(delayRotate);
        isStay = true;
    }

    private void ChooseRotate(bool p_add)
    {
		if ( !Intro )
		{
			return;
		}

        if (p_add && GlobalManager.Ui.menuOpen == MenuType.Nothing)
        {
            Debug.Log("Rotate");
            chooseOption++;
            if (chooseOption == moveRotate.Length)
                chooseOption = 0;
        }
        else 
        {
            if(GlobalManager.Ui.menuOpen == MenuType.Nothing)
            {
                chooseOption--;
                if (chooseOption == -1)
                    chooseOption = moveRotate.Length - 1;

            }
        }
        isStay = false;
        StartCoroutine(TimerRotate());
        Player.transform.DOLocalRotate(moveRotate[chooseOption], delayRotate);
    }

    private void GameStartedUpdate()
    {
        /*if (Input.GetAxis("CoupSimple") == 1 || Input.GetAxis("CoupDouble") == 1)
        {*/
            if (GameStarted && !checkStart)
            {
                //Debug.Log("Demarrage");

                GlobalManager.Ui.Intro();
                isStay = false;

                checkStart = true;
                //Debug.Log("player = " + Player);
                Player.GetComponent<PlayerController>().StopPlayer = false;
                Camera.main.GetComponent<RainbowRotate>().time = .4f;
                Camera.main.GetComponent<RainbowMove>().time = .2f;

                soundFootSteps = DOVirtual.DelayedCall(GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed / GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed - GlobalManager.GameCont.Player.GetComponent<PlayerController>().MaxSpeed / 25, () => {
                    //Debug.Log("here");
                    int randomSound = UnityEngine.Random.Range(0, 6);

                    GlobalManager.AudioMa.OpenAudio(AudioType.FxSound, "FootSteps_" + (randomSound + 1), false);
                    //J'ai essayé de jouer le son FootSteps_1 pour voir, mais ça marche
                    //Debug.Log("Audio");
                }).SetLoops(-1, LoopType.Restart);
            }
            /*else
            {
                // punch the door
            }
        }*/

        if (Input.GetKeyDown(KeyCode.T))
        {

            Debug.Log("Fx");
            Vector3 playerPos = GlobalManager.GameCont.Player.transform.position;
            GameObject thisGO = GlobalManager.GameCont.FxInstanciate(new Vector3(.16f, 0.12f, 0.134f) + playerPos, "PlayerReady", GlobalManager.GameCont.Player.transform, 1f);
            thisGO.transform.SetParent(GlobalManager.GameCont.Player.GetComponent<PlayerController>().rightHand.transform);
            thisGO.transform.DOLocalMove(Vector3.zero, 0);

            GameObject thisGOLeft = GlobalManager.GameCont.FxInstanciate(new Vector3(.16f, 0.12f, 0.134f) + playerPos, "PlayerReady", GlobalManager.GameCont.Player.transform, 1f);
            thisGOLeft.transform.SetParent(GlobalManager.GameCont.Player.GetComponent<PlayerController>().leftHand.transform);
            thisGOLeft.transform.DOLocalMove(Vector3.zero, 0);
        }
    }

    protected override void InitializeManager ( )
	{
		Player = GameObject.FindGameObjectWithTag("Player");

		SpawnerChunck = GetComponentInChildren<SpawnChunks> ( );
		SpawnerChunck.InitChunck ( );
        AllPlayerPrefs.saveData = SaveData.Load();

		List<ChunkLock> GetChunk = ChunkToUnLock; 
		List<NewChunk> CurrList; 

		int b; 

		for ( int a = 0; a < GetChunk.Count; a++ ) 
		{ 
			CurrList = GetChunk [ a ].AllUnlock; 
			for ( b = 0; b < CurrList.Count; b++ ) 
			{ 
				if ( AllPlayerPrefs.GetBoolValue ( Constants.ChunkUnLock + CurrList[ b ].ThisChunk.name ) ) 
				{ 
					UnLockChunk ( GetChunk [ a ].ThisChunk, CurrList [ b ].ThisChunk ); 
					GetChunk [ a ].AllUnlock.RemoveAt ( b ); 
					b--; 
				} 
			} 

			if ( GetChunk [ a ].AllUnlock.Count == 0 ) 
			{ 
				GetChunk.RemoveAt ( a ); 
				a--; 
			} 
		} 
	}

	void SetAllBonus ( )
	{
		Dictionary <string, ItemModif> getMod = AllModifItem;
		PlayerController currPlayer = Player.GetComponent<PlayerController> ( );
		List <ItemModif> AllTI = AllTempsItem;
		ItemModif thisItem;
		List<string> getKey = new List<string> ( );

		SpawnerChunck.EndLevel = 1;

		if ( getMod != null )
		{
			foreach ( KeyValuePair <string, ItemModif> thisKV in getMod )
			{
				thisItem = thisKV.Value;

				setItemToPlayer ( thisItem, currPlayer );
			}
		}

		if ( AllTI != null )
		{
			while ( AllTI.Count > 0 )
			{
				setItemToPlayer ( AllTI [ 0 ], currPlayer );

				AllTI.RemoveAt ( 0 );
			}
		}
	}

	void setItemToPlayer ( ItemModif thisItem, PlayerController currPlayer )
	{
		if ( thisItem.ModifSpecial )
		{
			currPlayer.ThisAct = thisItem.SpecAction;

			switch ( thisItem.SpecAction )
			{
			case SpecialAction.OndeChoc:
				currPlayer.SliderSlow.maxValue = currPlayer.delayChocWave;
				currPlayer.SliderSlow.value = currPlayer.delayChocWave;
				break;
			case SpecialAction.DeadBall:
				currPlayer.SliderSlow.maxValue = currPlayer.DelayDeadBall;
				currPlayer.SliderSlow.value = currPlayer.DelayDeadBall;
				break;
			default:
				currPlayer.SliderSlow.maxValue = 10;
				break;
			}

			if ( thisItem.SpecAction == SpecialAction.SlowMot ) 
			{ 
				currPlayer.SlowMotion = thisItem.SlowMotion; 
				currPlayer.SpeedSlowMot = thisItem.SpeedSlowMot; 
				currPlayer.SpeedDeacSM = thisItem.SpeedDeacSM; 
				currPlayer.ReduceSlider = thisItem.ReduceSlider; 
				currPlayer.RecovSlider = thisItem.RecovSlider; 
			} 
			else if ( thisItem.SpecAction == SpecialAction.DeadBall ) 
			{ 
				currPlayer.DistDBTake = thisItem.DistTakeDB; 
				if ( thisItem.AddItem ) 
				{ 
					currPlayer.SlowMotion += thisItem.SlowMotion; 
					currPlayer.SpeedSlowMot += thisItem.SpeedSlowMot; 
					currPlayer.SpeedDeacSM += thisItem.SpeedDeacSM; 
					currPlayer.ReduceSlider += thisItem.ReduceSlider; 
					currPlayer.RecovSlider += thisItem.RecovSlider; 
				} 
				else 
				{ 
					currPlayer.SlowMotion = thisItem.SlowMotion; 
					currPlayer.SpeedSlowMot = thisItem.SpeedSlowMot; 
					currPlayer.SpeedDeacSM = thisItem.SpeedDeacSM; 
					currPlayer.ReduceSlider = thisItem.ReduceSlider; 
					currPlayer.RecovSlider = thisItem.RecovSlider; 
				} 
			} 
		}

		if ( thisItem.ModifVie )
		{
			currPlayer.Life++;
		}

		if ( thisItem.StartBonus )
		{
			SpawnerChunck.StartBonus = true;
			SpawnerChunck.EndLevel++;
		}
	}
    #endregion

}

#region Save
public static class SaveData
{
    public static void Save(ListData p_dataSave)
    {
        string path1 = Application.dataPath + "/Save/save.bin";
        FileStream fSave = File.Create(path1);
        AllPlayerPrefs.saveData.listScore.SerializeTo(fSave);
        fSave.Close();
        Debug.Log("save");
    }

    public static ListData Load()
    {
        string path1 = Application.dataPath + "/Save/save.bin";
        ListData l = new ListData();
        if (File.Exists(path1))
        {
            FileStream fSave = File.Open(path1, FileMode.Open, FileAccess.ReadWrite);
            l.listScore = fSave.Deserialize<List<DataSave>>();
        }
        return l;
    }
}

[System.Serializable]
public class DataSave
{
    public DataSave(int p_fs, int p_s, int p_p, float p_d)
    {
        finalScore = p_fs;
        score = p_s;
        piece = p_p;
        distance = p_d;
    }

    public DataSave()
    {
        finalScore = 0;
        score = 0;
        piece = 0;
        distance = 0;
    }

    public int finalScore;
    public int score;
    public int piece;
    public float distance;

}

public class ListData
{
    public List<DataSave> listScore;
    public ListData()
    {
        listScore = new List<DataSave>();
    }
    public void Tri_Insert()
    {
        int verif;
        int i, j;
        DataSave verifSave;
        for (i = 1; i < listScore.Count; i++)
        {
            verif = listScore[i].finalScore;
            verifSave = listScore[i];
            for (j = i; j > 0 && listScore[j - 1].finalScore < verif; j--)
            {
                listScore[j] = listScore[j - 1];
            }
            listScore[j] = verifSave;
        }
    }
    public void Add(DataSave p_save)
    {
        if (listScore.Count <= 9)
        {
            listScore.Add(p_save);
            Tri_Insert();
        }
        else if (p_save.finalScore > listScore[listScore.Count - 1].finalScore)
        {
            listScore.Add(p_save);
            Tri_Insert();
            listScore.RemoveAt(listScore.Count - 1);
        }
        SaveData.Save(this);
    }
}

public static class StreamExtensions
{
    public static void SerializeTo<T>(this T o, Stream stream)
    {
        new BinaryFormatter().Serialize(stream, o);  // serialize o not typeof(T)
    }

    public static T Deserialize<T>(this Stream stream)
    {
        return (T)new BinaryFormatter().Deserialize(stream);
    }
}
#endregion

[System.Serializable]
public class FxList 
{
	public string FxName;
	public GameObject FxObj;
}


[System.Serializable] 
public class ChunkLock 
{ 
	public ChunksScriptable ThisChunk; 
	public List<NewChunk> AllUnlock; 
} 

[System.Serializable] 
public class NewChunk  
{ 
	public GameObject ThisChunk; 
	public UnLockMethode ThisMethod; 
} 