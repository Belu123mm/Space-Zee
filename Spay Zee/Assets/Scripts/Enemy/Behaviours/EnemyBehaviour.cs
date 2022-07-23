using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : IEnemy
{
    protected Enemy _owner;
    protected Model _target;

    public EnemyBehaviour SetOwner(Enemy E)
    {
        _owner = E;
        return this;
    }

    public EnemyBehaviour SetTarget(Model M)
    {
        _target = M;
        return this;
    }

    public virtual void ExecuteBehaviour()
    {
        
    }

    public virtual void OnAwake()
    {
        
    }

    public virtual void OnSleep()
    {
        
    }

    public virtual int GetDmg()
    {
        return 1;
    }

    public virtual int GetTarget()
    {
        return EnemyFlyweightPointer.Chaser.targetLayer;
    }

    public virtual int GetHP()
    {
        return 1;
    }

    public virtual void Die()
    {

    }
}