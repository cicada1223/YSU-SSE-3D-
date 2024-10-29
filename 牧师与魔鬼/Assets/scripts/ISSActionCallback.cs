using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//动作事件接口，作为动作和动作管理者的接口，所有动作管理者实现这个接口以实现事件的调度
//当动作完成时，对象会调用这个接口，通知管理器对象动作已经完成，以便管理器对下一个动作进行处理
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}
