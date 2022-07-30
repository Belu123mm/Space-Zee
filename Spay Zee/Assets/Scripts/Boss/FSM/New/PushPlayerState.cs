using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Object = UnityEngine.Object;

public class PushPlayerState : MonoBaseState
{
    private GameObject Shield;
    private Animator ShieldAnimator;
    private Rigidbody ShieldBody;

    Model _player;
    
    float timer = 0;
    int invokeCounter;
    bool start;
    public override event Action OnNeedsReplan;

    Boss boss;
    public PushPlayerState(Boss _boss, Model player)
    {
        boss = _boss;
        _player = player;

        Shield = (GameObject)GameObject.Instantiate(Resources.Load("Art/MISC/shieldEffect/BossShield", typeof(Object)));
        Shield.transform.SetParent(boss.transform);
        Shield.transform.position = boss.transform.position;
        Shield.transform.rotation = boss.transform.rotation;
        ShieldBody= Shield.GetComponent<Rigidbody>();
        ShieldAnimator = Shield.GetComponent<Animator>();
    }

    public override void UpdateLoop()
    {
        if (start)
        {
            ShieldAnimator.Play("push");

            Vector3 lookAtPos = _player.transform.position;
            lookAtPos.z = boss.transform.position.z;
            boss.transform.up = lookAtPos - boss.transform.position;

            if (!boss.CheckBossLife() && boss.CheckBossEnergy())
            {
                start = false;
                timer = 0;
                OnNeedsReplan?.Invoke();
            }
        }
  
        if (start)
        {
            timer += Time.deltaTime;

            boss.transform.position = Vector3.MoveTowards(boss.transform.position, _player.transform.position, Time.deltaTime);
        }           
    }

    public override IState ProcessInput()
    {
        if (timer >= 4 && invokeCounter <= 2 && boss.CheckBossLife()/* && Transitions.ContainsKey("OnInvokeWaveState")*/)
        {
            timer = 0;
            start = false;
            invokeCounter++;
            OnNeedsReplan?.Invoke();
            //return Transitions["OnInvokeWaveState"];
        }
        else if (timer >= 4 && Transitions.ContainsKey("OnLaserAttackState"))
        {
            timer = 0;
            return Transitions["OnLaserAttackState"];
        }

        return this;
    }
    
    public override Dictionary<string, object> Exit(IState to)
    {
        start = false;
        timer = 0;
        return base.Exit(to);
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        boss.SetPowerUpBoss(true);
        start = true;
    }
}
