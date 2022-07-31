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

    public ChargeState(Boss _boss, Model player, GameObject _warning, GameObject _triggerCollider)
    {
        boss = _boss;
        _player = player;
        warning = _warning;
        triggerCollider = _triggerCollider;
    } 

    public override void UpdateLoop()
    {
        if(start)
        {
            Vector3 lookAtPos = _player.transform.position;
            lookAtPos.z = boss.transform.position.z;
            boss.transform.up = lookAtPos - boss.transform.position;

            if (timer >= 2f)
            {
                warning.SetActive(false);
                GetLocation();
                counter++;
                timer = 0;
            }

            else if (timer > 1)
            {
                triggerCollider.SetActive(true);
                warning.SetActive(true);
            }
        }

        if (counter >= 4)
        {
            warning.SetActive(false);
        }

        if (start)
        {
            timer += Time.deltaTime;

            if (hasLocation)
            {
                boss.transform.position = Vector3.MoveTowards(boss.transform.position, location, Time.deltaTime * 20);
            }
        }
    }

    public void GetLocation()
    {
        location = _player.transform.position;
        hasLocation = true;
    }

    public override IState ProcessInput()
    {
        if (boss.IsPlayerClose() && Transitions.ContainsKey("OnPushPlayerState"))
        {
            triggerCollider.SetActive(false);
            return Transitions["OnPushPlayerState"];
        }

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        start = true;

        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to) 
    {
        timer = 0;
        start = false;
        hasLocation = false;
        counter = 0;
        return base.Exit(to);
    }
}
