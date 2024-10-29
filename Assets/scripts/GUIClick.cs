using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIClick : MonoBehaviour{
    UserAction action;
    Character bindingCharacter;// 组件当前绑定的角色对象
    SceneController sc;
    int state;// 游戏状态

    public void bindCharacter(Character ch){
        bindingCharacter = ch;
        state = (Director.getInstance ().currentSceneController as FirstController).state;
    }

    void Start(){
        action = Director.getInstance().currentSceneController as UserAction;
        sc = Director.getInstance().currentSceneController;
    }

    void OnMouseDown(){
        // 同步游戏状态
        state = (Director.getInstance ().currentSceneController as FirstController).state;
        // 只有在游戏中点击才有效
        if(state == 0){
            action.characterIsClicked(bindingCharacter);
        }
    }
}
