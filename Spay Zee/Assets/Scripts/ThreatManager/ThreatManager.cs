using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ThreatManager : MonoBehaviour
{
    private float TotalHP;
    private float ActualHP;
    private float LifePercentage;
    private float xValue;
    //private float processedX;
    public Image ThreatLevel;
    
    void Start()
    {
        TotalHP = 0;
        ActualHP = 0;
        LifePercentage = 1;
        //processedX = 0;
    }

    void Update()
    {
        //IA2-P2
        //IA2-P3
        ActualHP = CircleQuery2D.instance
            .Query(100)
            .Select(n => n.GetGO().GetComponent<Enemy>())
            .Where(n => n.GetGO().activeInHierarchy)
            .Select(n => n.GetCurrentHP)
            .Aggregate(0, (acum, current) => acum + current);
        
        if (ActualHP >= TotalHP)
        {
            TotalHP = ActualHP;
            ThreatLevel.rectTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            //print(ActualHP);
            //print(TotalHP);
            LifePercentage = ((ActualHP * 10) / TotalHP) / 10;
            //print(LifePercentage);
            ThreatLevel.rectTransform.localScale = new Vector3(LifePercentage,1,1);
        }
    }
}
