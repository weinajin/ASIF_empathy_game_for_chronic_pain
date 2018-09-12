using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.UI;

/*
This gameobject is a prefab that needs to be attached to each scene.
It saves the Game data (score, etc) when transit to different levels.

Xin Tong, Mar 2016
*/

[Serializable]
class GameData{

	public int score; //current score
	public string curLevel; //current game level

}



public class GameMaster : MonoBehaviour {


	public static GameMaster gameMaster;

	public int score; //current score

	public string curLevel;//current level name

	public bool curLevelCompelet;

	void Awake(){

		//detect if there is already a GameMaster Prefab in the project

		if(gameMaster == null){

			DontDestroyOnLoad (gameObject);
			gameMaster = this;

		}else{

			Destroy (gameObject);

		}

	}

	void Start(){
		
		//load the data when the scene starts
		GameMaster.gameMaster.Load ();
	
	}


	//Save the player's performance after they finish each game
	public void Save(){

		BinaryFormatter bf = new BinaryFormatter ();

		FileStream file = File.Create (Application.persistentDataPath + "/gameData.dat");

		GameData gamedata = new GameData ();

		//assign value from the script to GameData class
		gamedata.score = score;
		gamedata.curLevel = curLevel;


		//save data
		bf.Serialize (file, gamedata);
		file.Close ();



	}

	//load file very time this scene starts

	public void Load(){

		if(File.Exists(Application.persistentDataPath+"/gameData.dat")){

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/gameData.dat", FileMode.Open);

			GameData data = (GameData)bf.Deserialize (file);

			//load the saved data
			score = data.score;
			curLevel = data.curLevel;


			//close the file
			file.Close ();
		}

	}



}
