using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleQuery2D : MonoBehaviour, IQuery
{
    //IA2-P2
    public SpatialGrid grid;
    public float rad;
    public static CircleQuery2D instance;
    public void Awake()
    {
        if (instance == null) instance = this;
              
    }

    public void Update()
    {
        //Space para testear el CircleQuery
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int currentenemys = 0;

            var elements = Query();
            foreach (var element in elements)
            {
                print(element);
                currentenemys++;
            }

            Debug.Log(currentenemys);
        }
    }

    public IEnumerable<IGridEntity> Query(Vector3 originPos)
    {
        transform.position = originPos;

        var from = originPos - new Vector3(rad, rad, 0);
        var to = originPos + new Vector3(rad, rad, 0);

        return grid.Query(from, to, pos => (originPos - pos).sqrMagnitude < rad * rad);
    }

    public IEnumerable<IGridEntity> Query()
    {
        var from = transform.position - new Vector3(rad, rad, 0);
        var to = transform.position + new Vector3(rad, rad, 0);

        return grid.Query(from, to, pos => (transform.position - pos).sqrMagnitude < rad * rad);
    }
    
    public IEnumerable<IGridEntity> Query(float rad)
    {
        var from = transform.position - new Vector3(rad, rad, 0);
        var to = transform.position + new Vector3(rad, rad, 0);

        return grid.Query(from, to, pos => (transform.position - pos).sqrMagnitude < rad * rad);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawWireSphere(transform.position, rad);

        Gizmos.color = Color.red;
        
        Gizmos.DrawWireCube(transform.position, new Vector3(rad* 2, rad * 2, 0));
    }
}
