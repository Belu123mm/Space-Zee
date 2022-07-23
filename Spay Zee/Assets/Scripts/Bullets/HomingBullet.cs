using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HomingBullet : BulletBehaviour
{

    GameObject target;
    Vector2 _dirToGo;

    public override void OnAwake()
    {
        base.OnAwake();
        _owner.SetSprite(BulletFlyWeightPointer.PlayerHomingBullet.texture);
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }

    public override void ExecuteBehaviour()
    {
        base.ExecuteBehaviour();
        if (timeAlive >= BulletFlyWeightPointer.PlayerHomingBullet.lifetime)
            _owner.GoToSleep();

        FindEnemy();

        if (target)
        {
            _dirToGo = target.transform.position - _owner.transform.position;
            _owner.transform.up = Vector2.Lerp(_owner.transform.up, _dirToGo, BulletFlyWeightPointer.PlayerHomingBullet.turnSpeed * Time.deltaTime);
            _owner.transform.position += _owner.transform.up * BulletFlyWeightPointer.PlayerHomingBullet.speed *0.8f* Time.deltaTime;
        }
        else
        {
            _owner.transform.position += _owner.transform.up * BulletFlyWeightPointer.PlayerHomingBullet.speed * Time.deltaTime;
        }
    }

    void FindEnemy()
    {
        //IA2-P3

        var allTargets = CircleQuery2D.instance.Query(_owner.transform.position).Select(n => n.GetGO().GetComponent<Enemy>()).Take(1);

        foreach (var item in allTargets)
        {
            if (!target)
                target = item.gameObject;

            else if (Vector2.Distance(_owner.transform.position, item.transform.position) < Vector2.Distance(_owner.transform.position, target.transform.position))
                target = item.gameObject;
        }
    }

    #region Getters
    public override LayerMask GetTarget()
    {
        return BulletFlyWeightPointer.PlayerHomingBullet.Target;
    }

    public override int GetDamage()
    {
        return BulletFlyWeightPointer.PlayerHomingBullet.dmg;
    }

    public override LayerMask GetIgnore()
    {
        return BulletFlyWeightPointer.PlayerHomingBullet.Ignore;
    }

    public override float GetFireRate()
    {
        return BulletFlyWeightPointer.PlayerHomingBullet.fireRate;
    }
    #endregion

    public override BulletBehaviour SetOwner(Bullet B)
    {
        return base.SetOwner(B);
    }
}