using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land{
    private GameObject land;
    readonly Vector3 startPos = new Vector3(-6.6F,0,0);// 起始岸的位置
	readonly Vector3 endPos = new Vector3(3.5F,0,0);// 终点岸的位置
    
    private int type;//河岸类型 0-start 1-end
    private int count;// 岸上人数
    private Character[] curCharacter;// 当前岸上的角色
    readonly Vector3[] place;// 岸上的六个位置坐标 角色数组下标和位置数组下标保持一致
    private int[] emptyPlace;// 岸上六个位置的情况 0-empty 1-not empty

    public Land(string sel){
        place = new Vector3[6];
        if(sel == "start"){
            land = Object.Instantiate(Resources.Load("Prefabs/land", typeof(GameObject)),startPos, Quaternion.identity, null) as GameObject;
            land.name = "start";
            type = 0;
            count = 0;
            for(int i = 0;i < 6;++i){
                place[i] = new Vector3(-3.5F-0.8F*i,0.5F,0);
            }
        }
        else{
            land = Object.Instantiate(Resources.Load("Prefabs/land", typeof(GameObject)),endPos, Quaternion.identity, null) as GameObject;
            land.name = "end";
            type = 1;
            count = 0;
            for(int i = 0;i < 6;++i){
                place[i] = new Vector3(3.5F+0.8F*i,0.5F,0);
            }
        }
        curCharacter = new Character[6];
        emptyPlace = new int[6];
        for(int i = 0;i < 6;++i){
            curCharacter[i] = null;
        }
        for(int i = 0;i < 6;++i){
            emptyPlace[i] = 0;
        }
    }

    public int getType(){
        return type;
    }

    public int getCount(){
        return count;
    }

    public Vector3 getOnLand(Character ch){
        int index = 0;
        // 找到空位置，将角色放入对应下标的角色数组中，方便后面查找删除
        while(true){
            if(emptyPlace[index] == 0){
                emptyPlace[index] = 1;
                curCharacter[index] = ch;
                count++;
                break;
            }
            else{
                index++;
            }
        }
        // 返回角色所在位置
        return place[index];
    }

    public void leaveLand(Character ch){
        for(int i = 0;i < 6;++i){
            if( curCharacter[i] != null && curCharacter[i].getName() == ch.getName()){
                curCharacter[i] = null;
                emptyPlace[i] = 0;
                count--;
                return;
            }
        }  
    }

    public Vector3 getEmptyPosition(){
        for(int i = 0;i < 6;++i){
            if(emptyPlace[i] == 0){
                return place[i];
            }
        }
        return new Vector3(0,0,0);
    }

    public int getNumOfPriest(){
        int res = 0;
        for(int i = 0;i < 6;++i){
            if(curCharacter[i] == null){
                continue;
            }
            if(curCharacter[i].getType() == 0){
                res++;
            }
        }
        return res;
    }

    public int getNumOfDevil(){
        int res = 0;
        for(int i = 0;i < 6;++i){
            if(curCharacter[i] == null){
                continue;
            }
            if(curCharacter[i].getType() == 1){
                res++;
            }
        }
        return res;
    }

    public void reset(){
        if(type == 0){
            for(int i = 0;i < 6;++i){
                emptyPlace[i] = 1;
            }
            count = 6;
        }
        else{
            for(int i = 0;i < 6;++i){
                emptyPlace[i] = 0;
            }
            count = 0;
        }
    }
}
