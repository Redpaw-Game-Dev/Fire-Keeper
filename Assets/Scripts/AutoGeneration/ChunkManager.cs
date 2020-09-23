using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{

    private GameManager gameManager;
    private Transform playerTransformComponent;
    public Chunk[] chunkPrefabs;
    public Chunk firstChunk;
    private List<Chunk> spawnedChunks = new List<Chunk>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spawnedChunks.Add(firstChunk);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransformComponent.position.z > spawnedChunks[spawnedChunks.Count - 1].startPoint.transform.position.z + 3)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        Chunk newChunk = Instantiate(GetRandomChunk());
        newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].endPoint.transform.position;
        spawnedChunks.Add(newChunk);
        if(spawnedChunks.Count > 2)
        {
            DestroyImmediate(spawnedChunks[0].gameObject, true);
            spawnedChunks.RemoveAt(0);
            FindObjectOfType<GameManager>().SpeedUp();
        }
    }

    Chunk GetRandomChunk()
    {
        List<float> chances = new List<float>();
        for(int i = 0; i < chunkPrefabs.Length; i++)
        {
            chances.Add(chunkPrefabs[i].chanceFromDistance.Evaluate(gameManager.GetScore()));
        }

        float value = Random.Range(0, chances.Sum());
        float sum = 0;

        for(int i = 0; i < chances.Count; i++)
        {
            sum += chances[i];
            if(value < sum)
            {
                return chunkPrefabs[i];
            }
        }
        return chunkPrefabs[0];
    }

    public void GetPlayerTransform()
    {
        playerTransformComponent = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Transform GetStartPointFirstChunkTransform()
    {
        return spawnedChunks[0].startPoint.transform;
    }

    public void MoveChunksToStart()
    {
        spawnedChunks[0].gameObject.transform.position = new Vector3(spawnedChunks[0].gameObject.transform.position.x, spawnedChunks[0].gameObject.transform.position.y, 0);
        spawnedChunks[1].gameObject.transform.position = new Vector3(spawnedChunks[1].gameObject.transform.position.x, spawnedChunks[1].gameObject.transform.position.y, 40);
    }

    public Chunk GetLastChunk()
    {
        return spawnedChunks[spawnedChunks.Count - 1];
    }
}
