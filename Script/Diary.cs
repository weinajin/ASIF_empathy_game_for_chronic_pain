using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*

This is the diary scene, and this script is used acorss scenes.
For the diary, it is an imag instead of a 3D scene I guess?


Xin Tong, Mar 2016
*/



public class Diary : MonoBehaviour {


	public int diaryLevel;
	public int performance;


	// Use this for initialization
	void Start () {
		
		diaryLevel = int.Parse(GameMaster.gameMaster.curLevel);
		performance = GameMaster.gameMaster.score;

	}
	

}
