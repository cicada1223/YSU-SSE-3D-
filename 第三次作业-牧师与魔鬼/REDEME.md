### 魔鬼与牧师游戏的动作分离版设计

#### 前言
​		本周作业为制作《魔鬼与牧师》的动作分离版。该项目的目标是通过新增动作管理器类，独立管理游戏对象的动作，将原本由场景控制器负责的职责分离出去。同时，还需引入裁判类，当游戏结束时，及时通知场景控制器游戏结束状态。

#### 设计思路
​		动作分离版的设计遵循面向对象和基于职责的原则。通过将场景控制器的职责限制为处理用户交互和加载游戏资源等，避免了对象的臃肿。具体功能被分配给特定的对象，减少了代码耦合，提高了系统的灵活性和可维护性。

#### 模式应用
在本项目中，采用了多种设计模式：
- **门面模式（Facade Pattern）**: 通过门面模式，将组合好的几个动作暴露给外部系统进行调用。
- **组合模式（Composite Pattern）**: 实现动作组合，使动作管理更为灵活。
- **模板方法（Template Method）**: 降低用户对动作管理过程细节的需求。

#### 关键类的设计
- **Director.cs**: 控制场景的创建、切换、销毁等。
- **FirstController.cs**: 作为场记，管理所有游戏对象之间的通讯和逻辑结构。==核心==
- **GUI.cs**: 实现用户交互界面，根据游戏状态展示游戏结局。
- **GUIClick.cs**: 处理用户鼠标操作，连接用户输入与游戏逻辑。
- **Boat.cs**: 实例化船对象，管理船相关的函数。
- **Character.cs**: 管理游戏角色的实例化和行为。
- **Land.cs**: 实例化河岸对象。
- **ISSActionCallback.cs**: 动作事件接口，用于调度动作事件。
- **SSAction.cs**: 动作基类，提供所有动作的共性。
- **MySceneActionManager.cs**: 本游戏的动作管理器，负责创建和执行动作。
- **Judge.cs**: 根据游戏规则判断游戏状态（输，赢，正在运行）并更新。

#### UML类图
在游戏的设计中，类与类之间存在不同的关系，这些关系可以通过UML类图表示。类图将展示类的结构、属性、方法及其之间的关系，包括继承、关联和依赖。

![game_uml_class_diagram](game_uml_class_diagram.png)

#### 部分代码示例

**FirstController.cs**: 作为场记，管理所有游戏对象之间的通讯和逻辑结构。（挂载在空对象上，加载所有预制件到指定位置，为需要互动的分配“GUIClick”脚本，保存各个预制件的状态）

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController, UserAction{
    readonly Vector3 water_pos = new Vector3(0,-0.5F,0);// 河流预制的位置
    
	public Land startLand;// 起始河岸
    public Land endLand;// 终点河岸
	public Boat boat;// 船
	public int state = 0;//游戏状态 0-gaming 1-lose 2-win 

	private Character[] characters;// 六个游戏角色
	_GUI userGUI;// GUI组件

	private Judge judge;// 新增的裁判类

	public MySceneActionManager actionManager;   //新增的动作管理
	
	void Awake() {
		// 将自身设置为当前场记
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		// 将GUI作为一个组件
		userGUI = gameObject.AddComponent <_GUI>() as _GUI;
		characters = new Character[6];
		// 加载游戏资源
		loadResources();
		// 将游戏动作管理器作为一个组件
		actionManager = gameObject.AddComponent<MySceneActionManager>() as MySceneActionManager; 
	}

	// 实现SceneController接口，加载游戏资源
    public void loadResources() {
		GameObject water = Instantiate (Resources.Load ("Prefabs/river", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";
		startLand = new Land ("start");
		endLand = new Land ("end");
		boat = new Boat();
		judge = new Judge(startLand, endLand, boat);// 新增裁判类的初始化
		loadCharacter ();
	}


    private void loadCharacter() {
		for (int i = 0; i < 3; i++) {
			Character ch = new Character("priest");
			ch.setName("priest" + i);
			ch.getOnLand(startLand);
			ch.setPosition (startLand.getOnLand(ch));
			ch.setRotation(Quaternion.Euler(0,90,0));
			characters[i] = ch;
		}

		for (int i = 0; i < 3; i++) {
			Character ch = new Character("devil");
			ch.setName("devil" + i);
			ch.setPosition (startLand.getOnLand(ch));
			ch.setRotation(Quaternion.Euler(0,90,0));
			ch.getOnLand(startLand);
			characters [i+3] = ch;
		}
	}

	// 动作分离版新增，获取游戏对象到达目的地的路径上的中间位置，以便实现折线移动
	public Vector3 getMiddlePosition(GameObject gameObject,Vector3 dest){
		Vector3 middle = dest;
        if (dest.y < gameObject.transform.position.y) {	// character from coast to boat
            middle.y = gameObject.transform.position.y;
        } else {								// character from boat to coast
            middle.x = gameObject.transform.position.x;
        }
		return middle;
	}

	// 动作分离版修改，修改了原有的goButtonIsClicked
	public void goButtonIsClicked(){                 
		if (!boat.isEmpty()){
			actionManager.moveBoat(boat.getGameObject(),boat.boatMoveToPosition(),boat.move_speed);   //动作分离版本改变
			state = judge.checkGameState();
		}
		
    }

	// 动作分离版修改，修改了原有的characterIsClicked
	public void characterIsClicked(Character ch){
		// 角色在船上
		if(ch.getIsOnBoat()){
			if(boat.getCurPosition() == 0){
				Vector3 end_pos = startLand.getOnLand(ch);//
				//ch.moveToPosition(startLand.getOnLand(ch));// 原来的
				//Vector3 end_pos = land.GetEmptyPosition();                                         //动作分离版本改变
            	Vector3 middle_pos = getMiddlePosition(ch.getGameObject(),end_pos);  //动作分离版本改变
            	actionManager.moveCharacter(ch.getGameObject(), middle_pos, end_pos, ch.move_speed);  //动作分离版本改变
				ch.getOnLand(startLand);
			}
			else{
				Vector3 end_pos = endLand.getOnLand(ch);
				Vector3 middle_pos = getMiddlePosition(ch.getGameObject(),end_pos);  //动作分离版本改变
				actionManager.moveCharacter(ch.getGameObject(), middle_pos, end_pos, ch.move_speed);  //动作分离版本改变
				//ch.moveToPosition(endLand.getOnLand(ch));
				ch.getOnLand(endLand);
			}
			ch.getOffBoat();
			boat.removePassenger(ch);
		}
		// 角色在岸上
		else{
			if(!boat.isFull()){
				if(ch.getIsFinished() && boat.getCurPosition() == 1){
					ch.getOnBoat(boat);
					endLand.leaveLand(ch);

					Vector3 end_pos = boat.getEmptyPosition();
					Vector3 middle_pos = getMiddlePosition(ch.getGameObject(),end_pos);
					//ch.moveToPosition(boat.getEmptyPosition());
					actionManager.moveCharacter(ch.getGameObject(), middle_pos, end_pos, ch.move_speed);  //动作分离版本改变
					boat.addPassenger(ch);
				}
				if(!ch.getIsFinished() && boat.getCurPosition() == 0){
					ch.getOnBoat(boat);
					startLand.leaveLand(ch);

					Vector3 end_pos = boat.getEmptyPosition();
					Vector3 middle_pos = getMiddlePosition(ch.getGameObject(),end_pos);
					actionManager.moveCharacter(ch.getGameObject(), middle_pos, end_pos, ch.move_speed);  //动作分离版本改变

					//ch.moveToPosition(boat.getEmptyPosition());
					boat.addPassenger(ch);
				}
			}

		}
	}

	// 重置游戏
	public void restart(){
		state = 0;
		for(int i = 0;i < 6;++i){
			characters[i].reset();
		}
		boat.reset();
		startLand.reset();
		endLand.reset();		
	}

}

```

**MySceneActionManager.cs**: 本游戏的动作管理器，负责创建和执行动作。

```c#
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

```

**Judge.cs**: 根据游戏规则判断游戏状态（输，赢，正在运行）并更新。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//根据游戏胜利/失败条件，判断游戏进行状态
public class Judge : MonoBehaviour
{
    private Land startLand;// 起始河岸
    private Land endLand;// 终点河岸
	private Boat boat;// 船

    public Judge(Land _startLand, Land _endLand, Boat _boat){
        startLand = _startLand;
        endLand = _endLand;
        boat = _boat;
    }

    public int checkGameState() {	
		// 船在出发点
		int startNumOfPriest = startLand.getNumOfPriest();
		int startNumOfDevil = startLand.getNumOfDevil();
		int endNumOfPriest = endLand.getNumOfPriest();
		int endNumOfDevil = endLand.getNumOfDevil();
		int boatNumOfPriest = boat.getNumOfPriest();
		int boatNumOfDevil = boat.getNumOfDevil();
		if(endNumOfPriest + endNumOfDevil + boatNumOfPriest + boatNumOfDevil == 6){
			return 2;
		}
		if(boat.getCurPosition() == 1){
			if((startNumOfPriest < startNumOfDevil && startNumOfPriest > 0)||(endNumOfPriest +  boatNumOfPriest < endNumOfDevil + boatNumOfDevil && endNumOfPriest +  boatNumOfPriest > 0)){
				return 1;
			}
		}
		else{
			if((startNumOfPriest + boatNumOfPriest < startNumOfDevil + boatNumOfDevil && startNumOfPriest + boatNumOfPriest > 0) || (endNumOfPriest < endNumOfDevil && endNumOfPriest > 0)){
				return 1;
			}
		}
		return 0;
	}
}

```





#### 结论

本项目通过对游戏《魔鬼与牧师》的设计与实现，展示了如何使用面向对象的设计理念和设计模式来提升代码的可维护性与可扩展性。通过分离职责，独立管理游戏动作，不仅提高了代码的复用性，也使得整个系统的逻辑更加清晰。通过合理的设计，游戏的实现更具灵活性，能够适应未来的需求变化。 



视频：[牧师与魔鬼——unity小作业_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV16jS8Y7EWL/?spm_id_from=333.999.0.0&vd_source=b5e5b58df5584430b5f5e1c8be8669a6)

代码（包括完整的asset）：https://github.com/cicada1223/YSU-SSE-3D-