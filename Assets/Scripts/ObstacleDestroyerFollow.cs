using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyerFollow : MonoBehaviour
{
    private Transform playerTransform;
    private float offsetZ;
    private float posX;
    private float posY;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().transform;
        offsetZ = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Spawner>().obstacleDestroyerOffsetZ;
        posX = playerTransform.position.x;
        posY = playerTransform.position.y;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(posX, posY, playerTransform.position.z - offsetZ);
    }
}
