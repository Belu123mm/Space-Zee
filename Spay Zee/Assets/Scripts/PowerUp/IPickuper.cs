using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickuper
{
    void Heal(int current);

    void MultiShot(float time);

    void Invulnerable(float time);

    void SpeedBoost(float time, float boost);
}
