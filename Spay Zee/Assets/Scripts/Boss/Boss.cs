﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public float life;
    public float damageTaken;
    public int overheatingCounter;

    public BossGoap goap;
    public GameObject explosion;
    bool isDead;
    float feedbackTimer;

    public Transform playerPosition;
    public float closeDistance { get; private set; }

    public int invokeStateStarter;

    public BossMood Mood;
    


    private void Awake()
    {
        closeDistance = 0.60f;

        invokeStateStarter = 50;
        Mood = BossMood.Calm;
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

    public bool IsPlayerClose() => Vector2.Distance(playerPosition.position, transform.position) < closeDistance;

    public float CheckBossLife() => life;

    public int CheckOverheating() => overheatingCounter;
}

