using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;     // UI 文本组件：显示当前分数
    private int score = 0;     // 当前玩家分数

    // 增加分数并更新显示
    // 使用者：Customer.cs（通过 onServeSuccess / onQuickServe 事件触发）
    public void AddScore(int value)
    {
        score += value;
        UpdateScoreText();
        Debug.Log("分数增加：" + value);
    }

    // 更新 UI 显示文本
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
