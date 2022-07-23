using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T>
{
    public delegate T  FactoryMethod();

    List<T> _curentStock;
    FactoryMethod _factoryMethod;
    bool _dynamic;
    Action<T> _turnOnCallBack;
    Action<T> _turnOffCallBack;

    public ObjectPool(FactoryMethod factory, Action<T> turnOn, Action<T> turnOff, int initialStock = 0, bool dynamic = true)
    {
        _factoryMethod = factory;
        _dynamic = dynamic;

        _turnOffCallBack = turnOff;
        _turnOnCallBack = turnOn;

        _curentStock = new List<T>();

        for (int i = 0; i < initialStock; i++)
        {
            var o = _factoryMethod();
            _turnOffCallBack(o);
            _curentStock.Add(o);
        }
    }

    public T GetObject()
    {
        var obj = default(T);

        if (_curentStock.Count > 0)
        {
            obj = _curentStock[0];
            _curentStock.RemoveAt(0);
        }
        else if (_dynamic)
            obj = _factoryMethod();
        _turnOnCallBack(obj);
        return obj;
    }
    public void ReturnObject(T obj)
    {
        _turnOffCallBack(obj);
        _curentStock.Add(obj);
    }
}
