using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Model : MonoBehaviour, Ishootable, IPickuper, IObservable
{
    Icontroller _myController;

    public int maxHP;
    int _currentHP;

    int _indexBehaviour = 1;
    Dictionary<int, Func<BulletBehaviour>> AllBehaviours = new Dictionary<int, Func<BulletBehaviour>>();

    public Transform[] ShootPositions;
    public Transform[] MultiShotPositions;
    int _shootIndex = 0;
    bool canShoot = true;

    public float normalSpeed;
    float _currentSpeed;

    bool _invulnerable = false;

    bool _multiShot = false;

    Vector2 _moveDirection;

    Rigidbody2D RB2D;

    List<IObserver> _allObs = new List<IObserver>();

    public event Action<float, float> ActionDMG = delegate { };
    public event Action<float, float> ActionMove = delegate { };
    public event Action ActionFire = delegate { };
    public event Action ActionDIE = delegate { };
    public event Action<float, float> ActionHeal = delegate { };
    public event Action<float> ActionInvulnerable = delegate { };
    public event Action<float> ActionMultiShot = delegate { };
    public event Action<float> ActionSpeedBoost = delegate { };
    public event Action<int> ActionSwitchShotType = delegate {};

    public event Action ActionStop = delegate {};
    public event Action ActionContinue = delegate {};

    private void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
        _currentHP = maxHP;
        _currentSpeed = normalSpeed;
        _myController = new Controller(GetComponentInChildren<View>(), this);
        SubscribeTo(SceneHandler.Instance);
        AllBehaviours.Add(1, () => { return new LinearBullet(); });
        AllBehaviours.Add(2, () => { return new SinBullet(); });
        AllBehaviours.Add(3, () => { return new HomingBullet(); });
    }

    private void Update()
    {
        if (_myController != null)
            _myController.OnUpdate();
    }

    public void Takedmg(int DMG)
    {
        if (_invulnerable)
            return;
        _currentHP -= DMG;
        ActionDMG(_currentHP, maxHP);
        if (_currentHP <= 0)
            Die();
    }

    public void MovePlayer(float x, float y)
    {
        ActionMove(x,y);
        _moveDirection.x = x;
        _moveDirection.y = y;
        RB2D.velocity = _moveDirection * _currentSpeed;
    }

    public void LookPlayer()
    {
        Vector3 lookAtPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookAtPos.z = transform.position.z;
        transform.up = lookAtPos - transform.position;
    }

    public void NextBehaviour()
    {
        if (_indexBehaviour >= AllBehaviours.Count)
            _indexBehaviour = 1;
        else
            _indexBehaviour++;

        ActionSwitchShotType(_indexBehaviour);
    }

    public void Previousbehaviour()
    {
        if (_indexBehaviour <= 1)
            _indexBehaviour = AllBehaviours.Count;
        else
            _indexBehaviour--;

        ActionSwitchShotType(_indexBehaviour);
    }

    public void Fire()
    {
        if(canShoot)
        {
            ActionFire();
            if (_shootIndex >= ShootPositions.Length - 1)
                _shootIndex = 0;
            else
                _shootIndex++;

            BulletPool.instance.SpawnBullet(ShootPositions[_shootIndex].position, transform.localEulerAngles).SetBehaviour(AllBehaviours[_indexBehaviour]());
            if (_multiShot)
            {
                foreach (var item in MultiShotPositions)
                {
                    BulletPool.instance.SpawnBullet(item.position, item.eulerAngles).SetBehaviour(AllBehaviours[_indexBehaviour]());
                }
            }
            canShoot = false;
            StartCoroutine(ShootAgain(AllBehaviours[_indexBehaviour]().GetFireRate()));
        }
    }

    public void PauseMenu()
    {
        Time.timeScale = 0;

        ActionStop();
    }

    public void UnpauseMenu()
    {
        Time.timeScale = 1;

        ActionContinue();
    }

    IEnumerator ShootAgain(float value)
    {
        float ticks = 0;

        while (ticks < value)
        {
            ticks += Time.deltaTime;
            yield return null;
        }
        canShoot = true;
    }

    void Die()
    {
        ActionDIE();
        _myController = null;
        _invulnerable = true;
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(3);
        NotifyObservers("Lose");
    }

    public void GetHit(int value)
    {
        Takedmg(value);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if( collision.gameObject.layer == 11)
       {
            GetHit(1);
       }
    }


    #region Power Ups
    public void Heal(int value)
    {
        _currentHP += value;
        if (_currentHP > maxHP)
            _currentHP = maxHP;
        ActionHeal(_currentHP, maxHP);
    }

    public void MultiShot(float time)
    {
        if(!_multiShot)
        {
            ActionMultiShot(time);
            _multiShot = true;
            StartCoroutine(LoseMultiShot(time));
        }
    }

    public void Invulnerable(float time)
    {
        if(!_invulnerable)
        {
            ActionInvulnerable(time);
            _invulnerable = true;
            StartCoroutine(LoseInvulnerability(time));
        }
    }

    public void SpeedBoost(float time, float boost)
    {
        ActionSpeedBoost(time);
        _currentSpeed += normalSpeed * boost;
        StartCoroutine(LoseSpeedBoost(time, boost));
    }

    IEnumerator LoseSpeedBoost(float time, float boost)
    {
        yield return new WaitForSeconds(time);
        _currentSpeed -= normalSpeed * boost;
    }

    IEnumerator LoseInvulnerability(float t)
    {
        yield return new WaitForSeconds(t);
        _invulnerable = false;
    }

    IEnumerator LoseMultiShot(float t)
    {
        yield return new WaitForSeconds(t);
        _multiShot = false;
    }

    public void SubscribeTo(IObserver observer)
    {
        if (!_allObs.Contains(observer))
            _allObs.Add(observer);
    }

    public void UnSubscribeFrom(IObserver observer)
    {
        if (_allObs.Contains(observer))
            _allObs.Remove(observer);
    }

    public void NotifyObservers(string action)
    {
        foreach (var item in _allObs)
        {
            item.Notify(action);
        }
    }
    #endregion
}