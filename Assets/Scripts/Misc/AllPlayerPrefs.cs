using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

public static class AllPlayerPrefs
{
    public static int piece;
    public static int finalScore;
    public static int scoreWhithoutDistance;
    public static float distance;
    public static ListData saveData;
    public static bool relance;

    //var and function analytics
    public static bool canSendAnalytics;

    public static int ANbRun;

    public static float ATimerRun;
    public static int AHeartUse;
    public static int AExtraStart;
    
    public static float ATimeDash;
    public static int ANbCoupSimple;
    public static int ANbCoupDouble;
    public static int ANbPassageMadness;
    public static int ANbTechSpe;
    public static string ANameTechSpe;

    public static int ANbTotalEnemyKill;
    public static int ANbCharlotte;
    public static int ANbVino;
    public static int ANbKnighty;

    public static string ATypeObstacle;
    public static string ANameObstacle;
    public static string ANameChunk;
    //public static int

    private static string AnalyticsTimeGame(float deltatime)
    {
        int seconde = 0, minute = 0, heure = 0;
        seconde = (int)deltatime;
        minute = seconde / 60;
        seconde = seconde % 60;
        heure = minute / 60;
        minute = minute % 60;
        return heure + " h " + minute + " min " + seconde + " s";
    }

    public static void SendAnalytics()
    {
        Debug.Log("entrer");
        if (canSendAnalytics)
        {
            Dictionary<string, object> gameoverAttribut = new Dictionary<string, object>();
            Dictionary<string, object> enemyAttribut = new Dictionary<string, object>();

            gameoverAttribut.Add("Durée d'une run", AnalyticsTimeGame(ATimerRun));
            gameoverAttribut.Add("Nombre de vie utilisé", AHeartUse);
            gameoverAttribut.Add("Nombre extra start utilisé", AExtraStart);

            gameoverAttribut.Add("Durée total du dash", ATimeDash);
            gameoverAttribut.Add("Nombre de coups simple", ANbCoupSimple);
            gameoverAttribut.Add("Nombre de coups double", ANbCoupDouble);
            gameoverAttribut.Add("Nombre de passage en madness", ANbPassageMadness);
            gameoverAttribut.Add("Nombre de "+ANameTechSpe+" utilisé", ANbTechSpe);

            gameoverAttribut.Add("Nom de la chunk du gameover", ANameChunk);
            //gameoverAttribut.Add(" de l'ennemi du gameover", ATypeObstacle);
            gameoverAttribut.Add("Nom et Type de l'ennemi du gameover", ANameObstacle+" / "+ATypeObstacle);

            enemyAttribut.Add("Nombre total ennemi tuer", ANbTotalEnemyKill);
            enemyAttribut.Add("Nombre total charlotte tuer", ANbCharlotte);
            enemyAttribut.Add("Nombre total vino tuer", ANbVino);
            enemyAttribut.Add("Nombre total knighty tuer", ANbKnighty);

            var result = Analytics.CustomEvent("Stats Run", gameoverAttribut);
            var result2 = Analytics.CustomEvent("Stats Ennemies", enemyAttribut);
            if (result.Equals(AnalyticsResult.Ok))
            {
                Debug.Log("envoit = " + result);
                Debug.Log("envoit 2 = " + result2);
            }
            else
            {
                Debug.LogWarning("envoit = " + result);
                Debug.LogWarning("envoit 2 = " + result2);
            }
        }
    }

    public static void ResetStaticVar()
    {
        piece = 0;
        finalScore = 0;
        scoreWhithoutDistance = 0;
        distance = 0;

        AHeartUse = 0;
        AExtraStart = 0;
        ATimeDash = 0;
        ATimerRun = 0;
        ANbCoupDouble = 0;
        ANbCoupSimple = 0;
        ANbPassageMadness = 0;
        ANbTechSpe = 0;

        ANbTotalEnemyKill = 0;
        ANbVino = 0;
        ANbCharlotte = 0;
        ANbKnighty = 0;
    }

    public static DataSave NewData()
    {
        return new DataSave(finalScore, scoreWhithoutDistance, piece, distance);
    }

    /*public static void testSave()
    {
        GameObject.Find("Trash_text").GetComponent<Text>().text = Application.dataPath;
        if(!File.Exists(Application.dataPath + "/save.bin"))
        {
            File.Create(Application.dataPath + "/save.bin");
            GameObject.Find("Trash_text").GetComponent<Text>().text = "create";
        }
        else
        {
            GameObject.Find("Trash_text").GetComponent<Text>().text = "file find";
        }
    }*/

    #region Get Methods
	public static int GetIntValue ( string thisString )
	{
		return PlayerPrefs.GetInt ( thisString, 10000 );
	}

	public static int GetIntValueForSong ( string thisString )
	{
		return PlayerPrefs.GetInt ( thisString, 100 );
	}

	public static bool GetBoolValue ( string thisString )
	{
		string getVal = PlayerPrefs.GetString ( thisString, "Nope" );

		if ( getVal == "Nope" )
		{
			return false;
		}
		else
		{
			return true;
		}
	}
    #endregion

	#region Set Methods
	public static void SetIntValue ( string thisString, int thisValue, bool addition = true )
	{
		if ( addition )
		{
			PlayerPrefs.SetInt ( thisString, GetIntValue ( thisString ) + thisValue );
		}
		else
		{
			PlayerPrefs.SetInt ( thisString, thisValue );
		}
	}

	public static void SetStringValue ( string thisName, string value = "ok" )
	{
		PlayerPrefs.SetString ( thisName, value );
	}
	#endregion
}
