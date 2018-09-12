using UnityEngine;
using System.Collections;


/************Task Controller*************
This script is to handle the tasks transition within one scene.
Need to attach and re-assign tasks[] array in the hierarchy at every scene.

Xin Tong, Mar 2016
**************************************/

public class taskController : MonoBehaviour {


	public int maxTask = 4;//total game tasks in this level
	private int curTask = 0;


	//change the number of tasks accordingly, it is 4 for now
	public GameObject [] tasks = new GameObject[4];
	private bool[] tasksControl = new bool[4];

	public bool levelCompleted = false;

	// Use this for initialization
	void Start () {

		//set the initial task to true
		tasks [0].SetActive(true);

		for(int i =1; i<maxTask; i++){
			tasks [i].SetActive(false);

		}
	
	}
	
	// Update is called once per frame
	void Update () {

		getTaskStatus ();

		displayTask ();

		GameMaster.gameMaster.curLevelCompelet = levelCompleted;

	}

	//get the boolean value of game tasks
	void getTaskStatus(){

		for(int i=0; i< maxTask; i++){

			tasksControl [i] = tasks [i].GetComponent<ConnectDots> ().completed;

		}

	}


	//Display the enabled task and disable others
	void displayTask(){
		

		//if the last task is completed, then current level is done
		if(tasksControl[maxTask-1]){
			
			levelCompleted = true; //transit to the next new scene
		}
			

		//figure out what is the current level and enable next task when cur task completed
		for(int i = maxTask -2; i>=0; i--){
			

			if (tasksControl [i]) {
				
				if(i>=curTask){
					curTask = i;
					tasks [curTask+1].SetActive (true);
				}
					
				//Debug.Log (curTask);
				//disable other tasks
				for(int j = maxTask -1; j>=0 ; j--){

					if(j !=curTask+1){
						tasks [j].SetActive (false);
					}
				}
			}
				

		}


	}




}
