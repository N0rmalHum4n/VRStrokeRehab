using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NewGameManager : MonoBehaviour
{
    public static NewGameManager Instance;

    [SerializeField] private AudioSource levelAudioSource;
    [SerializeField] private AudioClip levelUpSound;
    // chomp sound only given for fruit fall game
    [SerializeField] private AudioClip fruitChomp;
    [SerializeField] private Transform xROrigin;
    [SerializeField] private int numberOfAttemptsInLevel; 
    [SerializeField] private float levelUpThreshold;
    [SerializeField] private float levelDownThreshold;
    [SerializeField] private int levelOneCatchTarget;
    [SerializeField] private GameObject quitIcon;
    [SerializeField] private Transform leftMiddleMetacarpalTip;
    [SerializeField] private Transform rightMiddleMetacarpalTip;
    [SerializeField] private GameObject quitCanvas;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI quitText;

    public bool levelIsOne = true;
    public int level = 1;
    private int score = 0;
    [SerializeField] private float timeRemaining;
    private bool gameActive = false;
    public bool timerActive = false;
    //public CubeSpawner spawner;
    public BunnySpawner bunnySpawner;
    public FruitSpawner fruitSpawner;


    private int recentCatches = 0;
    private int recentAttempts = 0;


    void Awake()
    {
        xROrigin.position = CalibrationData.CalibratedPosition;
       Instance = this;
    }

    void Update()
    {
        if (!gameActive) return;

        while (gameActive)
        {
            ShowLevel();
            break;
        }
        if (timerActive) { 
        timeRemaining -= Time.deltaTime;
        if (timerText) timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
            if (timeRemaining <= 0)
            {
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        gameActive = true;
        timerActive = true;
        if (bunnySpawner != null)
        {
            bunnySpawner.StartSpawning();
            bunnySpawner.RequestRespawn();
        }
        if (fruitSpawner != null)
        {
            fruitSpawner.StartSpawning();
            fruitSpawner.RequestRespawn();
        }
    }

    public void AddLevel() {
        levelAudioSource.PlayOneShot(levelUpSound);
        level++;
        if (level > 1) { 
            levelIsOne = false;
            
        }
        if (level == 2)
        {
            if (fruitSpawner != null) fruitSpawner.StartTimedSpawn();
            //if (bunnySpawner != null) bunnySpawner
        }
    }

    void ShowLevel()
    {           
        if (levelText) levelText.text = "Level: " + level;
        
    }

    private void SubtractLevel()
    {
        level--;
        if (level < 1) level = 1;
        if (level == 1) {
            if (fruitSpawner != null) {
                fruitSpawner.StopTimedSpawn();
            }
            if (bunnySpawner != null) {
                bunnySpawner.StopTimedSpawn();
            }
        }
    }
    public void AddScore()
    {   

        if (!gameActive) return;
        score++;
        if (fruitChomp) levelAudioSource.PlayOneShot(fruitChomp);
        if (scoreText) scoreText.text = "Score: " + score;
        if (level == 1 && score >= levelOneCatchTarget)
        {
            AddLevel();
            return;
        }
        if (level > 1) { 
            recentAttempts++;
            recentCatches++;
            GetPerformanceRating();
        }
    }

    public void RecordMiss() {
        if (level > 1) {
            recentAttempts++;
            GetPerformanceRating();
        }
    }

    private void GetPerformanceRating() {
        if (recentAttempts < numberOfAttemptsInLevel) return;
        float successRate = (float)recentCatches / numberOfAttemptsInLevel;
        if (successRate >= levelUpThreshold) { AddLevel(); }
        if (successRate < levelDownThreshold) { SubtractLevel(); }
        recentAttempts = 0; recentCatches = 0;
    }

    void EndGame()
    {
        if (bunnySpawner != null)
        {
            bunnySpawner.StopSpawning();
        }
        var rabbits = FindObjectsOfType<RabbitBehaviour>();
        for (int i = 0; i < rabbits.Length; i++)
        {
            Destroy(rabbits[i].gameObject);
        }

        if (fruitSpawner != null)
        {
            fruitSpawner.StopSpawning();
        }
        var fruits = FindObjectsOfType<FruitBehaviour>();
        for (int i = 0; i < fruits.Length; i++)
        {
            Destroy(fruits[i].gameObject);
        }        

        gameActive = false;
                    
        if (scoreText) scoreText.text = "Final Score: " + score;
        if (timerText) timerText.text = "Game Over!";
        StartCoroutine(ShowReturnButtons());
    }

    private IEnumerator ShowReturnButtons()
    {
        yield return new WaitForSeconds(1.5f);
        quitCanvas.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Instantiate(quitIcon, leftMiddleMetacarpalTip.position + new Vector3(0f, 0.04f, 0.06f), Quaternion.Euler(0f, 0f, 45f));
        Instantiate(quitIcon, rightMiddleMetacarpalTip.position + new Vector3(0f, 0.04f, 0.06f), Quaternion.Euler(0f, 0f, 45f));
        if (quitText) quitText.text = "Press one of the X icons to exit to the Main Menu!";                                                                                                                                                                                               
    }

}

