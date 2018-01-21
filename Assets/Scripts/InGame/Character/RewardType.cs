using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardType : MonoBehaviour {

    public enum TypeReward
    {
        succes,
        defis
    }

    public TypeReward typeReward;
    public int idReward;
    public GameObject model;
    [HideInInspector]

    // si succes
    private bool unlock;

    // si defis
    private bool unlockFinal;
    private int rank;
    private int progress;

    public void Load()
    {
        RewardOption.LoadReward(typeReward == TypeReward.succes ? true : false, idReward, this);
    }

    public void InitReward(bool unlockSucces, bool unlockDefis, int rankDefis, int progressDefis)
    {
        unlock = unlockSucces;
        unlockFinal = unlockDefis;
        rank = rankDefis;
        progress = progressDefis;

        if (typeReward == TypeReward.succes && unlock)
            transform.GetChild(0).gameObject.SetActive(true);
    }

    public bool isUnlock
    {
        get
        {
            return unlock;
        }
    }
}
