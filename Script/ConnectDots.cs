using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//functions:
//1. get input of correct pattern
//2. disappear and remove all lines if not correctly connected
//3. if correctly connected, return a finish state

public class ConnectDots : MonoBehaviour
{
    public List<GameObject> handCursors;
	public Material lineMaterial;
	public List<int> targetSeq;
	public List<int> targetSeq1;  //optional multiple sequences for player's choices, these need to have the same line num; null should be in the last several not in the middle
	public List<int> targetSeq2;
	public List<int> targetSeq3;


	public static List<string> handCursorNames = new List<string>();
    public static GameObject hittedDot;
	public static GameObject mainhandCursor;

	public int choice = 0;
	public bool completed = false;//Xin comment, should not be a static value if other script can access it.

	GameObject[] dots;
	List<int> connectedLineList = new List<int>();
	//List<List<int>> targetLines; 
	List<List<List<int>>> targetLinesList = new List<List<List<int>>>(); 

	List<GameObject> lineObjs = new List<GameObject>();
	int currentDot = -1; // the current nth dot

	void Start()
    {
        //create array for however many dots we have
        dots = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            dots[i] = transform.GetChild(i).gameObject;
        }
		for (int i = 0; i < handCursors.Count; i ++) {
			Rigidbody handCursorRB = handCursors [i].AddComponent<Rigidbody> ();
			handCursorRB.isKinematic = true;
			SphereCollider handCursorCollider = handCursors [i].AddComponent<SphereCollider> ();
			handCursorCollider.isTrigger = true;
			handCursorCollider.radius = 0.1f;
			handCursorNames.Add (handCursors[i].name);
		}

		if (targetSeq.Count >= 2) {
			List<List<int>> tarLines = GenerateLineList (targetSeq);
			targetLinesList.Add (tarLines);
		}
		if (targetSeq1.Count >= 2) {
			List<List<int>> tarLines = GenerateLineList (targetSeq1);
			targetLinesList.Add (tarLines);
		}
		if (targetSeq2.Count >= 2) {
			List<List<int>> tarLines = GenerateLineList (targetSeq2);
			targetLinesList.Add (tarLines);
		}
		if (targetSeq3.Count >= 2) {
			List<List<int>> tarLines = GenerateLineList (targetSeq3);
			targetLinesList.Add (tarLines);
		}

    }
	

    // Update is called once per frame
    void Update()
    {
		if (! completed) {
			Connect ();
		}
		if (currentDot >= 0 && !completed) {
			lineObjs[currentDot].GetComponent<LineRenderer>().SetPosition(1, mainhandCursor.transform.position);
		}

    }

	void Connect(){
		for (int i = 0; i < dots.Length; i++) {
			if (hittedDot == dots [i]) {
				if (connectedLineList.Count == 0 || connectedLineList[connectedLineList.Count-1] != i){ 
					connectedLineList.Add (i);
					if (currentDot>=0 && !completed){
						lineObjs[currentDot].GetComponent<LineRenderer>().SetPosition(1, dots [i].transform.position);  //auto make the line's end connect to previous touched dot
					}
					//only one target dot scenario
					if (targetSeq.Count == 1){
						if(connectedLineList[0] == targetSeq[0] ){
							completed = true;
							Complete();
						}
					}
					else{
						bool compared = Compare(connectedLineList);
						if (!compared){
							Restart();
						}
						else{
							for(int j =0; j < targetLinesList.Count; j++){
								if (targetLinesList[j].Count == 0 && targetSeq.Count == connectedLineList.Count){
									completed = true; 
									choice = j;
									Complete();
								}
							}
							if(!completed){
								AddLine(dots[i].transform.position);
							}
						}

					}
				}
			}
		}
	}

	//compare each new generated line with the target lines list, if not exist, restart, if exist and not replicate, continue
	bool Compare (List<int> connectedLineList){
		bool matched = false;
		if (connectedLineList.Count > targetSeq.Count) {
			return false;
		}
		int last = connectedLineList.Count-1; //at least 2 dots in the connected list
		foreach (List<List<int>> targetLines in targetLinesList) {
			for (int tar = 0; tar < targetLines.Count; tar++) {
				if (last < 1) {  //only one dot in connected list
					if (connectedLineList [last] == targetLines [tar] [0] || connectedLineList [last] == targetLines [tar] [1]) {
//					Debug.Log ("connectedLineList[last]: " + connectedLineList [last] + "     tar[0]: " + targetLines[tar] [0] + " tar[1]: " + targetLines[tar] [1]);
						matched = true;
					}
				} else {
					if ((connectedLineList [last] == targetLines [tar] [0] && connectedLineList [last - 1] == targetLines [tar] [1]) 
						|| (connectedLineList [last - 1] == targetLines [tar] [0] && connectedLineList [last] == targetLines [tar] [1])) {
//					Debug.Log ("connectedLineList[last]: " + connectedLineList [last] + " connectedLineList[last-1]: " + connectedLineList [last - 1] + "     tar[0]: " + targetLines[tar] [0] + " tar[1]: " + targetLines[tar] [1]);
						targetLines.RemoveAt (tar);
						matched = true;
					}
				}
			}
		}
		return matched;
	}

	void AddLine(Vector3 startPos){
		currentDot ++;
		GameObject lineObj = new GameObject();
		lineObj.transform.SetParent (this.gameObject.transform);
		LineRenderer newLine = lineObj.AddComponent<LineRenderer>();
		lineObjs.Add(lineObj);
		newLine.useWorldSpace = false;
		newLine.material = lineMaterial; //new Material(Shader.Find("Particles/Multiply"));
		newLine.useWorldSpace = false;
		newLine.SetWidth(0.2F, 0.2F);
		newLine.SetPosition(0, startPos);

	}
	
	void Restart(){
		Debug.Log ("restart");
		currentDot = -1;
		completed = false;
		targetLinesList.Clear ();
		if (targetSeq.Count >= 2) {
			List<List<int>> tarLines = GenerateLineList (targetSeq);
			targetLinesList.Add (tarLines);
		}
		if (targetSeq1.Count >= 2) {
			List<List<int>> tarLines = GenerateLineList (targetSeq1);
			targetLinesList.Add (tarLines);
		}
		if (targetSeq2.Count >= 2) {
			List<List<int>> tarLines = GenerateLineList (targetSeq2);
			targetLinesList.Add (tarLines);
		}
		if (targetSeq3.Count >= 2) {
			List<List<int>> tarLines = GenerateLineList (targetSeq3);
			targetLinesList.Add (tarLines);
		}
		for (int i = 0; i < lineObjs.Count; i++) {
			Destroy (lineObjs [i]);
		}
		lineObjs.Clear ();
		connectedLineList.Clear ();

	}

	void Complete(){
		Debug.Log ("Complete!!!");
		Debug.Log ("completed state: " + completed);
		Debug.Log("Choice is: "+choice);
	}

	//generate lines from the sequence of dots list
	List<List<int>> GenerateLineList(List<int> dotsequence){
		List<List<int>> linesList= new List<List<int>>(); 
		for (int i = 0; i < dotsequence.Count-1; i++) {
			List<int> tmpedge = new List<int>{ dotsequence [i], dotsequence [i + 1] };
			linesList.Add (tmpedge);
		}
		return linesList;
	}

}
