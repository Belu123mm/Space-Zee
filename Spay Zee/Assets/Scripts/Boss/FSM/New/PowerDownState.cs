﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class PowerDownState : MonoBaseState
{
    public GameObject objective;
    public GameObject batery;
    Boss boss;

    float reloadTimer;

    public override event Action OnNeedsReplan;

    public PowerDownState(GameObject _objective, GameObject _batery, Boss _boss, Action onNeedsReplan)
    {
        boss = _boss;
        objective = _objective;
        batery = _batery;
        OnNeedsReplan = onNeedsReplan;
    }
    public override void UpdateLoop() 
    {
            reloadTimer += Time.deltaTime;

            batery.SetActive(true);

            boss.transform.position = Vector3.MoveTowards(boss.transform.position, objective.transform.position, Time.deltaTime * 2);
           
     }
    public override IState ProcessInput()
    {
        if (reloadTimer >= 5 && Transitions.ContainsKey("OnChargeState"))
        {
            batery.SetActive(false);
            reloadTimer = 0;
            boss.overheatingCounter = 0;
            return Transitions["OnChargeState"];
        }
        else if (reloadTimer >= 6 && Transitions.ContainsKey("OnInvokeWaveState"))
        {
            batery.SetActive(false);
            reloadTimer = 0;
            boss.overheatingCounter = 0;
            OnNeedsReplan?.Invoke();
            return Transitions["OnInvokeWaveState"];
        }
        else if(reloadTimer >= 7)
        {
            batery.SetActive(false);
            reloadTimer = 0;
            boss.overheatingCounter = 0;
            OnNeedsReplan?.Invoke();
        }


        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        boss.Mood = BossMood.PoweredUp;
        reloadTimer = 0;
        return base.Exit(to);
    }
}