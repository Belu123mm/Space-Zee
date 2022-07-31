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

    public AttackState(Boss _boss, GameObject _position, List<Transform> _canons)
    {
        boss = _boss;
        position = _position;
        canons = _canons;    
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
    }

    public void Shoot(Transform t)
    {
        BulletPool.instance.SpawnBullet(t.position,t.parent.parent.localEulerAngles, 12).SetBehaviour(new EnemyLinearBullet());
    }

    public override IState ProcessInput() 
    {
        if (stateTimer >= 6)
        {
            OnNeedsReplan?.Invoke();
        }
            return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        stateTimer = 0;
        bulletsTimer = 0;
        Debug.Log("entro a attack");
        base.Enter(from);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        stateTimer = 0;
        bulletsTimer = 0;
        return base.Exit(to);
    }
}