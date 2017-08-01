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

        // add steps from gamelogic

        if (Input.GetKeyDown(KeyCode.Alpha1) || (game.moveSteps == 1)) {
            game.playerOrder[game.curPlayer].movebystep(1);
            game.moveSteps = 0;
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || (game.moveSteps == 2))
        {
            game.playerOrder[game.curPlayer].movebystep(2);
            game.moveSteps = 0;
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || (game.moveSteps == 3))
        {
            game.playerOrder[game.curPlayer].movebystep(3);
            game.moveSteps = 0;
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || (game.moveSteps == 4))
        {
            game.playerOrder[game.curPlayer].movebystep(4);
            game.moveSteps = 0;
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || (game.moveSteps == 5))
        {
            game.playerOrder[game.curPlayer].movebystep(5);
            game.moveSteps = 0;
            //p1.teleport(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || (game.moveSteps == 6))
        {
            game.playerOrder[game.curPlayer].movebystep(6);
            game.moveSteps = 0;
            //p1.teleport(3);
        }
    }
}
