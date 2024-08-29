using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PulpitSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject PulpitPrefab; // Prefab 
    public Transform[] spawnPoints; // Array of possible spawn points for the platforms

    [Header("Color Settings")]
    public Color[] pulpitColors; // Array of possible colors for the platforms

    [Header("Timing Settings")]
    [SerializeField] private float minPulpitTime = 4.0f;
    [SerializeField] private float maxPulpitTime = 5.0f;
    [SerializeField] private float spawnDelay = 2.5f;

    private List<GameObject> currentPlatforms = new List<GameObject>(); // List to keep track of active platforms
    private int previousSpawnIndex = -1; // Index of the last spawned platform
    private int numColumns = 4; // Number of columns in the grid
    private int numRows = 3; // Number of rows in the grid
    private int platformCounter = 0; // Counter to keep track of platform instances

    void Awake()
    {
        // Place platform at the starting index
        PlaceInitialPlatform(0);
        // Start the coroutine to spawn platforms with a delay
        StartCoroutine(SpawnPlatformWithDelay());
    }

    private void PlaceInitialPlatform(int index)
    {
        GameObject platform = CreatePlatform(index);
        currentPlatforms.Add(platform);
        platformCounter++; // Increment the platform counter
        previousSpawnIndex = index; // Update the last spawned index
    }

    private IEnumerator SpawnPlatformWithDelay()
    {
        while (true)
        {
            // Wait for the delay before spawning a new platform
            yield return new WaitForSeconds(spawnDelay);

            // Check if fewer than two platforms are active
            if (currentPlatforms.Count < 2)
            {
                int nextSpawnIndex = DetermineNextSpawnIndex();

                if (nextSpawnIndex != -1)
                {
                    GameObject platform = CreatePlatform(nextSpawnIndex);
                    currentPlatforms.Add(platform);
                    platformCounter++; // Increment the platform counter
                }
            }
        }
    }

    private int DetermineNextSpawnIndex()
    {
        // If no previous spawn index, pick a random index from available spawn points
        if (previousSpawnIndex == -1) return Random.Range(0, spawnPoints.Length);

        // Get the indices of adjacent points
        List<int> neighbors = FindAdjacentIndices(previousSpawnIndex);
        if (neighbors.Count == 0)
        {
            // Log an error if no adjacent indices are found
            Debug.LogError($"No adjacent points found for index {previousSpawnIndex}");
            return -1;
        }

        int newIndex;
        do
        {
            // Pick a random adjacent index that is different from the previous one
            newIndex = neighbors[Random.Range(0, neighbors.Count)];
        } while (newIndex == previousSpawnIndex && neighbors.Count > 1);

        previousSpawnIndex = newIndex; // Update the last spawned index
        return newIndex;
    }

    private List<int> FindAdjacentIndices(int index)
    {
        List<int> adjIndices = new List<int>();

        // Calculate column and row from the index
        int col = index % numColumns;
        int row = index / numColumns;

        // Add adjacent indices based on grid position
        if (col > 0) adjIndices.Add(index - 1); // Check left
        if (col < numColumns - 1) adjIndices.Add(index + 1); // Check right
        if (row > 0) adjIndices.Add(index - numColumns); // Check above
        if (row < numRows - 1) adjIndices.Add(index + numColumns); // Check below

        return adjIndices;
    }

    private GameObject CreatePlatform(int index)
    {
        // Instantiate the Pulpit prefab at the given spawn point
        GameObject platform = Instantiate(PulpitPrefab, spawnPoints[index].position, Quaternion.identity);
        platform.name = "platform" + platformCounter; // Set a unique name for the platform for scoring

        // Assign a random color to the platform's material
        Renderer renderer = platform.GetComponent<Renderer>();
        if (renderer != null && pulpitColors.Length > 0)
        {
            renderer.material.color = pulpitColors[Random.Range(0, pulpitColors.Length)];
        }

        // Start a coroutine to destroy the platform after its lifetime
        StartCoroutine(RemovePlatformAfterLifetime(platform));
        return platform;
    }

    private IEnumerator RemovePlatformAfterLifetime(GameObject platform)
    {
        // Determine how long the platform will stay before being destroyed
        float lifetime = Random.Range(minPulpitTime, maxPulpitTime);
        // Wait for the duration of the lifetime
        yield return new WaitForSeconds(lifetime);
        // Remove the platform from the list and destroy it
        currentPlatforms.Remove(platform);
        Destroy(platform);
    }
}
