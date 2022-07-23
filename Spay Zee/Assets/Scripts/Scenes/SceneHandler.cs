using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour, IObserver
{
    static SceneHandler _instance;
    public static SceneHandler Instance{get { return _instance; }}

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        var temp = FindObjectsOfType<SceneHandler>();

        if (temp.Length > 1)
            for (int i = 1; i < temp.Length; i++)
                Destroy(temp[i]);

        Time.timeScale = 1;
    }

    public void Notify(string action)
    {
        //if (action == "NextLevel")
            //NextLevel(SceneManager.GetActiveScene().buildIndex + 1);
        if (action == "Lose")
            Lose();
        if (action == "Credits")
            Credits();
        if (action == "Menu")
            Menu();
        if (action == "Quit")
            Quit();
        if (action == "Play")
            StartGame();

    }

    private void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    void NextLevel(int Level)
    {
        SceneManager.LoadScene(Level);
    }

    void Lose()
    {
        SceneManager.LoadScene("Lose");
    }

    void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    void Quit()
    {
        Application.Quit();
    }
}
