using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHomingBullet : BulletBehaviour
{
    GameObject target;
    Vector2 _dirToGo;
    public override void OnAwake()
    {
        base.OnAwake();
        _owner.SetSprite(BulletFlyWeightPointer.EnemyHomingBullet.texture);
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }

    public override void ExecuteBehaviour()
    {
        base.ExecuteBehaviour();
        if (timeAlive >= BulletFlyWeightPointer.EnemyHomingBullet.lifetime)
            _owner.GoToSleep();

        FindEnemy();

        if (target)
        {
            _dirToGo = target.transform.position - _owner.transform.position;
            _owner.transform.up = Vector2.Lerp(_owner.transform.up, _dirToGo, BulletFlyWeightPointer.EnemyHomingBullet.turnSpeed * Time.deltaTime);
            _owner.transform.position += _owner.transform.up * BulletFlyWeightPointer.EnemyHomingBullet.speed * Time.deltaTime;
        }
        else
        {
            _owner.transform.position += _owner.transform.up * BulletFlyWeightPointer.EnemyHomingBullet.speed * Time.deltaTime;
        }
    }
    void FindEnemy()
    {
        var allTargets = Physics2D.OverlapCircleAll(_owner.transform.position, 2, 1 << BulletFlyWeightPointer.EnemyHomingBullet.Target);

        if (allTargets.Length > 0)
        {
            foreach (var item in allTargets)
            {

                if (!target)
                    target = item.gameObject;

                else if (Vector2.Distance(_owner.transform.position, item.transform.position) < Vector2.Distance(_owner.transform.position, target.transform.position))
                    target = item.gameObject;
            }
        }
    }

    #region Getters
    public override LayerMask GetTarget()
    {
        return BulletFlyWeightPointer.EnemyHomingBullet.Target;
    }

    public override int GetDamage()
    {
        return BulletFlyWeightPointer.EnemyHomingBullet.dmg;
    }

    public override LayerMask GetIgnore()
    {
        return BulletFlyWeightPointer.EnemyHomingBullet.Ignore;
    }

    public override float GetFireRate()
    {
        return BulletFlyWeightPointer.EnemyHomingBullet.fireRate;
    }
    #endregion

    public override BulletBehaviour SetOwner(Bullet B)
    {
        return base.SetOwner(B);
    }

}
