using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class LaserAttackState : MonoBaseState
{
    Boss boss;
    GameObject objective;
    GameObject forwardLazer;
    GameObject redLaser;

    public override event Action OnNeedsReplan;

    bool inPosition = false;

    float timer;

    public LaserAttackState(Boss _boss, GameObject _objective, GameObject _forwardLazer, GameObject _redLaser, Action onNeedsReplan)
    {
        boss = _boss;
        objective = _objective;
        forwardLazer = _forwardLazer;
        redLaser = _redLaser;
        inPosition = false;
        timer = 0;
        OnNeedsReplan = onNeedsReplan;
    }

    public override void UpdateLoop()
    {
        timer += Time.deltaTime;

        boss.transform.position = Vector3.MoveTowards(boss.transform.position, objective.transform.position, Time.deltaTime * 10);

        if (inPosition == false)
        {
            Vector3 lookAtPos = objective.transform.position;
            lookAtPos.z = boss.transform.position.z;
            boss.transform.up = lookAtPos - boss.transform.position;
        }

        if (boss.transform.position == objective.transform.position)
        {
            inPosition = true;
            forwardLazer.SetActive(true);

            Vector3 lookAtPos = forwardLazer.transform.position;
            lookAtPos.z = boss.transform.position.z;
            boss.transform.up = lookAtPos - boss.transform.position;
        }


        if (timer >= 4f)
            redLaser.SetActive(true);

        if (timer >= 5.5f)
            redLaser.SetActive(false);
    }

    public override IState ProcessInput()
    {
        if (timer >= 6.30f)
        {
            if (boss.life < 50 && boss.overheatingCounter <= 3 && boss.Mood == BossMood.Angry && Transitions.ContainsKey("OnInvokeWaveState"))
            {
                return Transitions["OnInvokeWaveState"];
            }
            if (boss.overheatingCounter <= 5 && boss.IsPlayerClose() && Transitions.ContainsKey("OnPushPlayerState"))
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
        boss.overheatingCounter++;
        boss.ResetLaserCD();
        boss.Mood = BossMood.Angry;
        timer = 0;
        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        forwardLazer.SetActive(false);
        return base.Exit(to);
    }
}