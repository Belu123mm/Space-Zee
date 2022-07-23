using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Ishootable , IGridEntity
{
    //IA2-P2
    public event Action<IGridEntity> OnMove;
    public Vector3 Position {
        get => transform.position;
        set => transform.position = value;
    }
    //IA2-P2
    
    int _currentHP;
    EnemyBehaviour _MyBehaviour;
    SpriteRenderer  _SR;
    Model _target;


    private void Awake()
    {
        _SR = GetComponent<SpriteRenderer>();
        _target = FindObjectOfType<Model>();
    }
    void Update()
    {
        if (_MyBehaviour != null)
            //IA2-P2
            OnMove(this);
            _MyBehaviour.ExecuteBehaviour();
    }

    public static void TurnOn(Enemy E)
    {
        E.gameObject.SetActive(true);
        if (E._MyBehaviour != null)
            E._MyBehaviour.OnAwake();
    }

    public static void TurnOff(Enemy E)
    {
        if (E._MyBehaviour != null)
            E._MyBehaviour.OnSleep();
        E.gameObject.SetActive(false);
    }

    public int GetCurrentHP => _currentHP;
    
    public Enemy SetBehaviour(EnemyBehaviour EB)
    {
        _MyBehaviour = EB;
        _MyBehaviour.SetOwner(this).SetTarget(_target);
        _MyBehaviour.OnAwake();
        _currentHP = _MyBehaviour.GetHP();
        return this;
    }

    public Enemy SetSprite(Sprite SP)
    {
        _SR.sprite = SP;
        return this;
    }

    public void GetHit(int value)
    {
        _currentHP -= value;
        if (_currentHP <= 0)
            _MyBehaviour.Die();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        var temp = coll.gameObject.GetComponent<Ishootable>();
        if (temp != null && coll.gameObject.layer == _MyBehaviour.GetTarget())
            temp.GetHit(_MyBehaviour.GetDmg());
    }

    public GameObject GetGO()
    {
        return gameObject;
    }
}