﻿using UnityEngine;

public class GlobalManager : MonoBehaviour
{
	#region Variables
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
	#endregion

	#region Mono
	void Awake()
	{
		//PlayerPrefs.DeleteAll ( );
	
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
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	void InitializeManagers()
	{
		InitializeManager ( ref evnt );
		InitializeManager ( ref gCont );
		InitializeManager ( ref ui );
		InitializeManager ( ref scene );
		InitializeManager ( ref mAudio );
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
