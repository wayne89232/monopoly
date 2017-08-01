using System.Collections;
using System.Collections.Generic;
using RFIBricks_Cores_Libs;
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
    private int lastPos = 0;
    public int moveSteps = 0;
    RFIBricks_Cores RFIB;
    static string[] AllowBlockType = {  /* Color,TAG ON BOTTOM */
		"9999", /*Board*/ "9998", /*Virtual Board*/			
		//70-71-72  73-74-75 76-77-78  79-80-81 81-82-83  84-85-86 87-88-89   
		//90-91  92-93 94-95 96-97   98-99
		"9101", /*Black Block */ "9201", /*white*/ "9301", /*Gray*/ "9401", /*Red*/		
		"9601", //Tangle ID: 20-Black,30-white,40-Gray,50-Red
		"9701",
    };
    static string[] PreStackOrders = {
        "7428 0000 9999 0101 0001,7428 0000 9101 0101 0001",
        "7428 0000 9999 0201 0001,7428 0000 9101 0201 0001",
    };
    short[] EnableAntenna = { 1, 2, 3, 4 };
    int boxNumX = 5, boxNumY = 5;
    string ReaderIP = "192.168.1.93";
    double ReaderPower = 30, Sensitive = -70;
    bool Flag_ToConnectTheReade = true;
    bool Flag_AllowStackWithoutConnectingBoard = true;
    public Dictionary<int, GameObject> build_Blocks = new Dictionary<int, GameObject>();  //BlockID
    public Vector2 PosMessage = new Vector2(0,0);
    private int tempCount1 = 100;
    private int tempCount2 = 100;


    // Use this for initialization
    void Start () {
        print(PlayerPrefs.GetInt("PlayerNum"));

        RFIB = new RFIBricks_Cores(ReaderIP, ReaderPower, Sensitive, EnableAntenna, Flag_ToConnectTheReade);
        RFIB.setSysTagBased("7428 0000");
        RFIB.setAllowBlockType(AllowBlockType);

        RFIB.startReceive();
        RFIB.setPreStackOrders(PreStackOrders);
        RFIB.startToBuild();

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
        keyPressed();
        RFIB.statesUpdate();
        CheckBlockStatus();

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
    private void CheckBlockStatus()
    {
        if (tempCount1 != RFIB.StackedOrders3D.Count)
        {
            foreach (int tmpID in RFIB.StackedOrders3D.Keys)
            {
                AddBuildBlocks(tmpID, RFIB.StackedOrders3D[tmpID][5], RFIB.StackedOrders3D[tmpID][0], RFIB.StackedOrders3D[tmpID][1], RFIB.StackedOrders3D[tmpID][2]);
            }
            tempCount1 = RFIB.StackedOrders3D.Count;
        }

        if (RFIB.StackedOrders3D.Count == 0 && build_Blocks.Count > 0)
        {
            foreach (int BlockID in build_Blocks.Keys)
            {
                Destroy(build_Blocks[BlockID]);
                build_Blocks.Remove(BlockID);
                //send destroy message
                //if player id, save position
                if (isPlayer(BlockID))
                    print(playerOrder[curPlayer].cur_pos);
                    //lastPos = playerOrder[curPlayer].cur_pos;
                else
                {
                    string[] temp = build_Blocks[BlockID].name.Split('/');
                    print(new Vector2(int.Parse(temp[1]), int.Parse(temp[2])));
                    //PosMessage = new Vector2(int.Parse(temp[1]), int.Parse(temp[2]));
                }

                if (build_Blocks.Count == 0) break;
            }


        }

        if (tempCount2 != (build_Blocks.Count))
        {
            if (build_Blocks.Count > 0)
            {
                try
                {
                    foreach (int BlockID in build_Blocks.Keys)
                    {
                        if (RFIB.StackedOrders3D.ContainsKey(BlockID) == false)
                        {
                            Destroy(build_Blocks[BlockID]);
                            build_Blocks.Remove(BlockID);
                            // send destroy message
                            // if player id, save position
                            if (isPlayer(BlockID))
                                print(playerOrder[curPlayer].cur_pos);
                                //lastPos = playerOrder[curPlayer].cur_pos;
                            else
                            {
                                string [] temp = build_Blocks[BlockID].name.Split('/');
                                print(new Vector2(int.Parse(temp[1]), int.Parse(temp[2])));
                                //PosMessage = new Vector2(int.Parse(temp[1]), int.Parse(temp[2]));
                            }
                        }
                        if (build_Blocks.Count == 0) break;
                    }
                }
                catch { }
            }
            tempCount2 = build_Blocks.Count;
        }
    }

    public void AddBuildBlocks(int blockID, int BlockType, int X, int Y, int Z)
    {
        Color tmpColor = new Color(255, 0, 0);

        //========== Set Color ================
        if ((BlockType >= 9100 && BlockType < 9500) || (BlockType >= 7100 && BlockType <= 7500))
        {
            int blockIDa = BlockType - BlockType % 100 - 9000;
            if (blockIDa == 100) { tmpColor = new Color(255, 255, 255); }  //Black
            else if (blockIDa == 200) { tmpColor = new Color(0, 0, 0); }  //White
            else if (blockIDa == 300) { tmpColor = new Color(100, 100, 100); }  //Gray
            else if (blockIDa == 400) { tmpColor = new Color(255, 0, 0); }  //Red
        }
        else if (BlockType == 9601 || BlockType == 9602 || BlockType == 9603 || BlockType == 9604 || BlockType == 9701 || BlockType == 8602 || BlockType == 8603 || BlockType == 8604)
        {
            //int blockIDa = blockID % 100;
            //if (blockIDa <= 20) { stroke(255); fill(20); }  //Black
            //else if (blockIDa <= 30) { stroke(80); fill(255); }  //white
            //else if (blockIDa <= 40) { stroke(80); fill(200); }  //Gray
            //else if (blockIDa <= 50) { stroke(80); fill(255, 0, 0); }  //Red

        }

        //========== Draw Object ================
        if (build_Blocks.ContainsKey(blockID) == false)
        {
            if (BlockType == 9601)
            {
            }
            else if (BlockType == 9701 || BlockType == 8701)
            {
                //Draw_TRIANGLES_CONE();
            }
            else
                build_Blocks.Add(blockID, new GameObject());

            build_Blocks[blockID].name = "Block-" + blockID+"/"+X+"/"+Y;

            //if player id, send move steps
            // newPos (X,Y) - lastPos
            if (isPlayer(blockID))
            {
                print((5 * Y + X) - lastPos);
                //moveSteps = (5*Y+X)-lastPos;
            }

            //send build string to check if correct position
            //PosMessage = new Vector2(X,Y);
            else
            {
                string[] temp = build_Blocks[blockID].name.Split('/');
                //PosMessage = new Vector2(int.Parse(temp[1]), int.Parse(temp[2]));
                print(new Vector2(int.Parse(temp[1]), int.Parse(temp[2])));
            }

        }
    }

    public void keyPressed()
    {

        if (Input.GetKey("t"))
        {
            RFIB._Testing_AddTestingTemporarilyTag("7428 0000 9999 0303 0001", "7428 0000 9401 2201 0001");
        }
        if (Input.GetKey("f"))
        {
            RFIB._Testing_AddHoldingTag("7428 0000 9999 0302 0001", "7428 0000 9201 4401 0001");
        }



        if (Input.GetKeyDown("o")) RFIB.printStackedOrders3D();
        if (Input.GetKeyDown("p")) RFIB.printStackedOrders();
    }
    bool isPlayer(int id)
    {
        if (id == 1234)
            return true;
        else
            return false;
    }
}
