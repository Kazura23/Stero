using UnityEngine;

public static class AllPlayerPrefs
{
    public static int piece;
    public static int finalScore;
    public static int scoreWhithoutDistance;
    public static float distance;
    public static ListData saveData;
    public static bool relance;

    public static void ResetStaticVar()
    {
        piece = 0;
        finalScore = 0;
        scoreWhithoutDistance = 0;
        distance = 0;
    }

    public static DataSave NewData()
    {
        return new DataSave(finalScore, scoreWhithoutDistance, piece, distance);
    }

    #region Get Methods
	public static int GetIntValue ( string thisString )
	{
		return PlayerPrefs.GetInt ( thisString, 10000 );
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
