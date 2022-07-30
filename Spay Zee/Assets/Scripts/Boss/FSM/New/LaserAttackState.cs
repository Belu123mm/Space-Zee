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
    bool start;

    float timer;

    public LaserAttackState(Boss _boss, GameObject _objective, GameObject _forwardLazer, GameObject _redLaser)
    {
        boss = _boss;
        objective = _objective;
        forwardLazer = _forwardLazer;
        redLaser = _redLaser;
        inPosition = false;
    }    

    public override void UpdateLoop()
    {
        if (start)
            timer += Time.deltaTime;

        if (start == true)
        {
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
        }

        if (timer >= 4f)
            redLaser.SetActive(true);

        if (timer >= 5.5f)
            redLaser.SetActive(false);
    }

    public override IState ProcessInput()
    {
        if (timer >= 6.30f && boss.IsPlayerClose() && Transitions.ContainsKey("OnPushPlayerState"))
        {
            boss.powerCounter++;
            timer = 0;
            start = false;
            forwardLazer.SetActive(false);
            boss.SetPowerUpBoss(false);
            OnNeedsReplan?.Invoke();
            return Transitions["OnPushPlayerState"];
        }

        if (timer >= 6.35f)
        {
            boss.powerCounter++;
            timer = 0;
            start = false;
            forwardLazer.SetActive(false);
            boss.SetPowerUpBoss(false);
            OnNeedsReplan?.Invoke();
        }
        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
         timer = 0;
         start = true;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        return base.Exit(to);
    }
}