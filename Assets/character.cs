using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour {

    public GameObject rc;
    int cur_pos;

    bool isMoveDone;

	// Use this for initialization
	void Start () {
        isMoveDone = true;
	}
	
	// Update is called once per frame
	void Update () {
        if(isMoveDone == false) {

        }
    }
    

    public void move(int steps) {
        transform.Find("ThirdPersonController").transform.position = rc.transform.GetComponent<roadController>().roads[(cur_pos + steps) % rc.transform.GetComponent<roadController>().roads.Length].position;
        cur_pos = (cur_pos + 3) % rc.transform.GetComponent<roadController>().roads.Length;
    }
}
