using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    public GameObject[] playerList;
    public roadController rc;
    public p1control p1;
    public p1control p2;
    public int curPlayer;
    private int playerCheck;
    public p1control[] playerOrder;
    public bool turnChange = false;
    public bool startTimer = false;

    // Use this for initialization
    void Start () {
        playerList = new GameObject[2];
        playerOrder = new p1control[2];
        playerOrder[0] = p1;
        playerOrder[1] = p2;
        curPlayer = 1;
        
        //playerCheck = curPlayer;
    }
	
	// Update is called once per frame
	void Update () {
        // turn change
        if (turnChange && !startTimer)
        {
            curPlayer = (curPlayer + 1) % playerList.Length;
            turnChange = false;
        }
    }
    IEnumerator LateCall()
    {
        //StartCoroutine(LateCall());
        yield return new WaitForSeconds(3);

        print(123);
        //Do Function here...
    }
}
