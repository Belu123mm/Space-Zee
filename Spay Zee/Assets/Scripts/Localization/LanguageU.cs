using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class LanguageU
{
    public static Dictionary<Language, Dictionary<string, string>> loadCodexFromString(string source, string sheet)
    {
        var codex = new Dictionary<Language, Dictionary<string, string>>();
        
        int lineNum = 0;
        
        var rows = sheet.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        
        bool first = true;
        var columnToIndex = new Dictionary<string, int>();
        
        foreach (var row in rows)
        {
            lineNum++;
            var cells = row.Split(';');
            
            if (first)
            {
                first = false;
                for (int i = 0; i < cells.Length; i++)
                    columnToIndex[cells[i]] = i; 
                continue;
            }
            
            if (cells.Length != columnToIndex.Count)
            {
                Debug.Log(string.Format("Parsing CSV file {2} at line {0} columns {1} should be 4", lineNum, cells.Length, source));
                continue;
            }
            var langName = cells[columnToIndex["Idioma"]];

            Language lang;

            try
            {
                lang = (Language)Enum.Parse(typeof(Language), langName);
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Parsing CSV file {2} at line {0} invalid language {1}", lineNum, langName, source));
                Debug.Log(e.ToString());
                continue;
            }

            var idName = cells[columnToIndex["ID"]];
            var text = cells[columnToIndex["Texto"]];

            if (!codex.ContainsKey(lang))
            {
                codex[lang] = new Dictionary<string, string>();
            }
            codex[lang][idName] = text;
        }
        return codex;
    }
}
