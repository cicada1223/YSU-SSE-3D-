using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承动作管理基类的自定义游戏管理器
//本游戏的游戏动作对象只有移动这一个动作，实际上就是对SSMoveToAction对象的管理
public class MySceneActionManager : SSActionManager  //本游戏管理器
{
    private SSMoveToAction moveBoatToEndOrStart;     //移动船到结束岸，移动船到开始岸
    private SequenceAction moveRoleToLandorBoat;     //移动角色到陆地，移动角色到船上

    public FirstController sceneController;

    protected new void Start()
    {
        sceneController = (FirstController)Director.getInstance().currentSceneController;
        sceneController.actionManager = this;
    }
    public void moveBoat(GameObject boat, Vector3 target, float speed)
    {
        moveBoatToEndOrStart = SSMoveToAction.GetSSAction(target, speed);//创建移动动作
        this.RunAction(boat, moveBoatToEndOrStart, this);//执行动作
    }

    public void moveCharacter(GameObject role, Vector3 middle_pos, Vector3 end_pos, float speed)
    {
        SSAction action1 = SSMoveToAction.GetSSAction(middle_pos, speed);//创建前半部分路径的移动动作
        SSAction action2 = SSMoveToAction.GetSSAction(end_pos, speed);//创建后半部分路径的移动动作
        moveRoleToLandorBoat = SequenceAction.GetSSAcition(1, 0, new List<SSAction> { action1, action2 });//将两个动作组合成一个动作序列
        this.RunAction(role, moveRoleToLandorBoat, this);//执行动作序列
    }
}
