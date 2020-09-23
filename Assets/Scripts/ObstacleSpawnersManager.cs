using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnersManager : MonoBehaviour
{
    List<int> indexes = new List<int>();
    private Transform playerTransform;
    private float offsetZ;
    private float posX;
    private float posY;
    public float timerMax = 5f;
    public float timerMin = 3f;
    private float timerTime;
    public GameObject obstaclesPrefab;
    public GameObject coalPrefab;
    public List<GameObject> spawnPoints;
    public int obstaclesMax = 5;
    public int obstaclesMin = 2;
    private int obstaclesCount;
    public int coalsMax = 2;
    public int coalsMin = 0;
    private int coalsCount;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(coalPrefab.name);
        timerTime = timerMax;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().transform;
        offsetZ = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Spawner>().obstacleSpawnerOffsetZ;
        posX = 0f;
        posY = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        timerTime -= Time.deltaTime;

        if (timerTime <= 0.0f)
        {
            timerEnded();
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(posX, posY, playerTransform.position.z + offsetZ);
    }

    private void timerEnded()
    {
        obstaclesCount = Random.Range(obstaclesMin, obstaclesMax);
        coalsCount = Random.Range(coalsMin, coalsMax);
        for (int j = 0; j < obstaclesCount + coalsCount; j++)
        {
            indexes.Add(UniqueRandomInt(0, spawnPoints.Count));
        }
        int i;
        for (i = 0; i < obstaclesCount; i++)
        {
            var newObstacle = Instantiate(obstaclesPrefab, spawnPoints[indexes[i]].transform.position, Quaternion.identity);
            newObstacle.transform.parent = FindObjectOfType<ChunkManager>().GetLastChunk().transform;
        }
        for (; i < obstaclesCount + coalsCount; i++)
        {
            var newCoal = Instantiate(coalPrefab, spawnPoints[indexes[i]].transform.position, Quaternion.identity);
            newCoal.transform.parent = FindObjectOfType<ChunkManager>().GetLastChunk().transform;
        }
        if (FindObjectOfType<GameManager>().GetScore() < 2000)
        {
            timerTime = timerMax;
        }
        else if (FindObjectOfType<GameManager>().GetScore() < 3000)
        {
            timerTime = timerMax - 1;
        }
        else if (FindObjectOfType<GameManager>().GetScore() < 4000)
        {
            timerTime = timerMin;
        }
        indexes.Clear();
    }

    public int UniqueRandomInt(int min, int max)
    {
        int val = Random.Range(min, max);
        while (indexes.Contains(val))
        {
            val = Random.Range(min, max);
        }
        return val;
    }

}
