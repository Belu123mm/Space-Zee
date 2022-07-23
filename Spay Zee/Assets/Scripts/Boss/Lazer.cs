using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D coll)
    {
        var shootable = coll.gameObject.GetComponent<Ishootable>();

        if (coll.gameObject.layer == 9 && shootable != null)
        {
            shootable.GetHit(2);
        }
    }
}





