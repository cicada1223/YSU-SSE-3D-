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
