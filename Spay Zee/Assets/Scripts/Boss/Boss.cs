using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public float life;
    public float damageTaken;
    public int powerCounter;

    public BossGoap goap;
    public GameObject explosion;
    bool isDead;
    float feedbackTimer;

    public Transform playerPosition;
    public float closeDistance { get; private set; }
    public bool IsBossPowerUp { get; private set; }

    public int invokeStateStarter;

    


    private void Awake()
    {
        closeDistance = 0.60f;

        invokeStateStarter = 50;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (10))
        {
            life--;
            damageTaken++;
        }
    }

    private void Update()
    {
        if(life <= 0)
        {
            isDead = true;
            Destroy(goap);
            Debug.Log("me morí");
        }

        if (isDead)
        {
            feedbackTimer += Time.deltaTime;
            explosion.SetActive(true);
        }

        if(feedbackTimer >= 1)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void SetPowerUpBoss(bool value)
    {
        IsBossPowerUp = value;
    }

    public bool IsTheBossMad() => damageTaken >= 25;

    public bool IsPlayerClose() => Vector2.Distance(playerPosition.position, transform.position) < closeDistance;

    public bool CheckBossLife() => life <= invokeStateStarter;

    public bool CheckBossEnergy() => powerCounter >= 3;
}

