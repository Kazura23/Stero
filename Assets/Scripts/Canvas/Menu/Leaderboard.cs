using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : UiParent
{
    public Text score;

    public override MenuType ThisMenu
    {
        get
        {
            return MenuType.Leaderboard;
        }
    }

    protected override void InitializeUi()
    {
        
    }

    public override void OpenThis(MenuTokenAbstract GetTok = null)
    {
        base.OpenThis(GetTok);
        score.text = "";
        for(int i = 0; i < AllPlayerPrefs.saveData.listScore.Count; i++)
        {
            DataSave temp = AllPlayerPrefs.saveData.listScore[i];
            score.text += "Score total = "+temp.finalScore+" / score baston = "+temp.score+" / distance = "+temp.distance+" / piece recuperer = "+temp.piece+" \n";
        }
    }

	public override void CloseThis ( )
	{
		base.CloseThis();
	}
}
