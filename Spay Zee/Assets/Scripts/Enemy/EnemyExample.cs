using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExample : MonoBehaviour, Ishootable
{
    float HP = 5;

    public void GetHit(int value)
    {
        HP -= value;
    }

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }
}
