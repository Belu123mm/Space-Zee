using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHeal : PowerUp
{
    public int heal;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var temp = coll.GetComponent<IPickuper>();
        if (temp != null)
        {
            temp.Heal(heal);
            GoToSleep();
        }
    }
}
