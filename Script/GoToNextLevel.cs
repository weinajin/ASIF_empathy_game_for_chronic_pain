using UnityEngine;
using System.Collections;

/*
Go to next level once this level is done.
Call with level name (int/string)


Question: we can either put it as a UI button
OR as a 3D object for the hands to collide with.

Xin Tong, Mar 2016
*/

public class GoToNextLevel : MonoBehaviour {
	

	public string lvlName;

		
	void Update(){
	
		if(GameMaster.gameMaster.curLevelCompelet){
			NextLevelButton (lvlName);
		}
	
	}

	//or (int levelIndex) in the function methods
	void NextLevelButton(string levelName)
	{
		//save data
		GameMaster.gameMaster.Save ();

		Application.LoadLevel(levelName);

	}
		
}
