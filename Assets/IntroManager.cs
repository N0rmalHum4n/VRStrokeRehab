using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public static IntroManager Instance;

    [SerializeField] private GameObject dummyPrefab;
    [SerializeField] private Transform leftMiddleMetacarpalTip;
    [SerializeField] private Transform rightMiddleMetacarpalTip;
    [SerializeField] private Transform virtualTable;
    [SerializeField] private GameObject introCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        introCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnDummy();
    }

    private void SpawnDummy()
    {
        float avgX = (leftMiddleMetacarpalTip.position.x + rightMiddleMetacarpalTip.position.x) / 2f;
        float avgY = (leftMiddleMetacarpalTip.position.y + rightMiddleMetacarpalTip.position.y) / 2f + yOffset;
        float avgZ = (leftMiddleMetacarpalTip.position.z + rightMiddleMetacarpalTip.position.z) / 2f + zOffset;
        Instantiate(dummyPrefab, new Vector3(avgX, avgY + yOffset, avgZ), Quaternion.identity);
    }

    public void OnDummyHit()
    {
        introCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        NewGameManager.Instance.StartGame();
    }
}