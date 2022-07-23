using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletFlyWeightPointer
{
    public static readonly BulletFlyWeight PlayerLinearBullet = new BulletFlyWeight
    {
        speed = 5,
        lifetime = 5,
        dmg = 3,
        fireRate = .15f,
        texture = Resources.Load<Sprite>("Art/Bullets/LinearBulletSprite"),
        Target = LayerMask.NameToLayer("EnemyLayer"),
        Ignore = LayerMask.NameToLayer("PlayerLayer")
    };
    public static readonly BulletFlyWeight PlayerBombBullet = new BulletFlyWeight
    {
        speed = 6,
        lifetime = 7,
        dmg = 5,
        fireRate = 1.4f,
        amp = .06f,
        freq = 15f,
        texture = Resources.Load<Sprite>("Art/Bullets/bomb"),
        Target = LayerMask.NameToLayer("EnemyLayer"),
        Ignore = LayerMask.NameToLayer("PlayerLayer")
    };
    public static readonly BulletFlyWeight PlayerHomingBullet = new BulletFlyWeight
    {
        speed = 3,
        lifetime = 10,
        dmg = 10,
        fireRate = 1,
        turnSpeed = 5,
        texture = Resources.Load<Sprite>("Art/Bullets/HomingBulletSprite"),
        Target = LayerMask.NameToLayer("EnemyLayer"),
        Ignore = LayerMask.NameToLayer("PlayerLayer")
    };
    public static readonly BulletFlyWeight EnemyLinearBullet = new BulletFlyWeight
    {
        speed = 3.5f,
        lifetime = 5,
        fireRate = 1,
        dmg = 2,
        texture = Resources.Load<Sprite>("Art/Bullets/EnemyLinearBulletSprite"),
        Target = LayerMask.NameToLayer("PlayerLayer"),
        Ignore = LayerMask.NameToLayer("EnemyLayer")
    };
    public static readonly BulletFlyWeight EnemyHomingBullet = new BulletFlyWeight
    {
        speed = 2,
        lifetime = 7,
        fireRate = 2,
        dmg = 4,
        turnSpeed = 2,
        texture = Resources.Load<Sprite>("Art/Bullets/EnemyHomingBulletSprite"),
        Target = LayerMask.NameToLayer("PlayerLayer"),
        Ignore = LayerMask.NameToLayer("EnemyLayer")
    };
}
