﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using FSM;
using Object = UnityEngine.Object;


public class InvokeWaveState : MonoBaseState
{
    GameObject enemySpawner;
    GameObject BossBase;
    GameObject BossHide;
    private GameObject Warning;

    Boss boss;

    public override event Action OnNeedsReplan;

    float timer;
    bool InPosition;
    private bool Hiden;
    private bool spawned;
    int invokeReEnterCounter;

    float auxTim = 0;
    public InvokeWaveState(Boss _boss, GameObject _enemySpawner, GameObject _bossBase, GameObject _bossHide, Action onNeedsReplan)
    {
        boss = _boss;
        enemySpawner = _enemySpawner;
        BossBase = _bossBase;
        BossHide = _bossHide;
        OnNeedsReplan = onNeedsReplan;
        InPosition = false;
        spawned = false;
        Hiden = false;
        timer = 0;
        auxTim = 0;
        enemySpawner.GetComponent<EnemySpawner>().maxWaves = 1000;
    }

    public override void UpdateLoop()
    {
        if (!InPosition)
        {
            if (boss.transform.position != BossHide.transform.position)
            {
                boss.transform.position = Vector3.MoveTowards(boss.transform.position, BossHide.transform.position, Time.deltaTime * 10);

                Vector3 lookAtPos = BossHide.transform.position;
                lookAtPos.z = boss.transform.position.z;
                boss.transform.up = lookAtPos - boss.transform.position;

                return;
            }
        }
        if ((boss.transform.position == BossHide.transform.position) && Hiden == false)
        {
            InPosition = true;
            Hiden = true;
            Warning = GameObject.Instantiate(Resources.Load("Art/MISC/warningEffect/warningmessage", typeof(Object))) as GameObject;
            Warning.transform.position = new Vector3(-10.08f, 0, 0);
            GameObject.Destroy(Warning, 2.3f);
            return;
        }

        if (InPosition && Hiden)
        {
            if (!spawned)
            {
                auxTim += Time.deltaTime;
                if (auxTim >= 3.1f)
                {
                    enemySpawner.GetComponent<EnemySpawner>().Spawn();
                    spawned = true;
                }
            }
            if (timer >= 6f)
            {
                Vector3 lookAtPos = BossBase.transform.position;
                lookAtPos.z = boss.transform.position.z;
                boss.transform.up = lookAtPos - boss.transform.position;

                boss.transform.position = Vector3.MoveTowards(boss.transform.position, BossBase.transform.position, Time.deltaTime * 2);
                if ((boss.transform.position == BossBase.transform.position)) Hiden = false;
            }
        }

        if (spawned)
        {
            timer += Time.deltaTime;
            if (boss.life < 100f)
            {
                boss.life += Time.deltaTime * 1.5f;
            }
            else { boss.life = 100f; }
        }
    }

    public override IState ProcessInput()
    {
        if (timer >= 8)
        {
            if (boss.overheatingCounter < 5 && boss.Mood == BossMood.Calm && Transitions.ContainsKey("OnAttackState"))
            {
                return Transitions["OnAttackState"];
            }
            else if (boss.overheatingCounter <= 5 && boss.IsPlayerClose() && Transitions.ContainsKey("OnPushPlayerState"))
            {
                return Transitions["OnPushPlayerState"];
            }
            else if (!boss.IsPlayerClose() && Transitions.ContainsKey("OnChargeState"))
            {
                return Transitions["OnChargeState"];
            }
            else
                OnNeedsReplan?.Invoke();
        }
        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        timer = 0;
        InPosition = false;
        boss.overheatingCounter++;
        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        Hiden = false;
        enemySpawner.SetActive(false);
        boss.Mood = BossMood.Calm;
        spawned = false;
        auxTim = 0;
        return base.Exit(to);
    }
}
