using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//同步游戏状态，展示游戏界面、游戏结果
public class _GUI : MonoBehaviour{
    private SceneController sc;
    private UserAction ac;
    private GUIStyle style;
	GUIStyle buttonStyle;
    private int state;
    private Texture2D priest;
    private Texture2D devil;
    private Texture2D rule;

    void Start(){
        style = new GUIStyle();
		style.fontSize = 40;
        // 获得控制器，通过控制器更新model部分的数据
        sc = Director.getInstance ().currentSceneController;
        ac = Director.getInstance().currentSceneController as UserAction;
        //游戏状态
        state = (Director.getInstance ().currentSceneController as FirstController).state;

        buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 30;
    }

    void OnGUI(){
        // 同步游戏状态
        // 编写UI界面（相关按钮）
        state = (Director.getInstance ().currentSceneController as FirstController).state;
        if(GUI.Button(new Rect(Screen.width/2+150, Screen.height/2+70, 50, 30),"GO")){
            if(state == 0){
                ac.goButtonIsClicked();
            }
        }
        if(GUI.Button(new Rect(Screen.width/2+70, Screen.height/2+70, 70, 30), "Restart")){
            ac.restart();
        }
        if(state == 1){
            // GUI.Label(new Rect(800,100,300,250),devil);
            GUI.Label(new Rect(Screen.width/2, Screen.height/2, 100, 50), "You lose!",style);
        }
        if(state == 2){
            // GUI.Label(new Rect(800,100,300,250),priest);
            GUI.Label(new Rect(Screen.width/2, Screen.height/2, 100, 50), "You win!",style);
        }
        // GUI.Label(new Rect(0,800,1500,1500),rule);
    }
}
