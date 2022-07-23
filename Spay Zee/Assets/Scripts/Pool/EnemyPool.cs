using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    public SpatialGrid gridParent;






    public static EnemyPool instance
    {
        get
        {
            return _instance;
        }
    }
    static EnemyPool _instance;

    ObjectPool<Enemy> _enemyPool;

    public Enemy enemyPF;


    private void Awake()
    {
        _instance = this;
        _enemyPool = new ObjectPool<Enemy>(EnemyFactory, Enemy.TurnOn, Enemy.TurnOff, 4);
    }

    public Enemy EnemyFactory()
    {
        return Instantiate(enemyPF);
    }

    public void ReturnEnemy(Enemy E)
    {
        //E.transform.position = new Vector2(100, 100);

        gridParent.RemoveGridEntity(E);
        E.transform.parent = null;
        _enemyPool.ReturnObject(E);
    }

    public Enemy SpawnEnemy(Vector3 position, Vector3 rotation)
    {
        var e = _enemyPool.GetObject();
        gridParent.AddNewGridEntity(e);
        e.transform.SetParent(gridParent.transform);
        e.transform.position = position;
        e.transform.localEulerAngles = rotation;
        return e;
    }
}
