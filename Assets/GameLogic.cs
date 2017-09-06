using System.Collections;
using System.Collections.Generic;
using RFIBricks_Cores_Libs;
using UnityEngine;

public class GameLogic : MonoBehaviour {
	public bool projectionMode = true;

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
    //private int lastPos = 0;
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
        "7429 0000 9999 0101 0001,7429 0000 9101 0101 0001",
        "7429 0000 9999 0201 0001,7429 0000 9101 0301 0001",
    };
    short[] EnableAntenna = { 1, 2, 3, 4 };
    int boxNumX = 5, boxNumY = 5;
    string ReaderIP = "192.168.1.93";
    double ReaderPower = 32, Sensitive = -70;
    bool Flag_ToConnectTheReade = true;
    bool Flag_AllowStackWithoutConnectingBoard = true;
    public Dictionary<int, GameObject> build_Blocks = new Dictionary<int, GameObject>();  //BlockID
    public Vector2 PosMessage = new Vector2(0,0);
	public int PosInt = 100;
    private int tempCount1 = 100;
    private int tempCount2 = 100;
	Dictionary<Vector2, int> roadList = new Dictionary<Vector2, int>()
	{
		{ new Vector2(0,0), 0},
		{ new Vector2(1,0), 1},
		{ new Vector2(2,0), 2},
		{ new Vector2(3,0), 3},
		{ new Vector2(4,0), 4},
		{ new Vector2(5,0), 5},
		{ new Vector2(5,1), 6},
		{ new Vector2(5,2), 7},
		{ new Vector2(5,3), 8},
		{ new Vector2(5,4), 9},
		{ new Vector2(5,5), 10},
		{ new Vector2(4,5), 11},
		{ new Vector2(3,5), 12},
		{ new Vector2(2,5), 13},
		{ new Vector2(1,5), 14},
		{ new Vector2(0,5), 15},
		{ new Vector2(0,4), 16},
		{ new Vector2(0,3), 17},
		{ new Vector2(0,2), 18},
		{ new Vector2(0,1), 19},
	};
	Dictionary<int, int> playerMap = new Dictionary<int, int>(){
		{910102,0},{910110,1},
	};


    // Use this for initialization
    void Start () {
        //print variable from character scene
		//print(PlayerPrefs.GetInt("PlayerNum"));

        RFIB = new RFIBricks_Cores(ReaderIP, ReaderPower, Sensitive, EnableAntenna, Flag_ToConnectTheReade);
        RFIB.setSysTagBased("7429 0000");
        RFIB.setAllowBlockType(AllowBlockType);

        RFIB.startReceive();
        RFIB.setPreStackOrders(PreStackOrders);
        RFIB.startToBuild();

        //playerList = new GameObject[2];
        playerOrder = new p1control[2];
        playerOrder[0] = p1;
        playerOrder[1] = p2;

		if (projectionMode) {
			p1.gameObject.active = false;
			p2.gameObject.active = false;
		}
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
			playerOrder [curPlayer].gameover = true;
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
			} while ((!playerOrder[curPlayer].gameObject.activeSelf)&&playerOrder[curPlayer].gameover);
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
			} while ((!playerOrder[curPlayer].gameObject.activeSelf)&&playerOrder[curPlayer].gameover);           
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
			var buffer = new List<int>(build_Blocks.Keys);

			foreach (int BlockID in buffer)
            {
				string [] temp = build_Blocks[BlockID].name.Split('/');
                Destroy(build_Blocks[BlockID]);
                build_Blocks.Remove(BlockID);
                //send destroy message
				if (isPlayer(BlockID)){
					if (this.curPlayer != playerMap [BlockID])
						print("not your turn");
					else
						print("(1)leave "+playerOrder[curPlayer].cur_pos);
				}
					
                else
                {
					print("(1)remove"+int.Parse(temp[1]));
					PosInt = int.Parse(temp[1]);
                }

                if (build_Blocks.Count == 0) break;
            }


        }
		else if (build_Blocks.Count > 0)
            {  
				var buffer = new List<int>(build_Blocks.Keys);
		
				foreach (int BlockID in buffer)
                {
					string [] temp = build_Blocks[BlockID].name.Split('/');
                    if (RFIB.StackedOrders3D.ContainsKey(BlockID) == false)
                    {
                        Destroy(build_Blocks[BlockID]);
                        build_Blocks.Remove(BlockID);
                            // send destroy message
                            // if player id, save position
						if (isPlayer(BlockID)){
							if (this.curPlayer != playerMap [BlockID])
								print("not your turn");
							else
								print("(1)leave "+playerOrder[curPlayer].cur_pos);
						}
						else
                    	{                            
							print("(2)remove"+int.Parse(temp[1]));
							PosInt = int.Parse(temp[1]);
                    	}
                	}
                if (build_Blocks.Count == 0) break;
              	}            
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

			build_Blocks[blockID].name = "Block-" + blockID+"/"+playerOrder[curPlayer].cur_pos;

            //if player id, send move steps
            if (isPlayer(blockID))
            {
				//check if valid move
				if(this.curPlayer == playerMap[blockID]){

					Vector2 a = new Vector2 (X, Y);

					print("move to "+a );
					if ((roadList [a] - playerOrder [curPlayer].cur_pos) > 0)
						moveSteps = roadList [a] - playerOrder [curPlayer].cur_pos;
					else if ((roadList [a] - playerOrder [curPlayer].cur_pos) < 0)
						moveSteps = roadList [a] - playerOrder [curPlayer].cur_pos + rc.roads.Length;
					else
						print ("no move");
				}
				else
					print ("invalid player move");
            }

            //send build string to check if correct position
            //PosMessage = new Vector2(X,Y);
            else
            {
				print("build at"+playerOrder[curPlayer].cur_pos);
				PosInt = playerOrder[curPlayer].cur_pos;
            }

        }
    }

    public void keyPressed()
    {

        if (Input.GetKey("t"))
        {
            RFIB._Testing_AddTestingTemporarilyTag("7429 0000 9999 0303 0001", "7429 0000 9401 2201 0001");
        }
        if (Input.GetKey("f"))
        {
            RFIB._Testing_AddHoldingTag("7429 0000 9999 0302 0001", "7429 0000 9201 4401 0001");
        }



        if (Input.GetKeyDown("o")) RFIB.printStackedOrders3D();
        if (Input.GetKeyDown("p")) RFIB.printStackedOrders();
    }
    bool isPlayer(int id)
    {
		if (id == 910102 || id == 910110)
            return true;
        else
            return false;
    }
}
