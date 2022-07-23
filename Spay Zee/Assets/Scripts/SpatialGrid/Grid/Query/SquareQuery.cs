using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareQuery : MonoBehaviour, IQuery {

    public SpatialGrid             targetGrid;
    public float                   width    = 15f;
    public float                   height   = 30f;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    public IEnumerable<IGridEntity> Query() {
        var h = height * 0.5f;
        var w = width  * 0.5f;
        //posicion inicial --> esquina superior izquierda de la "caja"
        //posición final --> esquina inferior derecha de la "caja"
        //como funcion para filtrar le damos una que siempre devuelve true, para que no filtre nada.
        return targetGrid.Query(
                                transform.position + new Vector3(-w, -h, 0),
                                transform.position + new Vector3(w,  h, 0),
                                x => true);
    }

    public IEnumerable<IGridEntity> Query(Vector3 originPos)
    {
        throw new System.NotImplementedException();
    }

    void OnDrawGizmos() {
        if (targetGrid == null) return;

        //Flatten the sphere we're going to draw
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
}