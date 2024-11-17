using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 飞碟获取规则
public struct Ruler {
    public int trialNum; // 当前trial的编号
    public int roundNum; // 当前round的编号
    public int roundSum; // 一共round的总数目
    public int [] roundDisksNum; // 每一轮对于trial的飞碟数量
    public float sendTime; // 发射间隔时间

    public int size; // 飞碟大小
    public int speed; // 飞碟速度
    public string color; // 飞碟颜色
    public Vector3 direction; // 飞碟飞入方向
    public Vector3 beginPos; // 飞碟飞入位置
};
