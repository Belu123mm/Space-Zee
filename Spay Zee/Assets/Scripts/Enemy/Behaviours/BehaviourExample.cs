using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourExample : EnemyBehaviour
{
    public override void OnAwake()
    {
        base.OnAwake();
        _owner.SetSprite(EnemyFlyweightPointer.Chaser.texture);
    }

    public override void ExecuteBehaviour()
    {
        base.ExecuteBehaviour();
        _owner.transform.Rotate(new Vector3(0,0,20));
    }

    public override void OnSleep()
    {
        base.OnSleep();
    }

    public override int GetHP()
    {
        return EnemyFlyweightPointer.Chaser.HP;
    }

    public override void Die()
    {
        PowerUpManager.Instance.SpawnPowerUp(EnemyFlyweightPointer.Chaser.powerUpDropChance,_owner.transform.position);
        EnemyPool.instance.ReturnEnemy(_owner);
    }
}