using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对象Boat类，用于实例化船
public class Boat{
    readonly GameObject boat;
    readonly Vector3 startPos = new Vector3(-1.3F,0.1F,0);// 在起始岸边的位置
    readonly Vector3 endPos = new Vector3(1.25F,0.1F,0);// 在终点岸边的位置
    readonly Vector3 startFirstPos = new Vector3(-1.7F,0.1F,0);// 在起始岸边船的第一个座位位置
    readonly Vector3 startSecondPos = new Vector3(-1.25F,0.1F,0);// 在起始岸边船的第二个座位位置
    readonly Vector3 endFirstPos = new Vector3(1F,0.1F,0);// 在终点岸边船的第一个座位位置
    readonly Vector3 endSecondPos = new Vector3(1.45F,0.1F,0);// 在终点岸边船的第二个座位位置

    private Character[] passenger;// 乘客对象数组，存放船上的对象
    private int curPosition;//当前船在哪个岸边 0-start  1-end
    private int count;// 船上人数

    public float move_speed = 10;                                         //动作分离版本新增
	public GameObject getGameObject() { return boat; }                     //动作分离版本新增

    public Boat(){
        // 初始化对象数组，一开始船上无人，故置为空
        passenger = new Character[2];
        passenger[0] = null;
        passenger[1] = null;
        // 动态加载船的预制
        boat = Object.Instantiate (Resources.Load ("Prefabs/boat", typeof(GameObject)), startPos, Quaternion.Euler(0, 90, 0), null) as GameObject;
		boat.name = "boat";
        // 初始变量
        curPosition = 0;
        count = 0;
    }

    // 动作分离版修改
    public Vector3 boatMoveToPosition(){
        if(curPosition == 0){
            curPosition = 1;
            return endPos;
        }
        else{
            curPosition = 0;
            return startPos;
        }
    }

    public bool isEmpty(){
        return count == 0;
    }

    public bool isFull(){
        return count == 2;
    }

    public bool addPassenger(Character ch){
        if(count == 2){
            return false;
        }
        // 找到空位置
        for(int i = 0;i < 2;++i){
            if(passenger[i] == null){
                passenger[i] = ch;
                break;
            }
        }
        count++;
        return true;
    }

    public bool removePassenger(Character ch){
        if(count == 0){
            return false;
        }
        // 找到要删除的乘客
        for(int i = 0;i < 2;++i){
            if(passenger[i] != null && passenger[i].getName() == ch.getName()){
                passenger[i] = null;
            }
        }
        count--;
        return true;
    }

    public int getCurPosition(){
        return curPosition;
    }

    public Vector3 getEmptyPosition(){
        // 若船已满，则返回坐标(0,0,0)
        if(count == 2){
            return new Vector3(0,0,0);
        }
        if(curPosition == 0){
            if(passenger[0] == null){
                return startFirstPos;
            }
            else{
                return startSecondPos;
            }
        }
        else{
            if(passenger[0] == null){
                return endFirstPos;
            }
            else{
                return endSecondPos;
            }
        }
    }

    public GameObject getGameobj(){
        return boat;
    }


    public int getNumOfPriest(){
        int res = 0;
        for(int i = 0;i < 2;++i){
            if(passenger[i] != null && passenger[i].getType() == 0){
                res++;
            }
        }
        return res;
    }

    public int getNumOfDevil(){
        int res = 0;
        for(int i = 0;i < 2;++i){
            if(passenger[i] != null && passenger[i].getType() == 1){
                res++;
            }
        }
        return res;
    }

    public void reset(){
        count = 0;
        passenger[0] = null;
        passenger[1] = null;
        // 若船在终点，则回到起始岸
        if(curPosition == 1){
            boat.transform.position = startPos;
            curPosition = 0;
        }
    }
}
