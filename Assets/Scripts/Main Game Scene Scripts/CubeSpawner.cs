using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    public Vector3 spawnArea;

    public float beat = 1f;
    public float MaxOffset = 0.3f;
    public float lockedCubeOffset = 0.3f;
    public float offset = 0.2f;
    public GameObject[] cubeType;

    public GameObject spawnPrefab;

    public bool isSpawner = true;

    private bool keepSpawning = true;

    private int activeButtons;

    public int spawnAmount;

    private int spawnCount = 1;

    public float spawnInterval;
    //Doing the spawn system like this allows you to add more events that happen at the same time as the spawn one
    public UnityEvent OnTimer;


    // Start is called before the first frame update
    void Start()
    {   
        if (!isSpawner) return;
        
        if (NewGameManager.Instance.levelIsOne) {
            EventRespawn();
        }
        
    }

    public void StartTimedSpawn() {
        if (keepSpawning) { 
            StartCoroutine(StartTimer());
        }
        else if (!keepSpawning)
        {
            StopAllCoroutines();
        }
    }

    public void EventRespawn() { 
        
            Spawn();

    }
    public void StopSpawning() { 
        keepSpawning = false;
    }

    IEnumerator StartTimer()
    {
        OnTimer.Invoke();
        if (spawnInterval == 0)
            yield break;

        while (keepSpawning)
        {   
            yield return new WaitForSeconds(spawnInterval);
            OnTimer.Invoke();
        }
    }

    //Spawns stationary cubes in random positions on a 2D plane
    public void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float x = spawnArea.x / 2;
            float y = spawnArea.y / 2;
            float z = spawnArea.z / 2;

            x = Random.Range(-x, x);
            y = Random.Range(-y, y);
            z = Random.Range(-z, z);

            Vector3 spawnPos = new Vector3(x, y, z);

            Instantiate(spawnPrefab, transform.position + spawnPos, Quaternion.identity);
        }
    }

    IEnumerator SpawnMirrored()
    {
        yield return new WaitForSeconds(beat);

        float offset = Random.Range(0, MaxOffset); // for symmetric cubes + for one cube - for other.
        Vector3 cubeSP1 = transform.position; cubeSP1.x += offset;
        Vector3 cubeSP2 = transform.position; cubeSP2.x -= offset;

        GameObject newCube1 = Instantiate(cubeType[0], cubeSP1, Quaternion.identity);
        GameObject newCube2 = Instantiate(cubeType[0], cubeSP2, Quaternion.identity);
    }

    IEnumerator SpawnLocked()
    {
        yield return new WaitForSeconds(beat);

        Vector3 middleSpawnPos = transform.position;
        middleSpawnPos.x += Random.Range(-offset, offset); // randomize central x position

        // Locked hand version - spawn two cubes with 1/2 offset each side (middleSpawnPos is the middle of the two cubes).
        // E.g if lockedCubeOffset = 0.3f. Then the two cubes will spawn at x += -0.15f and x += 0.15f from middleSpawnPos.x.
        Vector3 cubeSP1 = middleSpawnPos;
        Vector3 cubeSP2 = middleSpawnPos;

        cubeSP1.x += -(0.5f * lockedCubeOffset);
        cubeSP2.x += (0.5f * lockedCubeOffset);

        GameObject newCube1 = Instantiate(cubeType[0], cubeSP1, Quaternion.identity);
        GameObject newCube2 = Instantiate(cubeType[0], cubeSP2, Quaternion.identity);

        // cubes.Add(newCube); // add the cube to the list of active cubes

    }

    }

