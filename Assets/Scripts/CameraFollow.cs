using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform playerTransform;
    private float offsetZ;
    public float startPosX = 0f;
    public float startPosY = 2f;
    private float posX;
    private float posY;
    private float offsetXY;

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (Application.isMobilePlatform)
            QualitySettings.vSyncCount = 0;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().transform;
        offsetZ = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Spawner>().cameraOffsetZ;
        offsetXY = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Spawner>().cameraOffsetXY;
        transform.position = new Vector3(startPosX, startPosY, playerTransform.position.z + offsetZ);
        posX = startPosX;
        posY = startPosY;
    }

    private void LateUpdate()
    {
        if(playerTransform.position.x - transform.position.x > offsetXY)
        {
            posX = playerTransform.position.x - offsetXY;
        }
        else if (transform.position.x - playerTransform.position.x > offsetXY)
        {
            posX = playerTransform.position.x + offsetXY;
        }
        if (playerTransform.position.y - transform.position.y > offsetXY)
        {
            posY = playerTransform.position.y - offsetXY;
        }
        else if (transform.position.y - playerTransform.position.y > offsetXY)
        {
            posY = playerTransform.position.y + offsetXY;
        }
        transform.position = new Vector3(posX, posY, playerTransform.position.z + offsetZ);
        transform.LookAt(playerTransform);
    }
}
