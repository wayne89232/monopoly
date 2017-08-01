using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour {

    //public panelControl panel = null;
    private string belong = null;
    private bool building = false;
    private bool tolling = false;
    private bool mortgage = false;
    public bool target = false;

    public int property = 10;
    public List<GameObject> buildStack = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // add new criterion from game logic
        if ((building == true) && (Input.GetKeyDown(KeyCode.B)|| this.transform.parent.GetComponent<roadController>().game.PosMessage!= new Vector2(0,0))  )
        {
            Vector2 tempPos = this.transform.parent.GetComponent<roadController>().game.PosMessage;
            if (tempPos != new Vector2(0, 0))
            {
                //check build position correct
                int pos = this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].cur_pos;
                if (pos == tempPos.y * 5 + tempPos.x)
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.AddComponent<Rigidbody>();
                    cube.AddComponent<cube>();
                    cube.transform.position = transform.Find("build").transform.position + new Vector3(0, 0.3f, 0);
                    cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    if (buildStack.Count == 0)
                    {
                        this.property = this.transform.parent.GetComponent<roadController>().game.curPlayer + 1;
                        this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].properties.Add(this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].cur_pos);
                    }
                    buildStack.Add(cube);
                    this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].money -= 5000;

                    building = false;
                    this.transform.parent.GetComponent<roadController>().game.turnChange = true;
                }

                this.transform.parent.GetComponent<roadController>().game.PosMessage = new Vector2(0, 0);
            }
            else
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<Rigidbody>();
                cube.AddComponent<cube>();
                cube.transform.position = transform.Find("build").transform.position + new Vector3(0, 0.3f, 0);
                cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                if (buildStack.Count == 0)
                {
                    this.property = this.transform.parent.GetComponent<roadController>().game.curPlayer + 1;
                    this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].properties.Add(this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].cur_pos);
                }
                buildStack.Add(cube);
                this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].money -= 5000;

                building = false;
                this.transform.parent.GetComponent<roadController>().game.turnChange = true;
            }
            
        }
        else if ((building == true) && Input.GetKeyDown(KeyCode.N))
        {
            building = false;
            this.transform.parent.GetComponent<roadController>().game.turnChange = true;
        }
        else if ((tolling == true) && Input.GetKeyDown(KeyCode.N))
        {
            tolling = false;
            this.transform.parent.GetComponent<roadController>().game.turnChange = true;
        }
        // add new criterion from game logic
        else if(mortgage && (Input.GetKeyDown(KeyCode.Keypad1) || this.transform.parent.GetComponent<roadController>().game.PosMessage != new Vector2(0, 0)) ) { 

            mortgage = false;
            roadController temp = this.transform.parent.GetComponent<roadController>();
            temp.game.playerOrder[temp.game.curPlayer].money += (5000 + temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().buildStack.Count * 1000);
            temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().property = 10;
            for (int j = 0; j < temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().buildStack.Count; j++)
            {
                Destroy(temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().buildStack[j]);
            }
            temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().buildStack = null;
            temp.panel.buildPanel.SetActive(false);
            temp.game.turnChange = true;
        }
    }
    void OnCollisionEnter(Collision collision) {
        //print(collision.gameObject.tag);
        if (this.target == true)
        {
            if (this.tag == "empty")
            {
                this.transform.parent.GetComponent<roadController>().game.turnChange = true;
            }
            else if (((this.transform.parent.GetComponent<roadController>().game.curPlayer + 1) != this.property) && buildStack.Count != 0)
            {
                Toll();
            }
            else
            {
                BuildDialogue(1);
            }
            this.target = false;
        }
        

       
    }
    public void BuildDialogue(int player)
    {
        if (this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].money - 5000 < 0)
        {
            tolling = true;
            this.transform.parent.GetComponent<roadController>().game.startTimer = true;
            this.transform.parent.GetComponent<roadController>().panel.question.text = "Not enough money ";
            this.transform.parent.GetComponent<roadController>().panel.buildPanel.SetActive(true);
            this.transform.parent.GetComponent<roadController>().panel.timeLeft = 3.0f;
            StartCoroutine(LateCall(3));
        }
        else
        {
            building = true;
            this.transform.parent.GetComponent<roadController>().game.startTimer = true;
            this.transform.parent.GetComponent<roadController>().panel.question.text = "Build the house?";
            this.transform.parent.GetComponent<roadController>().panel.buildPanel.SetActive(true);
            this.transform.parent.GetComponent<roadController>().panel.timeLeft = 10.0f;
            StartCoroutine(LateCall(10));
        }
    }
    public void Toll() {
        
        this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].money -= (this.buildStack.Count * 1000);
        if (this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].money <= 0 && this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].properties.Count==0)
        {
            this.transform.parent.GetComponent<roadController>().game.playerOut = true;
            this.transform.parent.GetComponent<roadController>().game.startTimer = true;
            this.transform.parent.GetComponent<roadController>().panel.question.text = "Player" + (this.transform.parent.GetComponent<roadController>().game.curPlayer+1).ToString()+" bankrupted";
            this.transform.parent.GetComponent<roadController>().panel.buildPanel.SetActive(true);
            this.transform.parent.GetComponent<roadController>().panel.timeLeft = 3.0f;
        }
        else if (this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].money <= 0)
        {
            mortgage = true;
            this.transform.parent.GetComponent<roadController>().panel.question.text = "Mortgage from below:\n";
            for(int i = 0; i < this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].properties.Count; i++)
            {
                this.transform.parent.GetComponent<roadController>().panel.question.text += this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].properties[i].ToString() + " ";
            }
            this.transform.parent.GetComponent<roadController>().panel.buildPanel.SetActive(true);
        }
        else
        {
            tolling = true;
            this.transform.parent.GetComponent<roadController>().game.startTimer = true;
            this.transform.parent.GetComponent<roadController>().panel.question.text = "Pay " + (this.buildStack.Count * 1000).ToString();
            this.transform.parent.GetComponent<roadController>().panel.buildPanel.SetActive(true);
            this.transform.parent.GetComponent<roadController>().panel.timeLeft = 3.0f;
            StartCoroutine(LateCall(3));
        }
    }

    IEnumerator LateCall(int sec)
    {
        //StartCoroutine(LateCall());
        yield return new WaitForSeconds(sec);
        building = false;
        tolling = false;
    }
}
