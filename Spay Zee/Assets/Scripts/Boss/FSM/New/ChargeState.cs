using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class ChargeState : MonoBaseState
{
    Boss boss;
    Model _player;

    GameObject warning;
    GameObject triggerCollider;

    int counter;
    float timer;
    bool start;
    bool hasLocation;

    Vector3 location;

    public override event Action OnNeedsReplan;

    public ChargeState(Boss _boss, Model player, GameObject _warning, GameObject _triggerCollider, Action onNeedsReplan)
    {
        boss = _boss;
        _player = player;
        warning = _warning;
        triggerCollider = _triggerCollider;
        OnNeedsReplan = onNeedsReplan;
    }

    public override void UpdateLoop()
    {

        if (timer >= 2f)
        {
            if (!hasLocation)
                GetLocation();
            warning.SetActive(false);
            counter++;
        }
        else if (timer > 1)
        {
            triggerCollider.SetActive(true);
            warning.SetActive(true);
        }

        if (counter >= 4)
        {
            warning.SetActive(false);
        }

        timer += Time.deltaTime;

        Vector3 lookAtPos = _player.transform.position;

        if (!hasLocation)
        {
            lookAtPos.z = boss.transform.position.z;
            boss.transform.up = lookAtPos - boss.transform.position;
        }
        else
        {
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, location, Time.deltaTime * 5);
            boss.transform.up = (location - boss.transform.position);
        }
    }

    public void GetLocation()
    {
        Vector3 dir = _player.transform.position - boss.transform.position;

        location = boss.transform.position + dir.normalized * 3;
        hasLocation = true;
    }

    public override IState ProcessInput()
    {
        if (timer >= 2.5f)
        {
            if (!boss.IsLaserOnCD && boss.overheatingCounter <= 3 && boss.Mood == BossMood.PoweredUp)
            {
                OnNeedsReplan?.Invoke();
            }
            else if (boss.life < 50 && boss.overheatingCounter <= 3 && boss.Mood == BossMood.Angry && Transitions.ContainsKey("OnInvokeWaveState"))
            {
                return Transitions["OnInvokeWaveState"];
            }
            if (boss.IsPlayerClose() && Transitions.ContainsKey("OnPushPlayerState"))
            {
                return Transitions["OnPushPlayerState"];
            }
            else if (boss.overheatingCounter < 5 && boss.Mood == BossMood.Calm && Transitions.ContainsKey("OnAttackState"))
            {
                return Transitions["OnAttackState"];
            }
            OnNeedsReplan?.Invoke();
        }
        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        hasLocation = false;
        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        timer = 0;
        hasLocation = false;
        counter = 0;
        triggerCollider.SetActive(false);
        warning.SetActive(false);
        return base.Exit(to);
    }
}
