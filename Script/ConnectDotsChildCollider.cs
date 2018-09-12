using UnityEngine;
using System.Collections;

public class ConnectDotsChildCollider : MonoBehaviour
{
	float rand ;

	void Awake(){
		rand = Random.Range(0, 5);
	}

    void Update()
    {
        float positionNum = transform.position.x + transform.position.y;
		float radius = (Mathf.Cos( (Time.time - positionNum -rand) )/4 + 1f );
		Vector3 newS = new Vector3( radius ,radius , radius);
        transform.localScale = newS;
    }


    void OnTriggerEnter(Collider other)
    {
		if (ConnectDots.mainhandCursor == null) {  //only one hand cursor is allowed to use in completing a task
			for (int i = 0; i < ConnectDots.handCursorNames.Count; i++) {
				if (other.gameObject.name == ConnectDots.handCursorNames[i]) {
					ConnectDots.hittedDot = this.gameObject;
					ConnectDots.mainhandCursor = GameObject.Find (ConnectDots.handCursorNames[i]);
				}
			}
		} else {
			if (other.gameObject.name == ConnectDots.mainhandCursor.name) {
				ConnectDots.hittedDot = this.gameObject;
			}
		}
    }

}
