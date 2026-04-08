using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{   

    // UI Text GameObject
    public GameObject tmp_score;

    // Text Component;
    TextMeshProUGUI tmp_score_text;

    // Start is called before the first frame update
    void Start()
    {
        tmp_score_text = tmp_score.GetComponent<TextMeshProUGUI>();
        tmp_score_text.text = "Score: " + "N/A";
    }

    // Update is called once per frame
    void Update()
    {
        tmp_score_text.text = "Score: " + GameManager.GM.GetScore();
    }
}
