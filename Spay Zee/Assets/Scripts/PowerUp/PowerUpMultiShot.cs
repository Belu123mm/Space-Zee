using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMultiShot : PowerUp
{
    public float time;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var temp = coll.GetComponent<IPickuper>();
        if (temp != null)
        {
            temp.MultiShot(time);
            GoToSleep();
        }
    }
}