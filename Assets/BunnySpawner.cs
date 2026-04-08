using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnySpawner : MonoBehaviour
{
    [SerializeField] Transform tableTransform;
    [SerializeField] Vector2 spawnAreaSize; // the table size
    [SerializeField] float yOffset;
    [SerializeField] float tablePadding;
    //// hand collider positions
    //[SerializeField] Transform leftPokeInteractor;
    //[SerializeField] Transform rightPokeInteractor;

    //[SerializeField] Transform leftMiddleMetacarpalTip;
    //[SerializeField] Transform rightMiddleMetacarpalTip;

    //[SerializeField] Transform leftIndexTip;
    //[SerializeField] Transform rightIndexTip;

    [SerializeField] Transform leftMiddleMetacarpal;
    [SerializeField] Transform rightMiddleMetacarpal;

    [SerializeField] GameObject bunnyPrefab;
    [SerializeField] float spawnIntervalMin;
    [SerializeField] float spawnIntervalMax;
    [SerializeField] float offsetDistanceMin;
    [SerializeField] float offsetDistanceMax;
    [SerializeField] float minOffsetDistanceFloor;
    [SerializeField] float despawnTimerMin;
    [SerializeField] float despawnTimerMax;
    [SerializeField] float despawnDifficultyThreshold;
    [SerializeField] int maxLevel;

    //private Vector3 handCenter;
    private float currentDifficulty;
    private bool isSpawning;
    private bool timedMode = false;

    public void StartTimedSpawn()
    {
        timedMode = true;
        StartCoroutine(TimedSpawn());
    }

    public void StopTimedSpawn()
    {
        timedMode = false;
    }

    IEnumerator DelayedFirstSpawn()
    {
        yield return new WaitForSeconds(2f);
        Spawn();
    }
    public void StartSpawning() {         
        isSpawning = true;
    }

    public void StopSpawning() {
        isSpawning = false;
    }

    //public void EventRespawn() {
    //    Spawn();
    //}

    public float GetSpawnInterval() {
        GetCurrentDifficulty();
        float spawnInterval = Mathf.Lerp(spawnIntervalMax, spawnIntervalMin, currentDifficulty);
        return spawnInterval;
    }
    public void RequestRespawn()
    {
        StartCoroutine(DelayedRespawn());
    }

    IEnumerator TimedSpawn()
    {
        while (timedMode)
        {
            Spawn();
            float interval = GetSpawnInterval();
            yield return new WaitForSeconds(interval);
        }

    }
    IEnumerator DelayedRespawn()
    {
        GetCurrentDifficulty();
        float interval = GetSpawnInterval();
        Debug.Log("SPAWNERDEBUG: DelayedRespawn interval = " + interval + " at difficulty " + currentDifficulty);
        yield return new WaitForSeconds(interval);
        Spawn();
    }

    private void Spawn() {
        if (isSpawning) {
            GetCurrentDifficulty();
            //FindHandCenter();
            Vector3 bunnyPosition = NewSpawnPosition();
            GameObject bunny = Instantiate(bunnyPrefab, bunnyPosition, Quaternion.identity);
            if (currentDifficulty >= despawnDifficultyThreshold)
            {
                // set despawn timer based on difficulty
                float despawnTime = Mathf.Lerp(despawnTimerMax, despawnTimerMin, currentDifficulty);
                bunny.GetComponent<RabbitBehaviour>().SetDespawnTimer(despawnTime);
            }
        }
    }

    public void BunnyMissed() { 
        NewGameManager.Instance.RecordMiss();
        Spawn();
    }

    private void GetCurrentDifficulty() {
        // calculate difficulty as a float clamped between 0 and 1
        currentDifficulty = Mathf.Clamp01((NewGameManager.Instance.level - 1f) / (maxLevel - 1f));
    }

    //private void FindHandCenter() { 
    //    float avgXValue = (leftMiddleMetacarpalTip.position.x + rightMiddleMetacarpalTip.position.x) / 2f;  
    //    float avgZValue = (leftMiddleMetacarpalTip.position.z + rightMiddleMetacarpalTip.position.z) / 2f;
    //    float yValue = tableTransform.position.y;
    //    handCenter = new Vector3(avgXValue, yValue, avgZValue);
    //}

    //private Vector3 GetSpawnPosition() {
    //    var randomPosition = Random.insideUnitCircle; // get random position in a circle with radius 1.0
    //    float currentMinOffset = Mathf.Lerp(offsetDistanceMin, minOffsetDistanceFloor, currentDifficulty);
    //    float offsetDistance = Mathf.Lerp(currentMinOffset, offsetDistanceMax, currentDifficulty);

    //    float averageIndexZ = (leftIndexTip.position.z + rightIndexTip.position.z) / 2f;

    //    // find random, but valid spawn position offsets from hand colliders
    //    float xOffset = handCenter.x + randomPosition.x * offsetDistance;
    //    float zOffset = handCenter.z + randomPosition.y * offsetDistance;
    //    // clamp the offsets to be within the spawn area
    //    float xBounded = Mathf.Clamp(xOffset, tableTransform.position.x - spawnAreaSize.x / 2, tableTransform.position.x + spawnAreaSize.x / 2);
    //    float zBounded = Mathf.Clamp(zOffset, tableTransform.position.z - spawnAreaSize.y / 2, tableTransform.position.z + spawnAreaSize.y / 2);
    //    Vector3 spawnPosition = new Vector3(xBounded, handCenter.y, zBounded);
    //    return spawnPosition;
    //}

    private Vector3 NewSpawnPosition() {
        float outerRadius = Mathf.Lerp(offsetDistanceMin, offsetDistanceMax, currentDifficulty);
        Transform[] fingerBase = { leftMiddleMetacarpal, rightMiddleMetacarpal };
        Transform randomBase = fingerBase[Random.Range(0, fingerBase.Length)];
        float baseAngle = Mathf.Atan2(randomBase.forward.z, randomBase.forward.x) * Mathf.Rad2Deg;
        float randomAngle = baseAngle + Random.Range(-150f, 150f);
        float randomX = randomBase.position.x + outerRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float randomZ = randomBase.position.z + outerRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
        float clampedX = Mathf.Clamp(randomX, tableTransform.position.x - spawnAreaSize.x / 2 + tablePadding, tableTransform.position.x + spawnAreaSize.x / 2 - tablePadding);
        float clampedZ = Mathf.Clamp(randomZ, tableTransform.position.z - spawnAreaSize.y / 2 + tablePadding, tableTransform.position.z + spawnAreaSize.y / 2 - tablePadding);
        return new Vector3(clampedX, tableTransform.position.y + yOffset, clampedZ);
    }

}
