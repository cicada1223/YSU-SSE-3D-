using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicFlyAction : SSAction
{
    public float speed_x;

    public static PhysicFlyAction GetSSAction(float x){
        PhysicFlyAction action =ScriptableObject.CreateInstance<PhysicFlyAction>();
        action.speed_x=x;
        return action;
    }



    public override void Start(){
        gameObject.GetComponent<Rigidbody>().isKinematic=false;
        gameObject.GetComponent<Rigidbody>().velocity=new Vector3(speed_x*10,0,0);
        gameObject.GetComponent<Rigidbody>().drag=1;
    }


    // Update is called once per frame
    public override void Update()
    {
        if(this.transform.gameObject.activeSelf==false){
            this.destroy=true;
            this.callback.SSActionEvent(this);
            return;
        }

        Vector3 vec3=Camera.main.WorldToScreenPoint(this.transform.position);
        if(vec3.x<-100||vec3.x>Camera.main.pixelWidth+100||vec3.y<-100||vec3.y>Camera.main.pixelHeight+100){
            this.destroy=true;
            this.callback.SSActionEvent(this);
            return;
        }


    }
}
