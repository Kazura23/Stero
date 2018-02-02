using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class StaticRewardTarget {

    //enfant de game controller
    public static Transform listRewardTrans;

    // var pendant une run
    public static ulong SCharlotteLV;

    public static ulong SDanielLV;

    public static ulong SVinoLV;

    public static ulong SScoreLV;

    public static ulong SScoreWithoutPunchAndTechSpec;

    public static float STimerSlowMo;

    public static int SSizeMagicSphere;

    public static float STimerMadness;

    public static int SNbObstacleDestoyInMadness;

    public static int SRedMadness;

    public static int SRankSteroidal;



    public static void SaveReward()
    {
        List<RewardObject> saveRewar = new List<RewardObject>();
        for(int i = 0; i < listRewardTrans.childCount; i++)
        {
            saveRewar.Add(listRewardTrans.GetChild(i).GetComponent<RewardObject>());
        }
        SaveDataReward.Save(saveRewar);
    }

    public static void LoadReward()
    {
        for(int i = 0; i < listRewardTrans.childCount; i++)
        {
            var tempReward = listRewardTrans.GetChild(i).GetComponent<RewardObject>();
            LoadRewardId(tempReward);
        }
    }

    private static void LoadRewardId(RewardObject p_reward)
    {
        switch (p_reward.idReward)
        {
            //lier au kill charlotte
            case 0:
                if (SCharlotteLV >= 50)
                {
                    p_reward.Unlock();
                }
                break;
            case 1:
                if (SCharlotteLV >= 500)
                {
                    p_reward.Unlock();
                }
                break;
            case 2:
                if (SCharlotteLV >= 1000)
                {
                    p_reward.Unlock();
                }
                break;
            //lier au kill daniel
            case 3:
                if (SDanielLV >= 5)
                {
                    p_reward.Unlock();
                }
                break;
            case 4:
                if (SDanielLV >= 30)
                {
                    p_reward.Unlock();
                }
                break;
            //lier au kill vino
            case 5:
                if (SVinoLV >= 25)
                {
                    p_reward.Unlock();
                }
                break;
            case 6:
                if (SVinoLV >= 300)
                {
                    p_reward.Unlock();
                }
                break;
            //lier au score
            case 7:
                if (SScoreLV >= 100000)
                {
                    p_reward.Unlock();
                }
                break;
            case 8:
                if (SScoreLV >= 500000)
                {
                    p_reward.Unlock();
                }
                break;
            case 9:
                if (SScoreLV >= 1000000)
                {
                    p_reward.Unlock();
                }
                break;
            //lier au score hardcore
            case 10:
                if (SScoreWithoutPunchAndTechSpec >= 100000)
                {
                    p_reward.Unlock();
                }
                break;
            //lier au temps slow motion
            case 11:
                if (STimerSlowMo >= 3)
                {
                    p_reward.Unlock();
                }
                break;
            //lier a la size de la boule magic
            case 12:
                if (SSizeMagicSphere >= 50)
                {
                    p_reward.Unlock();
                }
                break;
            //lier au temps passer en madness
            case 13:
                if (STimerMadness >= 66)
                {
                    p_reward.Unlock();
                }
                break;
            //lier au obstacle detruit en madness
            case 14:
                if (SNbObstacleDestoyInMadness >= 20)
                {
                    p_reward.Unlock();
                }
                break;
            //lier a la jauge de madness (dans le rouge)
            case 15:
                if (SRedMadness >= 10)
                {
                    p_reward.Unlock();
                }
                break;
            //lier au rang S steroidal
            case 16:
                if (SRankSteroidal >= 5)
                {
                    p_reward.Unlock();
                }
                break;
        }
    }
}

#region Save_Reward
public static class SaveDataReward
{
    public static void Save(List<RewardObject> p_dataSave)
    {
        if (!Directory.Exists(Application.dataPath + "/Save"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Save");
        }

        List<RewardSaveAttribut> listSave = new List<RewardSaveAttribut>();
        for(int i = 0; i < p_dataSave.Count; i++)
        {
            var temp = p_dataSave[i];
            listSave.Add(new RewardSaveAttribut(temp.idReward, temp.isUnlock));
        }

        string path1 = Application.dataPath + "/Save/saveReward.bin";
        FileStream fSave = File.Create(path1);
        listSave.SerializeTo(fSave);
        fSave.Close();
        //GameObject.Find("Trash_text").GetComponent<Text>().text = "save";
    }

    public static List<RewardSaveAttribut> Load()
    {
        if (!Directory.Exists(Application.dataPath + "/Save"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Save");
        }
        string path1 = Application.dataPath + "/Save/saveReward.bin";
        List<RewardSaveAttribut> l;// = new List<RewardSaveAttribut>();
        if (File.Exists(path1))
        {
            FileStream fSave = File.Open(path1, FileMode.Open, FileAccess.ReadWrite);
            l = fSave.Deserialize<List<RewardSaveAttribut>>();
            return l;
            //GameObject.Find("Trash_text").GetComponent<Text>().text = l.listScore.Count > 0 ? "score = "+l.listScore[0].finalScore : "no save";
        }
        return null;
    }
}

public class RewardSaveAttribut
{
    public int id;
    public bool unlock;
    public RewardSaveAttribut(int p_id, bool p_u)
    {
        id = p_id;
        unlock = p_u;
    }
}
#endregion