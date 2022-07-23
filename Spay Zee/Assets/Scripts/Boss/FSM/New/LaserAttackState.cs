using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class LaserAttackState : MonoBaseState
{
    public GameObject objective;
    public GameObject forwardLazer;
    public GameObject redLazer;

    public Boss boss;

    public override event Action OnNeedsReplan;

    bool inPosition = false;
    bool start;

    float timer;

    private void Awake()
    {
        inPosition = false;
    }

    public void Update()
    {
        if (start)
            timer += Time.deltaTime;
    }

    public override void UpdateLoop()
    {
        if (start == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, objective.transform.position, Time.deltaTime * 10);

            if (inPosition == false)
            {
                Vector3 lookAtPos = objective.transform.position;
                lookAtPos.z = transform.position.z;
                transform.up = lookAtPos - transform.position;
            }

            if (transform.position == objective.transform.position)
            {
                inPosition = true;
                forwardLazer.SetActive(true);

                Vector3 lookAtPos = forwardLazer.transform.position;
                lookAtPos.z = transform.position.z;
                transform.up = lookAtPos - transform.position;
            }
        }

        if (timer >= 4f)
            redLazer.SetActive(true);

        if (timer >= 5.5f)
            redLazer.SetActive(false);
    }

    public override IState ProcessInput()
    {
        if (timer >= 6.30f && BossWorldState.instance.IsPlayerClose() && Transitions.ContainsKey("OnPushPlayerState"))
        {
            boss.powerCounter++;
            timer = 0;
            start = false;
            forwardLazer.SetActive(false);
            BossWorldState.instance.SetPowerUpBoss(false);
            OnNeedsReplan?.Invoke();
            return Transitions["OnPushPlayerState"];
        }

        if (timer >= 6.35f)
        {
            boss.powerCounter++;
            timer = 0;
            start = false;
            forwardLazer.SetActive(false);
            BossWorldState.instance.SetPowerUpBoss(false);
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