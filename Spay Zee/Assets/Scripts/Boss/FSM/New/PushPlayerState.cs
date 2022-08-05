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
        ShieldBody = Shield.GetComponent<Rigidbody>();
        ShieldAnimator = Shield.GetComponent<Animator>();
        OnNeedsReplan = onNeedsReplan;
    }

    public override void UpdateLoop()
    {
        Vector3 lookAtPos = _player.transform.position;
        lookAtPos.z = boss.transform.position.z;
        boss.transform.up = lookAtPos - boss.transform.position;

        timer += Time.deltaTime;

        boss.transform.position = Vector3.MoveTowards(boss.transform.position, _player.transform.position, Time.deltaTime);
    }

    public override IState ProcessInput()
    {
        if (timer > 4)
        {
            if (boss.life < 50)
            {
                OnNeedsReplan?.Invoke();
            }
            else if (boss.overheatingCounter < 5 && boss.Mood == BossMood.Calm && Transitions.ContainsKey("OnAttackState"))
            {
                return Transitions["OnAttackState"];
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
        timer = 0;
        invokeCounter++;
        boss.Mood = BossMood.Calm;
        boss.overheatingCounter++;
        ShieldAnimator.Play("push");
        base.Enter(from);
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        return base.Exit(to);
    }
}
