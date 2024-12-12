using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text scoreText;       // 分数文本
    public Slider powerBar;      // 力度进度条
    private MainControl mainControl;

    private int score = 0;       // 当前分数


    void Start()
    {
        mainControl = FindObjectOfType<MainControl>();
        // 初始化分数和力度进度条
        UpdateScoreText();
        powerBar.value = 0;
    }
    void Update(){

         if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("KeyDown(KeyCode.Escape)");
            TogglePauseMenu();
        }
    }
     void TogglePauseMenu()
    {
        mainControl.TogglePause();

    }

    // 更新分数显示
    public void UpdateScore(int newScore)
    {
        score += newScore;
        UpdateScoreText();
    }

    // 更新分数文本
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "得分: " + score.ToString();
        }
    }

    // 更新力度进度条
    public void UpdatePowerBar(float newPower)
    {
        if (powerBar != null)
        {
            powerBar.value =newPower;
        }
    }
}
