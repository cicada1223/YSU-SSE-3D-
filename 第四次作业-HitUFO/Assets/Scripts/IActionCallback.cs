using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType:int{Start,Completed}
public interface IActionCallback 
{
    void SSActionEvent(SSAction source,
    SSActionEventType events=SSActionEventType.Completed,
    int intParam=0,
    string strParam=null,
    Object objectParam=null);
}
