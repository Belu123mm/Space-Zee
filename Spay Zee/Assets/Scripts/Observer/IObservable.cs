using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
    void SubscribeTo(IObserver observer);
    void UnSubscribeFrom(IObserver observer);
    void NotifyObservers(string action);
}
