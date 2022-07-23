using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Bullet : MonoBehaviour
{
    BulletBehaviour _behaviour;
    SpriteRenderer _SR;

    public Action<Bullet> callback = delegate { };

    public bool imABomb; 

    private void Awake()
    {
        gameObject.AddComponent<SpriteRenderer>();
        _SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _behaviour.ExecuteBehaviour();
    }

    public void GoToSleep()
    {
        BulletPool.instance.ReturnBullet(this);
    }

    public void AddCallback(Action<Bullet> callback)
    {
        this.callback = callback;
    }

    public static void TurnOn(Bullet B)
    {
        B.gameObject.SetActive(true);
    }
    public static void TurnOff(Bullet B)
    {
        if(B._behaviour != null)
            B._behaviour.OnSleep();
        B.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        var shootable = coll.gameObject.GetComponent<Ishootable>();
        if (coll.gameObject.layer == _behaviour.GetTarget() && shootable != null)
        {
            if (!imABomb)
            {
                shootable.GetHit(_behaviour.GetDamage());
                GoToSleep();
            }
            else
            {
                try { ((SinBullet)_behaviour).GoBoom(); }

                catch (System.InvalidCastException) { }
            }

        }
        else if (coll.gameObject.layer != gameObject.layer && coll.gameObject.layer != _behaviour.GetIgnore())
        {
            if (!imABomb)
                GoToSleep();
            else 
            {
                try { ((SinBullet)_behaviour).GoBoom();} 
                
                catch (System.InvalidCastException) { }     
            }           
        }     
    }

    #region Builders
    public Bullet SetBehaviour(BulletBehaviour B)
    {
        _behaviour = B;
        _behaviour.SetOwner(this);
        _behaviour.OnAwake();
        return this;
    }

    public Bullet SetSprite(Sprite SP)
    {
        _SR.sprite = SP;
        return this;
    }
    #endregion
}