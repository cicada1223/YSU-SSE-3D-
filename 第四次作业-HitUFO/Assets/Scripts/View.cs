using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour {
    private FirstController firstController;
    private int score;
    private string tip;
    private string roundStr;
    private string trialStr;
    public GUISkin gameSkin;  // 游戏控件的皮肤风格

    void Start() {
        score = 0;
        tip = "";
        roundStr = "";
        trialStr = "";
        firstController = Director.GetInstance().firstController;
    }

    public void SetTip(string tip) {
        this.tip = tip;
    }

    public void SetScore(int score) {
        this.score = score;
    }

    public void SetRoundStr(int round) {
        roundStr = "回合: " + round;
    }

    public void SetTrialStr(int trial) {
        if (trial == 0) trial = 10;
        trialStr = "Trial: " + trial;
    }

    public void Init() {
        score = 0;
        tip = "";
        roundStr = "";
        trialStr = "";
    }

    public void AddTitle() {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.normal.textColor = Color.black;
        titleStyle.fontSize = 50;

        GUI.Label(new Rect(Screen.width / 2 - 80, 20, 60, 100), "Hit UFO", titleStyle);
    }

    public void AddChooseModeButton() {
        GUI.skin = gameSkin;
        if (GUI.Button(new Rect(280, 100, 160, 80), "普通模式\n(" + firstController.GetRoundNum() + "回合)")) {
            firstController.SetRoundSum(firstController.GetRoundNum());
            firstController.Restart();
            firstController.SetGameState((int)GameState.Playing);
        }
        if (GUI.Button(new Rect(280, 210, 160, 80), "无尽模式\n")) {
            firstController.SetRoundSum(-1);
            firstController.Restart();
            firstController.SetGameState((int)GameState.Playing);
        }
    }

    public void ShowHomePage() {
        AddChooseModeButton();
    }

    public void AddActionModeButton() {
        GUI.skin = gameSkin;
        if (GUI.Button(new Rect(10, Screen.height - 100, 110, 40), "运动学模式")) {
           firstController.FreeAllFactoryDisk();
            firstController.SetPlayDiskModeToPhysis(false);
        }
        if (GUI.Button(new Rect(10, Screen.height - 50, 110, 40), "物理模式")) {
           firstController.FreeAllFactoryDisk();
            firstController.SetPlayDiskModeToPhysis(true);
        }
    }

    public void AddBackButton() {
        GUI.skin = gameSkin;
        if (GUI.Button(new Rect(10, 10, 60, 40), "Back")) {
            firstController.FreeAllFactoryDisk();
            firstController.Restart();
            firstController.SetGameState((int)GameState.Ready);
        }
    }

    public void AddGameLabel() {
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.black;
        labelStyle.fontSize = 30;

        GUI.Label(new Rect(100, 10, 100, 50), "得分: " + score, labelStyle);
        GUI.Label(new Rect(250, 120, 50, 200), tip, labelStyle);
        GUI.Label(new Rect(100, 60, 100, 50), roundStr, labelStyle);
        GUI.Label(new Rect(100, 110, 100, 50), trialStr, labelStyle);
    }

    public void AddRestartButton() {
        if (GUI.Button(new Rect(300, 150, 100, 60), "Restart")) {
            firstController.FreeAllFactoryDisk();
            firstController.Restart();
            firstController.SetGameState((int)GameState.Playing);
        }
    }

    public void ShowGamePage() {
        AddGameLabel();
        AddBackButton();
        AddActionModeButton();
        if (Input.GetButtonDown("Fire1")) {
            firstController.Hit(Input.mousePosition);
        }
    }

    public void ShowRestart() {
        ShowGamePage();
        AddRestartButton();
    }

    void OnGUI() {
        AddTitle();
        firstController.ShowPage();
    }
}


