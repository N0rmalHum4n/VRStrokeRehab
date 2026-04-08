using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] Transform tableTransform;
    [SerializeField] Vector2 spawnAreaSize; // the table size
    [SerializeField] float yOffset;
    [SerializeField] float tablePadding;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip spawnSound;
    // hand collider positions
    //[SerializeField] Transform leftPokeInteractor;
    //[SerializeField] Transform rightPokeInteractor;

    [SerializeField] Transform leftMiddleMetacarpalTip;
    [SerializeField] Transform rightMiddleMetacarpalTip;

    //[SerializeField] Transform leftIndexTip;
    //[SerializeField] Transform rightIndexTip;

    //[SerializeField] Transform leftMiddleMetacarpal;
    //[SerializeField] Transform rightMiddleMetacarpal;

    [SerializeField] GameObject fruitPrefab;
    [SerializeField] float spawnIntervalMin;
    [SerializeField] float spawnIntervalMax;
    [SerializeField] float offsetDistanceMin;
    [SerializeField] float offsetDistanceMax;
    //[SerializeField] float minOffsetDistanceFloor;
    //[SerializeField] float despawnTimerMin;
    //[SerializeField] float despawnTimerMax;
    //[SerializeField] float despawnDifficultyThreshold;
    [SerializeField] int maxLevel;

    [SerializeField] GameObject tableMarkerPrefab;
    [SerializeField] float fallSpeedMin;
    [SerializeField] float fallSpeedMax;

    private Vector3 handCenter;
    private float currentDifficulty;
    private bool isSpawning;
    private bool timedMode = false;

    //void Start()
    //{
    //    StartSpawning();
    //    RequestRespawn();
    //}

    IEnumerator DelayedFirstSpawn()
    {
        yield return new WaitForSeconds(2f);
        Spawn();
    }
    public void StartSpawning()
    {   
        FindHandCenter();
        isSpawning = true;
    }

    public void StartTimedSpawn() { 
        timedMode = true;
        StartCoroutine(TimedSpawn());
    }

    public void StopTimedSpawn() { 
        timedMode=false;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    //public void EventRespawn() {
    //    Spawn();
    //}

    public bool IsTimedMode() { 
        return timedMode;        
    }

    public float GetSpawnInterval()
    {
        float spawnInterval = Mathf.Lerp(spawnIntervalMax, spawnIntervalMin, currentDifficulty);
        return spawnInterval;
    }
    public void RequestRespawn()
    {
        StartCoroutine(DelayedRespawn());
    }

    IEnumerator DelayedRespawn()
    {
        float interval = GetSpawnInterval();
        yield return new WaitForSeconds(interval);
        Spawn();
    }

    IEnumerator TimedSpawn() {
        while (timedMode) {
            Spawn();
            float interval = GetSpawnInterval();
            yield return new WaitForSeconds(interval);
        }

    }

    private void Spawn()
    {
        if (isSpawning)
        {
            GetCurrentDifficulty();
            //FindHandCenter();
            StartCoroutine(MarkerAndFruit());
        }

    }

    private IEnumerator MarkerAndFruit() {
        //float interval = GetSpawnInterval();
        //yield return new WaitForSeconds(interval);
        Vector3 markerPosition = NewSpawnPosition() + new Vector3(0f, 0.75f, 0f);
        GameObject marker1 = Instantiate(tableMarkerPrefab, markerPosition, Quaternion.identity);
        audioSource.PlayOneShot(spawnSound);
        yield return new WaitForSeconds(1f);
        Debug.Log("FRUITDEBUG: about to instantiate");
        Vector3 fruitPosition = markerPosition + new Vector3(0f, 4f, 0f);
        GameObject fruit = Instantiate(fruitPrefab, fruitPosition, Quaternion.Euler(270f, 0f, 0f));
        fruit.GetComponent<FruitBehaviour>().SetMarker(marker1);
        fruit.GetComponent<FruitBehaviour>().SetFallSpeed(Mathf.Lerp(fallSpeedMin, fallSpeedMax, currentDifficulty));
        Debug.Log("FRUITDEBUG: instantiated");

    }

    public void FruitMissed()
    {
        NewGameManager.Instance.RecordMiss();
        if (!timedMode) Spawn();
    }

    private void GetCurrentDifficulty()
    {
        // calculate difficulty as a float clamped between 0 and 1
        currentDifficulty = Mathf.Clamp01((NewGameManager.Instance.level - 1f) / (maxLevel - 1f));
    }

    private void FindHandCenter()
    {
        float avgXValue = (leftMiddleMetacarpalTip.position.x + rightMiddleMetacarpalTip.position.x) / 2f;
        float avgZValue = (leftMiddleMetacarpalTip.position.z + rightMiddleMetacarpalTip.position.z) / 2f;
        float yValue = tableTransform.position.y;
        handCenter = new Vector3(avgXValue, yValue, avgZValue);
    }

    //private Vector3 GetSpawnPosition()
    //{
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

    private Vector3 NewSpawnPosition()
    {
        float outerRadius = Mathf.Lerp(offsetDistanceMin, offsetDistanceMax, currentDifficulty);
        //Transform[] fingerTips = { leftMiddleMetacarpal, rightMiddleMetacarpal };
        //Transform randomTip = fingerTips[Random.Range(0, fingerTips.Length)];
        //float baseAngle = Mathf.Atan2(randomTip.forward.z, randomTip.forward.x) * Mathf.Rad2Deg;

        float arcWidth = Mathf.Lerp(60f, 360f, currentDifficulty);
        float randomAngle = 270f + Random.Range(-arcWidth / 2f, arcWidth / 2f);
        //use hand center for fruit
        float randomX = handCenter.x + outerRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float randomZ = handCenter.z + outerRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
        float clampedX = Mathf.Clamp(randomX, tableTransform.position.x - spawnAreaSize.x / 2 + tablePadding, tableTransform.position.x + spawnAreaSize.x / 2 - tablePadding);
        float clampedZ = Mathf.Clamp(randomZ, tableTransform.position.z - spawnAreaSize.y / 2 + tablePadding, tableTransform.position.z + spawnAreaSize.y / 2 - tablePadding);
        return new Vector3(clampedX, handCenter.y, clampedZ);
    }

}

