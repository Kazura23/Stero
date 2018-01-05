using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Rewired;
using DG.Tweening;


public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Singleton;

    public int playerID;

    Player player;

    [Range(0,3)]
    public float ObsPower = .12f, ObsDuration = .2f, CoupSimplePower = .15f, CoupSimpleDuration = .25f, CoupDoublePower = .5f, CoupDoubleDuration = .5f, GameOverPower = 2.5f, GameOverDuration = 1f, ShockWavesPower = .9f, ShockWavesDuration = .25f, FleshBallPower = .7f, FleshBallDuration = 3;

    void Awake()
    {
        if (VibrationManager.Singleton == null)
        {
            VibrationManager.Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        player = ReInput.players.GetPlayer(0);
    }

    public void GameOverVibration()
    {
        foreach (Joystick j in player.controllers.Joysticks)
        {
            j.StopVibration();
            j.SetVibration(0, GameOverPower, true);
            DOVirtual.DelayedCall(GameOverDuration, () =>
            {
                j.StopVibration();
            });
        }
        
    }


    public void CoupSimpleVibration()
    {

        foreach (Joystick j in player.controllers.Joysticks)
            {
                j.SetVibration(0, CoupSimplePower,false);
                DOVirtual.DelayedCall(CoupSimpleDuration, () =>
                {
                   j.StopVibration();
              });
          }
    }


    public void CoupDoubleVibration()
    {

        foreach (Joystick j in player.controllers.Joysticks)
        {
            j.SetVibration(1, CoupDoublePower,false);
            DOVirtual.DelayedCall(CoupDoubleDuration, () =>
            {
                j.StopVibration();
            });
        }
    }


    public void ObsVibration()
    {

        foreach (Joystick j in player.controllers.Joysticks)
        {
            j.SetVibration(0, ObsPower, false);
            DOVirtual.DelayedCall(ObsDuration, () =>
            {
                j.StopVibration();
            });
        }
    }

    public void ShockWaveVibration()
    {
        foreach (Joystick j in player.controllers.Joysticks)
        {
            j.SetVibration(2, ShockWavesPower, true);
            DOVirtual.DelayedCall(ShockWavesDuration, () =>
            {
                j.StopVibration();
            });
        }

    }

    public void FleshBallVibration()
    {
        foreach (Joystick j in player.controllers.Joysticks)
        {
            j.SetVibration(2, FleshBallPower, true);
            DOVirtual.DelayedCall(FleshBallDuration, () =>
            {
                j.StopVibration();
            });
        }

    }
   
}
