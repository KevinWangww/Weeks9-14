using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreText();
        Debug.Log("得分增加：" + value);
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
