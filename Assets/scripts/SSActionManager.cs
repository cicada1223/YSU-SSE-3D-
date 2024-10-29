using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//动作管理器基类，实现对SequnceAction和SSAction对象的管理
//为动作类传递游戏对象，决定动作执行的顺序，切换动作等
public class SSActionManager : MonoBehaviour, ISSActionCallback
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>(); 
    //将执行的动作的字典集合,int为key，SSAction为value
    //等待执行的动作列表
    private List<SSAction> waitingAdd = new List<SSAction>();
    //等待删除的动作的key
    private List<int> waitingDelete = new List<int>();
    //不断更新待处理的动作
    protected void Update()
        {
            //将等待执行的动作加入到将执行的动作的字典集合
            foreach (SSAction ac in waitingAdd)
            {
                //获取动作实例的ID作为key
                actions[ac.GetInstanceID()] = ac;                                     
            }
            waitingAdd.Clear();
            //处理将执行的动作字典集合，确定动作执行的状态
            foreach (KeyValuePair<int, SSAction> kv in actions)
            {
                SSAction ac = kv.Value;
                if (ac.destroy)
                {
                    waitingDelete.Add(ac.GetInstanceID());
                }
                else if (ac.enable)
                {
                    ac.Update();
                }
            }

            //删除动作
            foreach (int key in waitingDelete)
            {
                SSAction ac = actions[key];
                actions.Remove(key);
                DestroyObject(ac);
            }
            waitingDelete.Clear();
        }
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        //牧师与魔鬼的游戏对象移动完成后就没有下一个要做的动作了，所以回调函数为空
    }
}
