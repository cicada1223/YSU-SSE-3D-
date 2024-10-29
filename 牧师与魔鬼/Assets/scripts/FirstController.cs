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

	// 实现SceneController接口，检查游戏状态 0->not finish, 1->lose, 2->win
	// public void checkGameState() {	
	// 	// 船在出发点
	// 	int startNumOfPriest = startLand.getNumOfPriest();
	// 	int startNumOfDevil = startLand.getNumOfDevil();
	// 	int endNumOfPriest = endLand.getNumOfPriest();
	// 	int endNumOfDevil = endLand.getNumOfDevil();
	// 	int boatNumOfPriest = boat.getNumOfPriest();
	// 	int boatNumOfDevil = boat.getNumOfDevil();
	// 	if(endNumOfPriest + endNumOfDevil + boatNumOfPriest + boatNumOfDevil == 6){
	// 		state = 2;
	// 		return ;
	// 	}
	// 	if(boat.getCurPosition() == 1){
	// 		if((startNumOfPriest  < startNumOfDevil && startNumOfPriest > 0)||(endNumOfPriest +  boatNumOfPriest < endNumOfDevil + boatNumOfDevil && endNumOfPriest +  boatNumOfPriest > 0)){
	// 			state = 1;
	// 			return ;
	// 		}
	// 	}
	// 	else{
	// 		if((startNumOfPriest + boatNumOfPriest < startNumOfDevil + boatNumOfDevil && startNumOfPriest + boatNumOfPriest > 0) || (endNumOfPriest < endNumOfDevil && endNumOfPriest > 0)){
	// 			state = 1;
	// 			return ;
	// 		}
	// 	}
	// 	state = 0;
	// }

	// 加载游戏对象
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

	// 实现UserAction接口的goButtonIsClicked函数
	// public void goButtonIsClicked(){
	// 	if(!boat.isEmpty()){
	// 		boat.move();
	// 		checkGameState();
	// 	}
	// }

	// 实现UserAction接口的characterIsClicked函数
	// public void characterIsClicked(Character ch){
	// 	// 角色在船上
	// 	if(ch.getIsOnBoat()){
	// 		if(boat.getCurPosition() == 0){
	// 			ch.moveToPosition(startLand.getOnLand(ch));
	// 			ch.getOnLand(startLand);
	// 		}
	// 		else{
	// 			ch.moveToPosition(endLand.getOnLand(ch));
	// 			ch.getOnLand(endLand);
	// 		}
	// 		ch.getOffBoat();
	// 		boat.removePassenger(ch);
	// 	}
	// 	// 角色在岸上
	// 	else{
	// 		if(!boat.isFull()){
	// 			if(ch.getIsFinished() && boat.getCurPosition() == 1){
	// 				ch.getOnBoat(boat);
	// 				endLand.leaveLand(ch);
	// 				ch.moveToPosition(boat.getEmptyPosition());
	// 				boat.addPassenger(ch);
	// 			}
	// 			if(!ch.getIsFinished() && boat.getCurPosition() == 0){
	// 				ch.getOnBoat(boat);
	// 				startLand.leaveLand(ch);
	// 				ch.moveToPosition(boat.getEmptyPosition());
	// 				boat.addPassenger(ch);
	// 			}
	// 		}

	// 	}
	// }

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
