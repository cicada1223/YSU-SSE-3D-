using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicFlyActionManager : SSActionManager,IActionCallback,IActionManager
{
    PhysicFlyAction flyAction;
    public void PlayDisk(GameObject disk, float speed, Vector3 direcion)
    {
        flyAction=PhysicFlyAction.GetSSAction(direcion[0]);
        RunAction(disk,flyAction,this);
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objectParam = null)
    {
        Singleton<RoundController>.Instance.FreeFactoryDisk(source.gameObject);
    }


}
