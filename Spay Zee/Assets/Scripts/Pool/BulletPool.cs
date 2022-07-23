using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance
    {
        get
        {
            return _instance;
        }
    }
    static BulletPool _instance;

    ObjectPool<Bullet> _bulletPool;

    public Bullet bulletPF;

    private void Awake()
    {
        _instance = this;
        _bulletPool = new ObjectPool<Bullet>(BulletFactory, Bullet.TurnOn, Bullet.TurnOff, 10);
    }

    public Bullet BulletFactory()
    {
        return Instantiate(bulletPF);
    }

    public void ReturnBullet(Bullet B)
    {
        _bulletPool.ReturnObject(B);
    }

    public Bullet SpawnBullet(Vector3 position, Vector3 rotation)
    {
        var b = _bulletPool.GetObject();
        b.transform.position = position;
        b.transform.localEulerAngles = rotation;
        return b;
    }
}
