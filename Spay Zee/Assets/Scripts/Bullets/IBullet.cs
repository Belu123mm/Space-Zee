using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet 
{
    BulletBehaviour SetOwner(Bullet B);
    void ExecuteBehaviour();
    void OnAwake();
    void OnSleep();
    LayerMask GetTarget();
    LayerMask GetIgnore();
    int GetDamage();
    float GetFireRate();
}
