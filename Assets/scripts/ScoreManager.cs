using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    // Call and add points when the order is completed.
    public void AddScore(int value)
    {
        score += value;
        UpdateScoreText();
        
    }

    // Show scores on screen
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
