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

    public Model _player;
    
    float timer = 0;
    int invokeCounter;
    bool start;
    public override event Action OnNeedsReplan;


    private void Awake()
    {
        Shield = (GameObject)Instantiate(Resources.Load("Art/MISC/shieldEffect/BossShield", typeof(Object)));
        Shield.transform.SetParent(this.transform);
        Shield.transform.position = this.transform.position;
        Shield.transform.rotation = this.transform.rotation;
        ShieldBody= Shield.GetComponent<Rigidbody>();
        ShieldAnimator = Shield.GetComponent<Animator>();
    }

    public override void UpdateLoop()
    {
        if (start)
        {
            ShieldAnimator.Play("push");

            Vector3 lookAtPos = _player.transform.position;
            lookAtPos.z = transform.position.z;
            transform.up = lookAtPos - transform.position;

            if (!BossWorldState.instance.CheckBossLife() && BossWorldState.instance.CheckBossEnergy())
            {
                start = false;
                timer = 0;
                OnNeedsReplan?.Invoke();
            }
        }
    }

    private void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, Time.deltaTime);
        }           
    }

    public override IState ProcessInput()
    {
        if (timer >= 4 && invokeCounter <= 2 && BossWorldState.instance.CheckBossLife()/* && Transitions.ContainsKey("OnInvokeWaveState")*/)
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
        BossWorldState.instance.SetPowerUpBoss(true);
        start = true;
    }
}
