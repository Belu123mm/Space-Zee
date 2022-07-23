using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SinBullet : BulletBehaviour
{
    float count;
    float timeToExplode = 1;
    GameObject bombEffect;

    public override void OnAwake()
    {
        base.OnAwake();
        _owner.SetSprite(BulletFlyWeightPointer.PlayerBombBullet.texture);
        bombEffect = Resources.Load<GameObject>("Art/MISC/bombEffect/bombBoomHolder");
        _owner.imABomb = true;
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }

    public override void ExecuteBehaviour()
    {
        count += Time.deltaTime;

        if(count >= timeToExplode)
        {
            Object.Instantiate(bombEffect, _owner.transform.position, Quaternion.identity);

            GoBoom();
        }
        _owner.transform.position += _owner.transform.up * BulletFlyWeightPointer.PlayerLinearBullet.speed * Time.deltaTime;
    }

    public void GoBoom()
    {
        //IA2-P3

        var elements = CircleQuery2D.instance.Query(_owner.transform.position).Select(n => n.GetGO().GetComponent<Enemy>()).Where(n => n.GetGO().activeInHierarchy);

        foreach (var item in elements)
        {
            item.GetHit(10000);
        }

        Object.Instantiate(bombEffect, _owner.transform.position, Quaternion.identity);

        _owner.callback.Invoke(_owner);

        _owner.GoToSleep();
    }

    #region Getters
    public override LayerMask GetTarget()
    {
        return BulletFlyWeightPointer.PlayerBombBullet.Target;
    }

    public override int GetDamage()
    {
        return BulletFlyWeightPointer.PlayerBombBullet.dmg;
    }

    public override LayerMask GetIgnore()
    {
        return BulletFlyWeightPointer.PlayerBombBullet.Ignore;
    }

    public override float GetFireRate()
    {
        return BulletFlyWeightPointer.PlayerBombBullet.fireRate;
    }
    #endregion

    public override BulletBehaviour SetOwner(Bullet B)
    {
        return base.SetOwner(B);
    }
}