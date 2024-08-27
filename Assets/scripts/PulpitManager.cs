using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PulpitSpawner : MonoBehaviour
{
    public GameObject pulpitPrefab;
    public Transform[] spawnLocations;
    public TextMesh timerText;

    private float scaleDuration = 0.1f;
    private Vector3 scaleUpSize = new(9, 0.4f, 9);

    private List<GameObject> activePulpits = new();

    private float minPulpitTime;
    private float maxPulpitTime;
    private float spawnDelay;

    private int lastSpawnIndex = -1;
    private Dictionary<int, List<int>> adjacencyList;

    private int pulpitCount = 0;

    void Start()
    {
        LoadPulpitData();
        InitializeAdjacencyList();
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

    void LoadPulpitData()
    {
        string filePath = Application.dataPath + "/DoofusDiary.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PulpitData pulpitData = JsonUtility.FromJson<PulpitData>(json);

            minPulpitTime = pulpitData.pulpit_data.min_pulpit_destroy_time;
            maxPulpitTime = pulpitData.pulpit_data.max_pulpit_destroy_time;
            spawnDelay = pulpitData.pulpit_data.pulpit_spawn_time;
        }
        else
        {
            Debug.LogError("The file is missing!");
        }
    }

    IEnumerator SpawnPulpit()
    {
        while (true)
        {
            if (activePulpits.Count < 2)
            {
                int spawnIndex = GetNextSpawnIndex();
                GameObject newPulpit = Instantiate(pulpitPrefab, spawnLocations[spawnIndex].position, Quaternion.identity);

                newPulpit.name = "Pulpit_" + pulpitCount;
                pulpitCount++;
                newPulpit.transform.localScale = Vector3.zero;
                activePulpits.Add(newPulpit);
                float pulpitLifetime = Random.Range(minPulpitTime, maxPulpitTime);

                StartCoroutine(ScalePulpit(newPulpit.transform, scaleUpSize, scaleDuration));
                StartCoroutine(UpdateTimer(newPulpit, pulpitLifetime));
                StartCoroutine(HandlePulpitDestruction(newPulpit, pulpitLifetime));

                lastSpawnIndex = spawnIndex;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    int GetNextSpawnIndex()
    {
        if (lastSpawnIndex == -1)
            return 5;

        List<int> possibleIndices = adjacencyList[lastSpawnIndex];
        return possibleIndices[Random.Range(0, possibleIndices.Count)];
    }

    IEnumerator HandlePulpitDestruction(GameObject pulpit, float delay)
    {
        yield return new WaitForSeconds(delay - scaleDuration);
        StartCoroutine(ScalePulpit(pulpit.transform, Vector3.zero, scaleDuration));
        yield return new WaitForSeconds(scaleDuration);

        if (pulpit != null)
        {
            activePulpits.Remove(pulpit);
            Destroy(pulpit);
        }
    }

    IEnumerator UpdateTimer(GameObject pulpit, float duration)
    {
        float remainingTime = duration;
        TextMesh pulpitTextMesh = pulpit.GetComponentInChildren<TextMesh>();

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            if (pulpitTextMesh != null)
            {
                pulpitTextMesh.text = "Time: " + remainingTime.ToString("F2") + "s";
            }
            else
            {
                yield break;
            }

            yield return null;
        }

        if (pulpitTextMesh != null)
        {
            pulpitTextMesh.text = "Time Left: 0.00s";
        }
    }

    IEnumerator ScalePulpit(Transform pulpitTransform, Vector3 targetScale, float duration)
    {
        if (pulpitTransform == null) yield break;
        Vector3 initialScale = pulpitTransform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (pulpitTransform != null)
            {
                pulpitTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            }
            yield return null;
        }

        if (pulpitTransform != null)
        {
            pulpitTransform.localScale = targetScale;
        }
    }
}

[System.Serializable]
public class PulpitData
{
    public Pulpit pulpit_data;
}

[System.Serializable]
public class Pulpit
{
    public float min_pulpit_destroy_time;
    public float max_pulpit_destroy_time;
    public float pulpit_spawn_time;
}