using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> skins = new List<GameObject>();
    public GameObject cameraPrefab;
    public GameObject obstacleDestroyerPrefab;
    public Transform cameraSpawnPoint;
    public float cameraOffsetZ = -2f;
    public float cameraOffsetXY = 0.5f;
    public float obstacleDestroyerOffsetZ = 0f;
    public float obstacleSpawnerOffsetZ = 0f;
    public GameObject obstacleSpawnerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(skins[(int)PlayerPrefsSafe.GetFloat("SelectedSkin")], transform.position, Quaternion.identity);
        
        Instantiate(cameraPrefab, cameraSpawnPoint.position, Quaternion.identity);
        FindObjectOfType<GameManager>().SetGameSettings();
        Instantiate(obstacleDestroyerPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z - obstacleDestroyerOffsetZ), Quaternion.identity);
        FindObjectOfType<PlayerCollision>().EdgeInit();
        FindObjectOfType<ChunkManager>().GetPlayerTransform();
    }

    public void SpawnObstacleSpawner()
    {
        Instantiate(obstacleSpawnerPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + obstacleSpawnerOffsetZ), Quaternion.identity);
    }
}
