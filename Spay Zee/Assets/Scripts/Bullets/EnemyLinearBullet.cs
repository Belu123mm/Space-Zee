using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLinearBullet : BulletBehaviour
{
    public override void OnAwake()
    {
        base.OnAwake();
        _owner.SetSprite(BulletFlyWeightPointer.EnemyLinearBullet.texture);
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }

    public override void ExecuteBehaviour()
    {
        if (timeAlive <= BulletFlyWeightPointer.EnemyLinearBullet.lifetime)
            timeAlive += Time.deltaTime;
        else
            _owner.GoToSleep();
        _owner.transform.position += _owner.transform.up * BulletFlyWeightPointer.EnemyLinearBullet.speed * Time.deltaTime;
    }

    #region Getters
    public override LayerMask GetTarget()
    {
        return BulletFlyWeightPointer.EnemyLinearBullet.Target;
    }

    public override int GetDamage()
    {
        return BulletFlyWeightPointer.EnemyLinearBullet.dmg;
    }

    public override LayerMask GetIgnore()
    {
        return BulletFlyWeightPointer.EnemyLinearBullet.Ignore;
    }

    public override float GetFireRate()
    {
        return BulletFlyWeightPointer.EnemyLinearBullet.fireRate;
    }
#endregion

    public override BulletBehaviour SetOwner(Bullet B)
    {
        return base.SetOwner(B);
    }
}
