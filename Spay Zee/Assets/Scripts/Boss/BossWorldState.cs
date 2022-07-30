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
        from.values[BossState.PlayerClose] = IsPlayerClose();
        from.values[BossState.PoweredUp] = IsBossPowerUp;
        from.values[BossState.PlayerAlive] = true;
        from.values[BossState.LowHP] = CheckBossLife();
        from.values[BossState.EnergyDown] = CheckBossEnergy();
        from.values[BossState.Angry] = IsTheBossMad();

        return from;
    }

    public GOAPState GetObjectiveState()
    {
        var to = new GOAPState();
        to.values[BossState.PlayerAlive] = false;
        to.values[BossState.LowHP] = false;
        to.values[BossState.EnergyDown] = false;
        to.values[BossState.Angry] = false;


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
