using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ChaserEnemyHeavyBehaviour : EnemyBehaviour
{ //IA2-P1
    public override void OnAwake()
    {
        base.OnAwake();
        _owner.SetSprite(EnemyFlyweightPointer.ChaserHeavy.texture);
    }

    public override void ExecuteBehaviour()
    {
        base.ExecuteBehaviour();
        Move();
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }

    void Move()
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(_owner.transform.position, 1);
        var dir = new Vector3();

        if (obstacles.Length > 0)
        {
            Collider2D closer = null;
            //IA2_P2
            closer = obstacles.OrderBy(x => Vector3.Distance(x.transform.position,_owner.transform.position)).First();;
            Vector3 dirFromObs = (_target.transform.position - closer.transform.position).normalized * 2;
            dir += dirFromObs;
        }

        var olddir = (_target.transform.position - _owner.transform.position).normalized;
        var newDir = Vector3.Lerp(olddir, dir, 0.9f);
        _owner.transform.up = newDir;
        _owner.transform.position += newDir * EnemyFlyweightPointer.Chaser.moveSpeed * Time.deltaTime;
    }

    public override int GetDmg()
    {
        return EnemyFlyweightPointer.ChaserHeavy.contactDMG;
    }

    public override int GetHP()
    {
        return EnemyFlyweightPointer.ChaserHeavy.HP;
    }

    public override void Die()
    {
        PowerUpManager.Instance.SpawnPowerUp(EnemyFlyweightPointer.ChaserHeavy.powerUpDropChance, _owner.transform.position);
        EnemyPool.instance.ReturnEnemy(_owner);
    }
    
}
