using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitPlatformSpawner : MonoBehaviour
{
    public GameObject pulpitPrefab;
    public Transform[] spawnLocations;
    public TextMesh timerText;

    private List<GameObject> activePulpits = new List<GameObject>();

    // Json Values
    private float minPulpitTime = 4.0f;
    private float maxPulpitTime = 5.0f;
    private float spawnDelay = 2.5f;

    private int lastSpawnIndex = -1;
    private Dictionary<int, List<int>> adjacencyList;

    private int pulpitCount = 0;

    void Start()
    {
        InitializeAdjacencyList();

        // Spawn the first pulpit 
        SpawnInitialPulpit(0);

        // Start the coroutine for subsequent pulpits
        StartCoroutine(SpawnPulpit());
    }

    void InitializeAdjacencyList()
    {
        adjacencyList = new Dictionary<int, List<int>>()
        {
            { 0, new List<int> { 1, 4 } },
            { 1, new List<int> { 0, 2, 5 } },
            { 2, new List<int> { 1, 3, 6 } },
            { 3, new List<int> { 2, 7 } },
            { 4, new List<int> { 0, 5, 8 } },
            { 5, new List<int> { 1, 4, 6, 9 } },
            { 6, new List<int> { 2, 5, 7, 10 } },
            { 7, new List<int> { 3, 6, 11 } },
            { 8, new List<int> { 4, 9 } },
            { 9, new List<int> { 5, 8, 10 } },
            { 10, new List<int> { 6, 9, 11 } },
            { 11, new List<int> { 7, 10 } }
        };
    }

    void SpawnInitialPulpit(int spawnIndex)
    {
        GameObject pulpit = Instantiate(pulpitPrefab, spawnLocations[spawnIndex].position, Quaternion.identity);
        pulpit.name = "capsule" + pulpitCount;  
        Debug.Log("Spawned initial pulpit: " + pulpit.name + " at position: " + pulpit.transform.position);

        activePulpits.Add(pulpit);
        pulpitCount++;  

        Debug.Log("pulpitCount after initial spawn: " + pulpitCount);

        StartCoroutine(DestroyPulpitAfterDelay(pulpit));
        lastSpawnIndex = spawnIndex;
    }

    IEnumerator SpawnPulpit()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            // Spawn the next pulpit adjacent to the last one
            int spawnIndex = GetNextSpawnIndex();
            GameObject pulpit = Instantiate(pulpitPrefab, spawnLocations[spawnIndex].position, Quaternion.identity);
            pulpit.name = "capsule" + pulpitCount; 
            Debug.Log("Spawned pulpit: " + pulpit.name + " at position: " + pulpit.transform.position);

            activePulpits.Add(pulpit);
            pulpitCount++; 

            Debug.Log("pulpitCount after spawn: " + pulpitCount);

            
            StartCoroutine(DestroyPulpitAfterDelay(pulpit));
        }
    }

    int GetNextSpawnIndex()
    {
        List<int> possibleIndices = adjacencyList[lastSpawnIndex];
        int newIndex;
        do
        {
            newIndex = possibleIndices[Random.Range(0, possibleIndices.Count)];
        } while (newIndex == lastSpawnIndex);

        lastSpawnIndex = newIndex;
        return newIndex;
    }

    IEnumerator DestroyPulpitAfterDelay(GameObject pulpit)
    {
        float destroyTime = Random.Range(minPulpitTime, maxPulpitTime);
        yield return new WaitForSeconds(destroyTime);
        activePulpits.Remove(pulpit);
        Destroy(pulpit);

    }
}
