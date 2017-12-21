using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyPathBonus : AbstractObject {

    public List<Transform> tabPath = new List<Transform>();
    private int currentMove;
    public int speed = 8;
    private bool moveFinish = false, active = false;
    public CoinBonusScriptable coin;
    public float dist = 15;




    protected override void Update()
    {
        base.Update();
        if(!active && Vector3.Distance(transform.position, playerTrans.position) <= dist)
        {
            active = true;
            currentMove = 0;
            MovePath();
        }
    }

    private void MovePath()
    {
        var trans = transform.parent.GetChild(1);
        this.transform.DOMove(trans.GetChild(currentMove).position, Vector3.Distance(transform.position, trans.GetChild(currentMove).position) / speed).SetEase(Ease.Linear).OnComplete(()=>
        {
            currentMove++;
            if (currentMove < trans.childCount)
            {
                MovePath();
            }
            else
            {
                moveFinish = true;
            }
        });
    }

    public void AddNewPointPath()
    {
        var tempPath = new GameObject("point" + tabPath.Count).transform;
        if (tabPath.Count == 0)
        {
            tempPath.position = transform.position;
        }
        else
        {
            tempPath.position = tabPath[tabPath.Count - 1].position;
        }
        tabPath.Add(tempPath);
        tempPath.parent = transform.parent.GetChild(1);
    }

    public void RemoveLastPointPath()
    {
        if(tabPath.Count > 0)
        {
            var tempPath = tabPath[tabPath.Count - 1];
            tabPath.RemoveAt(tabPath.Count - 1);
            DestroyImmediate(tempPath.gameObject);
        }
    }

    public override void Dead(bool enemy = false)
    {
        if (!moveFinish)
        {
            Instantiate(coin.CoinSpawn[0], transform.position, Quaternion.identity);
        }
        base.Dead(enemy);
    }
}
