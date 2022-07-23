using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShooterEnemyBehaviour : EnemyBehaviour
{
    bool _canShoot = true;
    BulletBehaviour _myBulletBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        _owner.SetSprite(EnemyFlyweightPointer.Shooter.texture);
    }

    public override void ExecuteBehaviour()
    {
        base.ExecuteBehaviour();
        Move();
        if (CheckView())
        {
            Shoot();
        }
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }

    void Move()
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(_owner.transform.position, 1);
        var dir = new Vector3();
       //Debug.Log(obstacles.Length);


        if (obstacles.Length > 0)
        {
            Collider2D closer = null;
            //IA2_P2
            closer = obstacles.OrderBy(x => Vector3.Distance(x.transform.position,_owner.transform.position)).First();;
            Vector3 dirFromObs = (_target.transform.position - closer.transform.position).normalized * 10;
            dir += dirFromObs;
        }

        var olddir = (_target.transform.position - _owner.transform.position).normalized;
        var newDir = Vector3.Lerp(olddir, dir, 0.9f);
        _owner.transform.up = newDir;
        _owner.transform.position += newDir * EnemyFlyweightPointer.Chaser.moveSpeed /10* Time.deltaTime;
    }

    bool CheckView()
    {
        return Vector3.Distance(_target.transform.position, _owner.transform.position) < EnemyFlyweightPointer.Shooter.shootRange;
    }

    void Shoot()
    {
        if (_canShoot)
        {
            _myBulletBehaviour = new EnemyLinearBullet();
            BulletPool.instance.SpawnBullet(_owner.transform.position, _owner.transform.localEulerAngles).SetBehaviour(_myBulletBehaviour);
            _canShoot = false;
            _owner.StartCoroutine(ShootAgain(_myBulletBehaviour.GetFireRate()));
        }
    }

    IEnumerator ShootAgain(float value)
    {
        float ticks = 0;

        while (ticks < value)
        {
            ticks += Time.deltaTime;
            yield return null;
        }
        _canShoot = true;
    }

    public override int GetDmg()
    {
        return EnemyFlyweightPointer.Shooter.contactDMG;
    }

    public override int GetHP()
    {
        return EnemyFlyweightPointer.Shooter.HP;
    }

    public override void Die()
    {
        PowerUpManager.Instance.SpawnPowerUp(EnemyFlyweightPointer.Shooter.powerUpDropChance, _owner.transform.position);
        EnemyPool.instance.ReturnEnemy(_owner);
    }
}
