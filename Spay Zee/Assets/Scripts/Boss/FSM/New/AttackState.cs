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

    bool start;

    public override event Action OnNeedsReplan;

    public AttackState(Boss _boss, GameObject _position, List<Transform> _canons)
    {
        boss = _boss;
        position = _position;
        canons = _canons;    
    }
    public override void UpdateLoop() 
    {
        if (start)
        {
            if (bulletsTimer > timeToShoot)
            {
                foreach (var t in canons)
                {
                    Shoot(t);
                }
                bulletsTimer = 0;
            }
        }


        if (start)
        {
            bulletsTimer += Time.deltaTime;
            stateTimer += Time.deltaTime;
            boss.transform.Rotate(0, 0, 10 * Time.deltaTime * 20);
        }
    }

    public void Shoot(Transform t)
    {
        BulletPool.instance.SpawnBullet(t.position,t.parent.parent.localEulerAngles).SetBehaviour(new EnemyLinearBullet());
    }

    public override IState ProcessInput() 
    {
        if (stateTimer >= 5 && Transitions.ContainsKey("OnInvokeWaveState")){
            return Transitions["OnInvokeWaveState"];
        }
        else if (stateTimer >= 6)
        {
            boss.damageTaken = 0;
            start = false;
            stateTimer = 0;
            bulletsTimer = 0;
            OnNeedsReplan?.Invoke();
        }
            return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        start = true;
        stateTimer = 0;
        bulletsTimer = 0;
        Debug.Log("entro a attack");
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        boss.damageTaken = 0;
        start = false;
        stateTimer = 0;
        bulletsTimer = 0;
        return base.Exit(to);
    }
}