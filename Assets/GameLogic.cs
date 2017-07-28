using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    //public GameObject[] playerList;
    public roadController rc;
    public p1control p1;
    public p1control p2;
    public int curPlayer;
    private int playerCheck;
    public p1control[] playerOrder;
    public bool turnChange = false;
    public bool startTimer = false;
    public bool playerOut = false;

    // Use this for initialization
    void Start () {
        print(PlayerPrefs.GetInt("PlayerNum"));
        //playerList = new GameObject[2];
        playerOrder = new p1control[2];
        playerOrder[0] = p1;
        playerOrder[1] = p2;
        curPlayer = 0;
        
        //playerCheck = curPlayer;
    }
	
	// Update is called once per frame
	void Update () {
        // turn change
        if (turnChange && !startTimer && playerOut)
        {
            playerOrder[curPlayer].gameObject.SetActive(false);
            playerOut = false;
            for (int i = 0; i < playerOrder[curPlayer].properties.Count; i++)
            {
                rc.roads[playerOrder[curPlayer].properties[i]].GetComponent<block>().property = 10;
                for(int j =0; j < rc.roads[playerOrder[curPlayer].properties[i]].GetComponent<block>().buildStack.Count; j++)
                {
                    Destroy(rc.roads[playerOrder[curPlayer].properties[i]].GetComponent<block>().buildStack[j]);
                }
                rc.roads[playerOrder[curPlayer].properties[i]].GetComponent<block>().buildStack = null;
            }


            playerOrder[curPlayer].transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerOrder[curPlayer].transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            do
            {
                curPlayer = (curPlayer + 1) % playerOrder.Length;
            } while (!playerOrder[curPlayer].gameObject.activeSelf);
            playerOrder[curPlayer].transform.gameObject.GetComponent<CapsuleCollider>().enabled = true;
            playerOrder[curPlayer].transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            turnChange = false;
        }
        else if (turnChange && !startTimer)
        {

            //print(playerOrder[curPlayer].transform.gameObject.GetComponent<Rigidbody>().constraints);
            playerOrder[curPlayer].transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerOrder[curPlayer].transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            do
            {
                curPlayer = (curPlayer + 1) % playerOrder.Length;
            } while (!playerOrder[curPlayer].gameObject.activeSelf);           
            playerOrder[curPlayer].transform.gameObject.GetComponent<CapsuleCollider>().enabled = true;
            playerOrder[curPlayer].transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            turnChange = false;
        }
    }
    IEnumerator LateCall()
    {
        //StartCoroutine(LateCall());
        yield return new WaitForSeconds(3);
        
        //Do Function here...
    }
}
