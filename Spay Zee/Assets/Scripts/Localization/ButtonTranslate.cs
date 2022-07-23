using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTranslate : MonoBehaviour
{
    public string ID;
    public LanguageManager manager;
    public Text myView;
    string myText = "";

    void Awake()
    {
        myText = myView.GetComponent<Text>().text;
        manager.OnUpdate += ChangeLang;
    }

    void ChangeLang()
    {
        myView.text = manager.GetTranslate(ID);
    }
}
