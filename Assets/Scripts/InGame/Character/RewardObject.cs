﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardObject : MonoBehaviour {

    public int idReward;
    private bool unlock = false;
    //public GameObject model;
    public AudioScriptable[] voice;

    public Transform afficheReward;

    [HideInInspector]
    public Image icon;

    private void Awake()
    {
        icon = afficheReward.GetChild(0).GetChild(3).GetComponent<Image>();
    }

    public void Unlock() // peut etre a dupliquer pour les succes steam
    {
        if (!unlock)
        {
            unlock = true;
            transform.GetChild(0).gameObject.SetActive(true);
            if(voice != null && voice.Length > 0)
            {
                // voir integrer voix
            }
            StaticRewardTarget.SaveReward();
        }
    }

    public void UnlockWithFile() // peut etre a dupliquer pour les succes steam
    {
        if (!unlock)
        {
            unlock = true;
            transform.GetChild(0).gameObject.SetActive(true);
            if (voice != null && voice.Length > 0)
            {
                // voir integrer voix
            }
        }
    }

    public bool isUnlock
    {
        get
        {
            return unlock;
        }
    }
}
