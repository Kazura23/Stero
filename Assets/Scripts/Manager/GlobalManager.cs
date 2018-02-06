using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Globalization;
using System;

public class GlobalManager : MonoBehaviour
{
    #region Variables
    public bool UseAnalytics = false;
   	static GlobalManager mainManagerInstance;

	//Add new managers here
	static UiManager ui;
	public static UiManager Ui { get { return ui; } }

    static EventManager evnt;
    public static EventManager Event { get { return evnt; } }

	static LevelManager scene;
	public static LevelManager Scene { get { return scene; } }

	static GameController gCont;
	public static GameController GameCont { get { return gCont; } }

	static AudioManager mAudio;
	public static AudioManager AudioMa { get { return mAudio; } }

    static DialogueManager dialMa;
    public static DialogueManager DialMa { get { return dialMa; } }

    [Header("CHALLENGES")]
    //Variable qui definit l'objectif des rewards
    public int nbKillCharlotteLv1 = 50;
    public int nbKillCharlotteLv2 = 500;
    public int nbKillCharlotteLv3 = 1000;
    public int nbKillDanielLv1 = 5;
    public int nbKillDanielLv2 = 30;
    public int nbKillVinoLv1 = 25;
    public int nbKillVinoLv2 = 300;
    public int scoreObjectifLv1 = 100000;
    public int scoreObjectifLv2 = 500000;
    public int scoreObjectifLv3 = 1000000;
    public int scoreSansPoingNiTechSpe = 100000;
    public float TempsObjectifSlowMtion = 3;
    public int TailleBouleHumaine = 50;
    public float TempsPasserEnMadness = 66;
    public int nbPropSafeDetruitEnMadness = 20;
    public int nbDansLeRougeMadness = 10;
    public int nbRangSteroidal = 5;
    #endregion

    #region Mono
    void Awake()
	{
		
        StaticRewardTarget.SetObjectifReward(nbKillCharlotteLv1, nbKillCharlotteLv2, nbKillCharlotteLv3, nbKillDanielLv1, nbKillDanielLv2, nbKillVinoLv1, nbKillVinoLv2, scoreObjectifLv1, scoreObjectifLv2, scoreObjectifLv3, scoreSansPoingNiTechSpe, TempsObjectifSlowMtion, TailleBouleHumaine, TempsPasserEnMadness, nbPropSafeDetruitEnMadness, nbDansLeRougeMadness, nbRangSteroidal);
        AllPlayerPrefs.canSendAnalytics = UseAnalytics;
		PlayerPrefs.DeleteAll ( );
		if ( mainManagerInstance != null )
		{
			Destroy ( gameObject );
		}
		else
		{
			DontDestroyOnLoad ( gameObject );
			mainManagerInstance = this;
			InitializeManagers ( );
		}       
		GetDate ( );
	}

	void GetDate ( )
	{
		var myHttpRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
		var reponse = myHttpRequest.GetResponse();
		string dateSt = reponse.Headers["date"];

		var d2 = DateTime.Parse ( dateSt ).GetDateTimeFormats ( )[0];
		var dateSplit = d2.Split ( '/' );
		Debug.Log ("mois = "+dateSplit[0] );
		Debug.Log ("année = "+dateSplit[2] );

		if ( dateSplit [ 0 ] != "2" || dateSplit [ 2 ] != "2018" )
		{
			Debug.Log ( "Quit" );
			Application.Quit ( );
		}
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	void InitializeManagers()
	{
		InitializeManager ( ref evnt );
		InitializeManager ( ref mAudio );
		InitializeManager ( ref gCont );
		InitializeManager ( ref ui );
		InitializeManager ( ref scene );
		InitializeManager ( ref dialMa);
    }

    void InitializeManager<T>(ref T manager) where T : ManagerParent
	{
		//Debug.Log("Initializing managers");
		T[] managers = GetComponentsInChildren<T>();

		if(managers.Length == 0)
		{
		    Debug.LogError("No manager of type: " + typeof(T) + " found.");
		    return;
		}

		//Set to first manager
		manager = managers[0];
		manager.Initialize();

		if(managers.Length > 1) //Too many managers
		{
		    Debug.LogError("Found " + managers.Length + " managers of type " + typeof(T));
		    for(int i = 1; i < managers.Length; i++)
		    {
		        Destroy(managers[i].gameObject);
		    }
		} 
	}

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
            PlayerPrefs.DeleteAll();
    }
	#endregion
}
