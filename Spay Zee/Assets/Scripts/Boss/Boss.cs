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
}
