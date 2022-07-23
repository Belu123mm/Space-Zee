using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : IBullet
{
    protected Bullet _owner;
    protected float timeAlive;
    bool _awake = false;

    public virtual void OnAwake()
    {
        timeAlive = 0;
        _awake = true;
    }

    public virtual void ExecuteBehaviour()
    {
        if (!_awake)
            return;
        timeAlive += Time.deltaTime;
    }

    public virtual void OnSleep()
    {
        _awake = false;
    }

    public virtual BulletBehaviour SetOwner(Bullet B)
    {
        _owner = B;
        return this;
    }

    public virtual LayerMask GetTarget()
    {   
        return LayerMask.NameToLayer("EnemyLayer");
    }

    public virtual LayerMask GetIgnore()
    {
        throw new System.NotImplementedException();
    }

    public virtual int GetDamage()
    {
        return 1;
    }

    public virtual float GetFireRate()
    {
        return .1f;
    }
}