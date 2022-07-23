using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyFlyweightPointer
{
    public static readonly EnemyFlyweight Chaser = new EnemyFlyweight
    {
        contactDMG = 2,
        moveSpeed = 1,
        HP = 2,
        turnSpeed = 3,
        powerUpDropChance = 5,
        texture = Resources.Load<Sprite>("Art/Enemies/Chaser")
    };
    public static readonly EnemyFlyweight Shooter = new EnemyFlyweight
    {
        contactDMG = 3,
        moveSpeed = 1,
        HP = 3,
        turnSpeed = 3,
        powerUpDropChance = 8,
        shootRange = 5,
        texture = Resources.Load<Sprite>("Art/Enemies/LinearShooter"),
        bulletBehaviour = new LinearBullet()
    };
    public static readonly EnemyFlyweight HomingShooter = new EnemyFlyweight
    {
        contactDMG = 2,
        moveSpeed = 1,
        HP = 2,
        turnSpeed = 3,
        powerUpDropChance = 20,
        shootRange = 7,
        texture = Resources.Load<Sprite>("Art/Enemies/HomingShooter"),
        bulletBehaviour = new SinBullet()
    };
    public static readonly EnemyFlyweight ChaserHeavy = new EnemyFlyweight
    {
        contactDMG = 5,
        moveSpeed = 0.5f,
        HP = 7,
        turnSpeed = 1,
        powerUpDropChance = 15,
        texture = Resources.Load<Sprite>("Art/Enemies/ChaserHeavy")
    };
}
