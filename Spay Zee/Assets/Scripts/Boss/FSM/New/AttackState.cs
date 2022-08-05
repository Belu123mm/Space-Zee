using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class AttackState : MonoBaseState
{
    Boss boss;
    GameObject position;
    List<Transform> canons;
    float timeToShoot = 0.1f;
    float bulletsTimer;
    float stateTimer;


    public override event Action OnNeedsReplan;

    public AttackState(Boss _boss, GameObject _position, List<Transform> _canons, Action onNeedsReplan)
    {
        boss = _boss;
        position = _position;
        canons = _canons;
        OnNeedsReplan = onNeedsReplan;
    }
    public override void UpdateLoop()
    {
        if (bulletsTimer > timeToShoot)
        {
            foreach (var t in canons)
            {
                Shoot(t);
            }
            bulletsTimer = 0;
        }

        bulletsTimer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        boss.transform.Rotate(0, 0, 10 * Time.deltaTime * 20);
        overheatTimer += Time.deltaTime;        
    }

    float overheatTimer = 0;

    public void Shoot(Transform t)
    {
        BulletPool.instance.SpawnBullet(t.position, t.parent.parent.localEulerAngles, 12).SetBehaviour(new EnemyLinearBullet());
    }

    public override IState ProcessInput()
    {
        if (stateTimer >= 4)
        {
            if (boss.life >= 50 )
            {
                if (boss.overheatingCounter <= 3 && !boss.IsPlayerClose() && Transitions.ContainsKey("OnLaserAttackState"))
                {
                    return Transitions["OnLaserAttackState"];
                }
                else if (!boss.IsPlayerClose() && Transitions.ContainsKey("OnChargeState"))
                {
                    return Transitions["OnChargeState"];
                }
            }
            if (Transitions.ContainsKey("OnInvokeWaveState"))
            {
                return Transitions["OnInvokeWaveState"];
            }
            else
            {
                OnNeedsReplan?.Invoke();
            }
        }
        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        overheatTimer = 0;
        stateTimer = 0;
        bulletsTimer = 0;
        boss.Mood = BossMood.Angry;
            boss.overheatingCounter++;
        //Debug.Log("entro a attack");
        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        stateTimer = 0;
        bulletsTimer = 0;

        return base.Exit(to);
    }
}