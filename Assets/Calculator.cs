using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;

/*
ECS or MVC conception is applied on the small game
Firstly, entitis and their states are defined as the game model.
And then, give some components/controls which can modify the game model.
The end, system handler OnGUI called by game circle provides a game UI view.
*/
public class Calculator : MonoBehaviour {

    // Entities and their states / Model
 
    private char[,] Board ;
    private string s;

    private long res;
    // System Handlers
    void Start () {
        Init();
    }

    // View render entities / models
    // Here! you cannot modify model directly, use components/controls to do it
    void OnGUI() {
        GUI.Box(new Rect(255, 20, 280, 350), "");
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.box);
        centeredStyle.alignment = TextAnchor.MiddleCenter; // 设置文本居中
        GUI.Box(new Rect(255,20,280,70),s,centeredStyle);
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if(i==3&&j==1&&GUI.Button(new Rect(255 + j * 70, 90 + i * 70, 70, 70), Board[i,j].ToString())){
                        s=' '.ToString();
                        continue;
                    };
                    if(i==3&&j==2&&GUI.Button(new Rect(255 + j * 70, 90 + i * 70, 70, 70), Board[i,j].ToString())){
                        CalculateResult();
                        continue;
                    };
                    if(GUI.Button(new Rect(255 + j * 70, 90 + i * 70, 70, 70), Board[i,j].ToString())){
                        s+=Board[i,j].ToString();
                    };
                }
            }

    }

    // Components /controls
    void Init() {
        s="";
    Board = new char[,] {
        {'1', '2', '3', '+'},
        {'4', '5', '6', '-'},
        {'7', '8', '9', '*'},
        {'0', 'C', '=', '/'}
    };
}

     void CalculateResult() {
        try {
        var result = new DataTable().Compute(s, null);
        s=result.ToString();
        } catch (System.Exception e) {
            // 如果出现异常（例如输入不合法的表达式），显示错误信息
            s = "Error: " + e.Message;
        }
    }


}