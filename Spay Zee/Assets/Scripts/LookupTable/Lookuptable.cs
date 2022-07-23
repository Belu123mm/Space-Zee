using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookuptable<T1, T2>
{
    public delegate T2 FactoryMethod(T1 ketToReturn);

    Dictionary<T1, T2> _table = new Dictionary<T1, T2>();

    FactoryMethod _factoryMethod;

    public Lookuptable(FactoryMethod myFactory)
    {
        _factoryMethod = myFactory;

    }

    public T2 ReturnValue(T1 myKey)
    {
        if (_table.ContainsKey(myKey))
            return _table[myKey];
        else
        {
            var value = _factoryMethod(myKey);
            _table[myKey] = value;
            return value;
        }
    }
}
