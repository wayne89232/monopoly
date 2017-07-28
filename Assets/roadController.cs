using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadController : MonoBehaviour {
    
    public GameLogic game;
    public panelControl panel;
    public Transform[] roads;


    // Use this for initialization
    void Start () {
        roads = new Transform[transform.childCount];
        int c = 0;
        foreach (Transform t in transform){
            //Debug.Log(t);
            roads[c] = t;
            c++;
        }
        game.playerOrder[0].initCharPosition();
        game.playerOrder[1].initCharPosition();
        game.playerOrder[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //game.playerOrder[1].gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            game.playerOrder[game.curPlayer].movebystep(1);
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            game.playerOrder[game.curPlayer].movebystep(2);
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            game.playerOrder[game.curPlayer].movebystep(3);
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            game.playerOrder[game.curPlayer].movebystep(4);
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            game.playerOrder[game.curPlayer].movebystep(5);
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            game.playerOrder[game.curPlayer].movebystep(6);
            //p1.teleport(3);
        }
    }
}
