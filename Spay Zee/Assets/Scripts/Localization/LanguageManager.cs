using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;

public enum Language
{
    en,
    es
}

public class LanguageManager : MonoBehaviour
{    
    public Dictionary<Language, Dictionary<string, string>> LangManager;
    
    string externalUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQoPCU6v5yOVr7enyq8825SefU4vuRVtdetVCYA8e-Hn8tBF87N9NvNrfGdsCTiw3iCzlK5ItyTWcNf/pubhtml?gid=0&single=true";
    
    public event Action OnUpdate = delegate { };
    
    void Awake()
    {
        StartCoroutine(DownloadCSV(externalUrl));
    }

    public string GetTranslate(string _id)
    {
        if (!LangManager[CurrentLanguage.currentLang].ContainsKey(_id))
            return "Error loading the text.";
        else
            return LangManager[CurrentLanguage.currentLang][_id];
    }

    public IEnumerator DownloadCSV(string url)
    {
        var www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        LangManager = LanguageU.loadCodexFromString("www", www.downloadHandler.text);
        OnUpdate();
    }

    public void UpdateLanguage()
    {
        if(CurrentLanguage.currentLang == Language.en)
            CurrentLanguage.currentLang = Language.es;
        else
            CurrentLanguage.currentLang = Language.en;

        OnUpdate();
    }
}
