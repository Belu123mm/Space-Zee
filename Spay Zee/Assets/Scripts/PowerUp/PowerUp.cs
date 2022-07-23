using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public ParticleSystem idleParticle;
    public ParticleSystem pickupParticle;

    SpriteRenderer _SR;
    BoxCollider2D _BC2D;

    private void Awake()
    {
        idleParticle.Play();
        _SR = GetComponent<SpriteRenderer>();
        _BC2D = GetComponent<BoxCollider2D>();
    }

    public virtual void GoToSleep()
    {
        pickupParticle.Play();
        _SR.enabled = false;
        _BC2D.enabled = false;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        while (pickupParticle.isPlaying)
            yield return null;
        Destroy(gameObject);
    }
}
