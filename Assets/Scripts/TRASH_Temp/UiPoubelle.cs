﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPoubelle : MonoBehaviour {

    public ShieldMan enemy;
    public Vector3 propulsion, prop2;
    private GameObject missi;

    private void Awake()
    {
        Debug.Log("path : " + Application.dataPath);
    }

    public void onActive()
    {
        GameObject.Find("ShieldMan").GetComponent<ShieldMan>().Degat(propulsion, 1);
        /*missi = GameObject.FindGameObjectWithTag(Constants._MissileBazoo);
        missi.GetComponent<MissileBazooka>().ActiveTir(-missi.GetComponent<MissileBazooka>().GetDirection(), 1.5f, true);*/
    }


    public void onActive2()
    {
        GameObject.Find("ShieldMan").GetComponent<ShieldMan>().Degat(prop2, 0);
    }
    public void onActiveRagdoll()
    {
        GameObject champi = GameObject.Find("Enemy_Shoot");
        champi.GetComponentInChildren<Animator>().enabled = false;
        //champi.GetComponent<Rigidbody>().AddForce(propulsion, ForceMode.VelocityChange);
        Rigidbody[] rigis = champi.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rig in rigis)
        {
            rig.useGravity = true;
            if(rig.gameObject.name == "Vino_veritas_Hips"/* && rig.gameObject.name == "charlotte_champi1_Head"*/)
            {
                rig.AddForce(propulsion, ForceMode.VelocityChange);
            }
        }
    }

    public void onActiveRagdoll2()
    {
        GameObject champi = GameObject.Find("Enemy_Normal");
        champi.GetComponentInChildren<Animator>().enabled = false;
        //champi.GetComponent<Rigidbody>().AddForce(propulsion, ForceMode.VelocityChange);
        Rigidbody[] rigis = champi.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rig in rigis)
        {
            rig.useGravity = true;
            if (rig.gameObject.name == "Character1_Hips"/* && rig.gameObject.name == "charlotte_champi1_Head"*/)
            {
                rig.AddForce(propulsion, ForceMode.VelocityChange);
            }
        }
    }

    public void onInterrupt()
    {
        GameObject.Find("Interrupt").GetComponent<PunchInterrupt>().Activation();
    }

}