using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class PowerDownState : MonoBaseState
{
    public GameObject objective;
    public GameObject batery;
    public Boss boss;

    bool start;   
    float reloadTimer;

    public override event Action OnNeedsReplan;

    public override void UpdateLoop() 
    {

    }
     void Update()
     {
        if (start)
        {
            reloadTimer += Time.deltaTime;

            batery.SetActive(true);

            transform.position = Vector3.MoveTowards(transform.position, objective.transform.position, Time.deltaTime * 2);
        }      
     }
    public override IState ProcessInput()
    {
        if (reloadTimer >= 5 && Transitions.ContainsKey("OnChargeState"))
        {
            batery.SetActive(false);
            start = false;
            reloadTimer = 0;
            boss.powerCounter = 0;
            return Transitions["OnChargeState"];
        }
        else if (reloadTimer >= 6 && Transitions.ContainsKey("OnInvokeWaveState"))
        {
            batery.SetActive(false);
            start = false;
            reloadTimer = 0;
            boss.powerCounter = 0;
            OnNeedsReplan?.Invoke();
            return Transitions["OnInvokeWaveState"];
        }
        else if(reloadTimer >= 7)
        {
            batery.SetActive(false);
            start = false;
            reloadTimer = 0;
            boss.powerCounter = 0;
            OnNeedsReplan?.Invoke();
        }


        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        start = true;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        start = false;
        reloadTimer = 0;
        return base.Exit(to);
    }
}