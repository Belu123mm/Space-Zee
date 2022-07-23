using System.Collections.Generic;
using UnityEngine;

public interface IQuery {

    IEnumerable<IGridEntity> Query();
    IEnumerable<IGridEntity> Query(Vector3 originPos);
}
