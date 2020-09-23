using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Obstacle")
        {
            StartCoroutine(TransparentCoroutine(collider));
            //Destroy(collisionInfo.collider.gameObject);
        }
    }


    IEnumerator TransparentCoroutine(Collider collider)
    {
        Renderer r = collider.gameObject.GetComponent<Renderer>();
        Color newColor = r.material.color;
        for (float i = 0; newColor.a > 0; i += Time.deltaTime)
        {
            newColor.a -= 0.05f;
            r.material.color = newColor;
            yield return null;
        }
        Destroy(collider.gameObject);
        newColor.a -= 1f;
        r.material.color = newColor;
    }

}
