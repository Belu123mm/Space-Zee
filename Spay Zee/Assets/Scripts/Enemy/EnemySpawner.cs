using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour, IObservable
{
    public List<Transform> enemySpawnPoints = new List<Transform>();
    List<IObserver> _allObs = new List<IObserver>();
    public float maxSpawnTime = 10;
    public int maxWaves;
    int _currentWave = 0;


    public SpatialGrid grid;

    EnemyBehaviour _currentBehaviour;
    Dictionary<int, Func<EnemyBehaviour>> AllBehaviours = new Dictionary<int, Func<EnemyBehaviour>>();

    Lookuptable<int, float> _timeToSpawn;
    
    private void Awake()
    {
        SubscribeTo(SceneHandler.Instance);

        _timeToSpawn = new Lookuptable<int, float>(MyFactory);

        for (int i = 1; i < 5; i++)
        {
            _timeToSpawn.ReturnValue(i);
        }

        AllBehaviours.Add(0, () => { return new ChaserEnemyBehaviour(); });
        AllBehaviours.Add(1, () => { return new ShooterEnemyBehaviour(); });

        AllBehaviours.Add(2, () => { return new HomingShooterEnemyBehaviour(); });
        AllBehaviours.Add(3, () => { return new ChaserEnemyHeavyBehaviour(); });
    }

    float TimeBetweenRounds(int w)
    {
        return _timeToSpawn.ReturnValue(w);
    }

    float MyFactory(int v)
    {
        var temp = maxSpawnTime - (v/3);
        if (temp < 2f)
            temp = 2f;
        return temp;
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        _currentWave++;

        //COMENTAR ESTO LO HACE ENDLESS

        //if (_currentWave > maxWaves)
         //   NotifyObservers("NextLevel");

        //if (_currentWave <= maxWaves)
        //{
            foreach (var pos in enemySpawnPoints)
            {
                var random = UnityEngine.Random.Range(0, AllBehaviours.Count);
                _currentBehaviour = AllBehaviours[random]();
                EnemyPool.instance.SpawnEnemy(pos.transform.position, pos.transform.localEulerAngles).SetBehaviour(_currentBehaviour);              
            }

        //grid.Refesh();
        //}

        yield return new WaitForSeconds(TimeBetweenRounds(_currentWave));
        StartCoroutine(SpawnEnemies());
    }

    public void NotifyObservers(string action)
    {
        for (int i = _allObs.Count - 1; i >= 0; i--)
            _allObs[i].Notify(action);
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
}