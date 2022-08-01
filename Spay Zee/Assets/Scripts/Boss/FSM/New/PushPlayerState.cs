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
    public override event Action OnNeedsReplan;

    Boss boss;
    public PushPlayerState(Boss _boss, Model player, Action onNeedsReplan)
    {
        
        boss = _boss;
        _player = player;

        Shield = (GameObject)GameObject.Instantiate(Resources.Load("Art/MISC/shieldEffect/BossShield", typeof(Object)));
        Shield.transform.SetParent(boss.transform);
        Shield.transform.position = boss.transform.position;
        Shield.transform.rotation = boss.transform.rotation;
        ShieldBody= Shield.GetComponent<Rigidbody>();
        ShieldAnimator = Shield.GetComponent<Animator>();
        OnNeedsReplan = onNeedsReplan;
    }

    public override void UpdateLoop()
    {
        
            ShieldAnimator.Play("push");

            Vector3 lookAtPos = _player.transform.position;
            lookAtPos.z = boss.transform.position.z;
            boss.transform.up = lookAtPos - boss.transform.position;

            if (boss.CheckBossLife() >= 50 && boss.CheckOverheating()>= 3)
            {
                timer = 0;
                OnNeedsReplan?.Invoke();
            }
        
  
            timer += Time.deltaTime;

            boss.transform.position = Vector3.MoveTowards(boss.transform.position, _player.transform.position, Time.deltaTime);
                 
    }

    public override IState ProcessInput()
    {
        if (timer >= 4 && invokeCounter <= 2 && boss.CheckBossLife() <= 50/* && Transitions.ContainsKey("OnInvokeWaveState")*/)
        {
            timer = 0;
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
        timer = 0;
        return base.Exit(to);
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {

    }
}
