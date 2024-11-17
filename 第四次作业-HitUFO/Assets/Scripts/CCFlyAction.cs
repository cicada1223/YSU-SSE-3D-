using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyAction : SSAction
{
    public float speed_x;
    public float speed_y;

    public static CCFlyAction GetSSAction(float x,float y){
        CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction>();
        action.speed_x=x;
        action.speed_y=y;
        return action;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic=true;
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
        transform.position+=new Vector3(speed_x,speed_y,0)*Time.deltaTime*2;
    }
}
