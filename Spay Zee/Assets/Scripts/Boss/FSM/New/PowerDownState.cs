using System.Collections;
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
        if (reloadTimer >= 5)
        {
            if (!boss.IsLaserOnCD && boss.overheatingCounter <= 3 && boss.Mood == BossMood.PoweredUp && Transitions.ContainsKey("OnLaserAttackState"))
            {
                return Transitions["OnLaserAttackState"];
            }
            else if(boss.overheatingCounter<5 && boss.IsPlayerClose() && Transitions.ContainsKey("OnPushPlayerState"))
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
        boss.Mood = BossMood.PoweredUp;
        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        batery.SetActive(false);
        reloadTimer = 0;
        boss.overheatingCounter = 0;

        return base.Exit(to);
    }
}