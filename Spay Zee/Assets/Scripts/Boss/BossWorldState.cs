using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWorldState : MonoBehaviour
{
    public static BossWorldState instance;
    public Boss boss;

    public Transform playerPosition;
    public Transform bossPosition;

    public int invokeStateStarter;
    public float closeDistance {get; private set;}

    public bool IsBossPowerUp { get; private set;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        closeDistance = 0.60f;

        invokeStateStarter = 50;
    }

    public GOAPState GetWorldState()
    {
        var from = new GOAPState();
        from.values["isPlayerClose"] = IsPlayerClose();
        from.values["BossPoweredUp"] = IsBossPowerUp;
        from.values["isPlayerAlive"] = true;
        from.values["LowHPBoss"] = CheckBossLife();
        from.values["LowEnergyBoss"] = CheckBossEnergy();
        from.values["isBossAngry"] = IsTheBossMad();

        return from;
    }

    public GOAPState GetObjectiveState()
    {
        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;
        to.values["LowHPBoss"] = false;
        to.values["LowEnergyBoss"] = false;
        to.values["isBossAngry"] = false;


        return to;
    }

    public void SetPowerUpBoss(bool value)
    {
        IsBossPowerUp = value;
    }

    public bool IsTheBossMad() => boss.damageTaken >= 25;

    public bool IsPlayerClose() => Vector2.Distance(playerPosition.position, bossPosition.position) < closeDistance;

    public bool CheckBossLife() => boss.life <= invokeStateStarter;

    public bool CheckBossEnergy() => boss.powerCounter >= 3;
}
