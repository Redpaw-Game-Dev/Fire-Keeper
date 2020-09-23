    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnimation : MonoBehaviour
{

    private float animSpeed = 1f;
    private float x;
    private float y;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        x = Random.Range(-1, 1);
        y = Random.Range(-1, 1);
        z = Random.Range(-1, 1);
        StartCoroutine(AnimCoroutine());
    }

    IEnumerator AnimCoroutine()
    {
        for (float i = 0; ; i += Time.deltaTime * animSpeed)
        {
            transform.Rotate(animSpeed * x, animSpeed * y, animSpeed * z);
            yield return null;
        }
    }
}
