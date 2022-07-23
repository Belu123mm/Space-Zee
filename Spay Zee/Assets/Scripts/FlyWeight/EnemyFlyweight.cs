using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyweight
{
    public int contactDMG = 1;
    public int HP;
    public int powerUpDropChance;
    public float moveSpeed = 2;
    public float turnSpeed = 2;
    public float shootRange = 5;
    public Sprite texture;
    public BulletBehaviour bulletBehaviour;
    public int targetLayer = LayerMask.NameToLayer("PlayerLayer");
    public LayerMask obstacleLayer = LayerMask.NameToLayer("Obstacle");
}
