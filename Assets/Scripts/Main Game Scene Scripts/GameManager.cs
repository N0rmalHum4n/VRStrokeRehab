using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    // Game Manager Singleton
    public static GameManager GM;

    // Game Variables
    [SerializeField] private int score = 0;

    public GameObject obj;

    void Awake()
    {
        //Singleton pattern
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
            return;
        }
        Destroy(gameObject);
    }

    public void IncrementScore(int amount)
    {
        //Increase the score by the amount
        //Debug.Log("Score increased by " + amount);'

        score += amount;
        if(score < 0){
            score = 0;
        }

        Vector3 pos = obj.transform.position;
        pos.y += 2.3f;
        obj.transform.position = pos;
    }

    public int GetScore(){
        return score;
    }

    
}
