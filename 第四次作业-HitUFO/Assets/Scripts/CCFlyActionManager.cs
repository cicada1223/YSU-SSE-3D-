using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager,IActionCallback,IActionManager
{
    CCFlyAction flyAction;

    public void PlayDisk(GameObject disk, float speed, Vector3 direcion)
    {
       flyAction=CCFlyAction.GetSSAction(direcion[0],direcion[1]);
       RunAction(disk,flyAction,this);
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objectParam = null)
    {
       Singleton<RoundController>.Instance.FreeFactoryDisk(source.gameObject);
    }

}
