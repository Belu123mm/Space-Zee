using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    public Image lifeBar;
    public Image lifeBarUnder;
    public Text LifeText;

    public GameObject shield;
    public GameObject quitThang;

    public ParticleSystem MoveParticle;
    public ParticleSystem HitParticle;
    public ParticleSystem DieParticle;
    public ParticleSystem HealParticle;

    public AudioClip hitSound;
    public AudioClip dieSound;
    public AudioClip shootSound;

    public AudioClip invulnerableSound;
    public AudioClip multishotSound;
    public AudioClip boostSound;
    public AudioClip healSound;

    public Image[] bulletimages;

    SpriteRenderer _sr;
    Animator _anim;
    AudioSource _au;

    public float fadeInOutSpeed;

    Coroutine function;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _au = GetComponent<AudioSource>();
        LifeText.text = "12/12";
    }

    private void Start()
    {
        ChangeShotType(1);
    }

    public void TakeDMG(float current, float max)
    {
        LifeText.text = current + "/" + max;
        if (function != null)
            StopCoroutine(function);
        function = StartCoroutine(DamageLindo(current / max));
        HitParticle.Play();
        _au.PlayOneShot(hitSound);
    }

    public void DeadPlayer()
    {
        DieParticle.Play();
        _au.PlayOneShot(dieSound);
        _sr.enabled = false;
    }

    public void MovePlayer(float x, float y)
    {
        if ((x != 0 || y != 0) && !MoveParticle.isPlaying)
            MoveParticle.Play();
        else if (x == 0 && y == 0 && MoveParticle.isPlaying)
            MoveParticle.Stop();
    }

    public void Shoot()
    {
        _au.PlayOneShot(shootSound);
    }

    public void ChangeShotType(int value)
    {
        value--;
        for (int i = 0; i <= bulletimages.Length-1; i++)
        {
            if (i == value)
                bulletimages[i].gameObject.SetActive(true);
            else
                bulletimages[i].gameObject.SetActive(false);
        }
    }

    public void Heal(float current, float max)
    {
        if (function != null)
            StopCoroutine(function);
        LifeText.text = current + "/" + max;
        function = StartCoroutine(DamageLindo(current / max));
        HealParticle.Play();
        _au.PlayOneShot(healSound);
    }

    public void ShowMenu()
    {
        quitThang.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        quitThang.gameObject.SetActive(false);
    }

    public void Boost(float time)
    {
        _au.PlayOneShot(boostSound);
    }

    public void Invulnerable(float time)
    {
        shield.SetActive(true);
        _au.PlayOneShot(invulnerableSound);
        StartCoroutine(LoseInvulnerability(time));
    }

    public void MultiShot(float time)
    {
        _au.PlayOneShot(multishotSound);
    }

    IEnumerator DamageLindo(float value)
    {
        lifeBar.fillAmount = value;
        float counter = 0;
        while(counter < 1)
        {
            counter += Time.deltaTime * 0.1f;
            lifeBarUnder.fillAmount = Mathf.Lerp(lifeBarUnder.fillAmount, lifeBar.fillAmount, counter);
            yield return null;
        }
    }

    IEnumerator LoseInvulnerability(float t)
    {
        yield return new WaitForSeconds(t);
        shield.SetActive(false);
    }
}