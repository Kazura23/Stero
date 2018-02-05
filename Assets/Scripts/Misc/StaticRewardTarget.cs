using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public static class StaticRewardTarget {

    //enfant de game controller
    public static Transform listRewardTrans;
    //icon succes
    public static Sprite icon_sucess;
    //list de lecture reward pendant une run
    public static List<GameObject> ListRewardEnAttente;

    // var pendant une run
    public static int SCharlotteLV;

    public static int SDanielLV;

    public static int SVinoLV;

    public static int SScoreLV;

    public static int SScoreWithoutPunchAndTechSpec;

    public static float STimerSlowMo;

    public static int SSizeMagicSphere;

    public static float STimerMadness;

    public static int SNbObstacleDestoyInMadness;

    public static int SRedMadness;

    public static int SRankSteroidal;


    //private var
    private static int nbKillCharlotteLv1;
    private static int nbKillCharlotteLv2;
    private static int nbKillCharlotteLv3;
    private static int nbKillDanielLv1;
    private static int nbKillDanielLv2;
    private static int nbKillVinoLv1;
    private static int nbKillVinoLv2;
    private static int scoreObjectifLv1;
    private static int scoreObjectifLv2;
    private static int scoreObjectifLv3;
    private static int scoreSansPoingNiTechSpe;
    private static float TempsObjectifSlowMtion;
    private static int TailleBouleHumaine;
    private static float TempsPasserEnMadness;
    private static int nbPropSafeDetruitEnMadness;
    private static int nbDansLeRougeMadness;
    private static int nbRangSteroidal;

    public static void SetObjectifReward(int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8, int p9, int p10, int p11, float p12, int p13, float p14, int p15, int p16, int p17)
    {
        nbKillCharlotteLv1 = p1;
        nbKillCharlotteLv2 = p2;
        nbKillCharlotteLv3 = p3;
        nbKillDanielLv1 = p4;
        nbKillDanielLv2 = p5;
        nbKillVinoLv1 = p6;
        nbKillVinoLv2 = p7;
        scoreObjectifLv1 = p8;
        scoreObjectifLv2 = p9;
        scoreObjectifLv3 = p10;
        scoreSansPoingNiTechSpe = p11;
        TempsObjectifSlowMtion = p12;
        TailleBouleHumaine = p13;
        TempsPasserEnMadness = p14;
        nbPropSafeDetruitEnMadness = p15;
        nbDansLeRougeMadness = p16;
        nbRangSteroidal = p17;
}

    public static void ResetVar()
    {
        SCharlotteLV = 0;
        SDanielLV = 0;
        SVinoLV = 0;
        SScoreLV = 0;
        SScoreWithoutPunchAndTechSpec = 0;
        STimerSlowMo = 0;
        SSizeMagicSphere = 0;
        STimerMadness = 0;
        SNbObstacleDestoyInMadness = 0;
        SRedMadness = 0;
        SRankSteroidal = 0;
    }

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

    private static void LoadUnlockReward(RewardObject p_reward)
    {
        p_reward.Unlock();
        p_reward.icon.sprite = icon_sucess;
        ListRewardEnAttente.Add(GameObject.Instantiate(p_reward.afficheReward.gameObject, p_reward.afficheReward.parent.parent));

    }

    private static void LoadRewardId(RewardObject p_reward)
    {
        if (!p_reward.isUnlock) {
            switch (p_reward.idReward)
            {
                //lier au kill charlotte
                case 0:
                    if (SCharlotteLV >= nbKillCharlotteLv1)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                case 1:
                    if (SCharlotteLV >= nbKillCharlotteLv2)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                case 2:
                    if (SCharlotteLV >= nbKillCharlotteLv3)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier au kill daniel
                case 3:
                    if (SDanielLV >= nbKillDanielLv1)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                case 4:
                    if (SDanielLV >= nbKillDanielLv2)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier au kill vino
                case 5:
                    if (SVinoLV >= nbKillVinoLv1)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                case 6:
                    if (SVinoLV >= nbKillVinoLv2)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier au score
                case 7:
                    if (SScoreLV >= scoreObjectifLv1)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                case 8:
                    if (SScoreLV >= scoreObjectifLv2)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                case 9:
                    if (SScoreLV >= scoreObjectifLv3)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier au score hardcore
                case 10:
                    if (SScoreWithoutPunchAndTechSpec >= scoreSansPoingNiTechSpe)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier au temps slow motion
                case 11:
                    if (STimerSlowMo >= TempsObjectifSlowMtion)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier a la size de la boule magic
                case 12:
                    if (SSizeMagicSphere >= TailleBouleHumaine)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier au temps passer en madness
                case 13:
                    if (STimerMadness >= TempsPasserEnMadness)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier au obstacle detruit en madness
                case 14:
                    if (SNbObstacleDestoyInMadness >= nbPropSafeDetruitEnMadness)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier a la jauge de madness (dans le rouge)
                case 15:
                    if (SRedMadness >= nbDansLeRougeMadness)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
                //lier au rang S steroidal
                case 16:
                    if (SRankSteroidal >= nbRangSteroidal)
                    {
                        LoadUnlockReward(p_reward);
                    }
                    break;
            }
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
            fSave.Close();
            return l;
            //GameObject.Find("Trash_text").GetComponent<Text>().text = l.listScore.Count > 0 ? "score = "+l.listScore[0].finalScore : "no save";
        }
        return null;
    }
}

[System.Serializable]
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