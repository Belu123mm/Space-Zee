using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : PowerUp
{
    public float time;
    public float boost;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var temp = coll.GetComponent<IPickuper>();
        if (temp != null)
        {
            temp.SpeedBoost(time,boost);
            GoToSleep();
        }
    }
}
