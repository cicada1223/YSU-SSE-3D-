using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于实例化角色对象
public class Character {
    readonly GameObject character;
    private Animator animator;

    readonly int type; //0-priest 1-devil
    readonly GUIClick _GUIClick;// 响应鼠标点击组件

    private bool isOnBoat;// 是否在船上
    private bool isFinished;//是否已经过河

    public float move_speed = 5;// 动作分离版新增

    // public void MoveToPosition() {
    //     // 开始移动，切换到行走动画
        
    // }


    // 构造函数，根据传入的字符串选择创建牧师对象或者魔鬼对象
    public Character(string sel){
        // 动态加载预制
        if(sel == "priest"){
            character = Object.Instantiate(Resources.Load("Prefabs/priest", typeof(GameObject)),Vector3.zero, Quaternion.identity, null) as GameObject;
            type = 0;
        }
        else{
            character = Object.Instantiate(Resources.Load("Prefabs/devil", typeof(GameObject)),Vector3.zero, Quaternion.identity, null) as GameObject;
            type = 1;
        }
        _GUIClick = character.AddComponent(typeof(GUIClick)) as GUIClick;
        _GUIClick.bindCharacter(this);
        // 初始化变量
        isOnBoat = false;
        isFinished = false;
    }

    public int getType(){
        return type;
    }

    public void setName(string name) {
        character.name = name;
    }

    public string getName() {
		return character.name;
	}

    public void getOnBoat(Boat boat){
        // 设置transform为船的子组件，这样当船移动的时候就可以随着船移动
        character.transform.parent = boat.getGameobj().transform;
        isOnBoat = true;
    }

    public void getOffBoat(){
        character.transform.parent = null;
        isOnBoat = false;
    }

    public void getOnLand(Land land){
        if(land.getType() == 0){
            isFinished = false;
        }
        else{
            isFinished = true;
        }
        isOnBoat = false;
    }

    public bool getIsOnBoat(){
        return isOnBoat;
    }

    public void setPosition(Vector3 pos){
        character.transform.position = pos;
        
    }

    public void setRotation(Quaternion rot){
        character.transform.rotation= rot;
    }
    



    public bool getIsFinished(){
        return isFinished;
    }

    // 重置对象
    public void reset(){
        // moveableScript.reset();
        // 如果对象已经过河
        if(isFinished){
            // 通过导演对象获取当前场景的控制器，取得其控制的Land对象
            Land endLand = (Director.getInstance ().currentSceneController as FirstController).endLand;
            endLand.leaveLand(this);
            Land startLand = (Director.getInstance ().currentSceneController as FirstController).startLand;
            getOnLand(startLand);
            setPosition(startLand.getOnLand(this));
        }
        // 如果对象在船上
        if(isOnBoat){
            getOffBoat();
            Land startLand = (Director.getInstance ().currentSceneController as FirstController).startLand;
            Boat boat = (Director.getInstance ().currentSceneController as FirstController).boat;
            boat.removePassenger(this);
            getOnLand(startLand);
            setPosition(startLand.getOnLand(this));
        }
        character.transform.parent = null;        
    }

    // 动作分离版新增
    public GameObject getGameObject(){
        return character;
    }

}
