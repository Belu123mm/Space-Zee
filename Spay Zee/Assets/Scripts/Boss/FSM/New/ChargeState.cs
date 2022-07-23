using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class ChargeState : MonoBaseState
{
    private Model _player;

    public GameObject warning;
    public GameObject triggerCollider;

    int counter; 
    float timer;
    bool start;
    bool hasLocation;

    Vector3 location;

    private void Awake()
    {
        _player = FindObjectOfType<Model>();
    }

    public override void UpdateLoop()
    {
        if(start)
        {
            Vector3 lookAtPos = _player.transform.position;
            lookAtPos.z = transform.position.z;
            transform.up = lookAtPos - transform.position;

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
    }

    private void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;

            if (hasLocation)
            {
                transform.position = Vector3.MoveTowards(transform.position, location, Time.deltaTime * 20);
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
        if (BossWorldState.instance.IsPlayerClose() && Transitions.ContainsKey("OnPushPlayerState"))
        {
            triggerCollider.SetActive(false);
            return Transitions["OnPushPlayerState"];
        }

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        start = true;
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
