using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PowerUp[] AllPowerUps;

    public static PowerUpManager Instance;

    private void Start()
    {
        Instance = this;
    }

    public PowerUp SpawnPowerUp(int chance, Vector3 pos)
    {
        if (Random.Range(0, 100) <= chance)
            return Instantiate(AllPowerUps[Random.Range(0, AllPowerUps.Length)], pos, Quaternion.identity);
        else
            return null;
    }
}
