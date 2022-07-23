using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombEffect : MonoBehaviour
{
    float counter;

    void Update()
    {
        counter += Time.deltaTime;

        if (counter >= 0.5f)
            Destroy(this.gameObject);
    }
}
