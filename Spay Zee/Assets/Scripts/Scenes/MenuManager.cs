using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour, IObservable
{
    //public GameObject quitThang;

    List<IObserver> _allObs;

    private void Start()
    {
        _allObs = new List<IObserver>();
        SubscribeTo(SceneHandler.Instance);
        //quitThang.SetActive(false);
    }

    public void GoToMenu()
    {
        NotifyObservers("Menu");
    }

    public void GoToPlay()
    {
        NotifyObservers("Play");
    }

    public void GoToCredits()
    {
        NotifyObservers("Credits");
    }

    public void GoToDesktop()
    {
        NotifyObservers("Quit");
    }

    public void GoToQuit()
    {
        Application.Quit();
    }

    public void NotifyObservers(string action)
    {
        for (int i = 0; i < _allObs.Count; i++)
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
