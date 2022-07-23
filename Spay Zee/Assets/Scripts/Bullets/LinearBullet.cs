using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : BulletBehaviour
{
    public override void OnAwake()
    {
        base.OnAwake();
        _owner.SetSprite(BulletFlyWeightPointer.PlayerLinearBullet.texture);
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }

    public override void ExecuteBehaviour()
    {
        if (timeAlive <= BulletFlyWeightPointer.PlayerLinearBullet.lifetime)
            timeAlive += Time.deltaTime;
        else
            _owner.GoToSleep();
        _owner.transform.position += _owner.transform.up * BulletFlyWeightPointer.PlayerLinearBullet.speed * Time.deltaTime;
    }

    #region Getters
    public override LayerMask GetTarget()
    {
        return BulletFlyWeightPointer.PlayerLinearBullet.Target;
    }

    public override int GetDamage()
    {
        return BulletFlyWeightPointer.PlayerLinearBullet.dmg;
    }

    public override LayerMask GetIgnore()
    {
        return BulletFlyWeightPointer.PlayerLinearBullet.Ignore;
    }

    public override float GetFireRate()
    {
        return BulletFlyWeightPointer.PlayerLinearBullet.fireRate;
    }
    #endregion

    public override BulletBehaviour SetOwner(Bullet B)
    {
        return base.SetOwner(B);
    }
}