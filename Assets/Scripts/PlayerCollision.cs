using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement playerMovementComponent;
    private Transform cameraTransform;
    private Transform obstacleDestroyerTransform;

    public AudioSource coalAudio;
    public AudioSource loseAudio;

    private void Start()
    {
        playerMovementComponent = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (playerMovementComponent.enabled)
        {
            if (collisionInfo.collider.tag == "Obstacle" || collisionInfo.collider.tag == "ObstacleNonDestroyable")
            {
                loseAudio.Play();
                playerMovementComponent.enabled = false;
                GameObject.FindObjectOfType<CameraFollow>().enabled = false;
                GetComponent<Renderer>().enabled = false;
                GameObject.FindObjectOfType<GameManager>().EndGame();
            }
        }
        
    }

    private void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.tag == "Edge")
        {
            float dif = playerMovementComponent.transform.position.z - FindObjectOfType<ChunkManager>().GetStartPointFirstChunkTransform().position.z;
            playerMovementComponent.transform.position = new Vector3(playerMovementComponent.transform.position.x, playerMovementComponent.transform.position.y, dif);
            cameraTransform.position.Set(cameraTransform.position.x, cameraTransform.position.y, -4);
            obstacleDestroyerTransform.position.Set(obstacleDestroyerTransform.position.x, obstacleDestroyerTransform.position.y, FindObjectOfType<Spawner>().obstacleDestroyerOffsetZ);
            Transform obstacleSpawnerTransform = GameObject.FindGameObjectWithTag("ObstacleSpawner").transform;
            obstacleSpawnerTransform.position.Set(obstacleSpawnerTransform.position.x, obstacleSpawnerTransform.position.y, FindObjectOfType<Spawner>().obstacleSpawnerOffsetZ);
            FindObjectOfType<ChunkManager>().MoveChunksToStart();
        }
        if (colliderInfo.tag == "Coal")
        {
            coalAudio.Play();
            FindObjectOfType<GameManager>().AddBrightness();
            Destroy(colliderInfo.gameObject);
        }
        if (colliderInfo.tag == "SpawnObstacleSpawner")
        {
            if (!GameObject.FindGameObjectWithTag("ObstacleSpawner"))
            {
                FindObjectOfType<Spawner>().SpawnObstacleSpawner();
            }
        }
        if (colliderInfo.tag == "DestroyObstacleSpawner")
        {
            if (GameObject.FindGameObjectsWithTag("DestroyObstacleSpawner").Length < 2)
            {
                Destroy(GameObject.FindGameObjectWithTag("ObstacleSpawner"));
            }
        }
    }

    public void EdgeInit()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        obstacleDestroyerTransform = GameObject.FindGameObjectWithTag("ObstacleDestroyer").GetComponent<Transform>();
    }
}
