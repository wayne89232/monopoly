﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour {

    //public panelControl panel = null;
    private string belong = null;
    public bool building = false;
    public bool tolling = false;
    public bool mortgage = false;
    public bool target = false;

    public int property = 10;
    public List<GameObject> buildStack = new List<GameObject>();
	Dictionary<int, Color> colorMap = new Dictionary<int, Color>()
	{
		{ 0, Color.white},
		{ 1, Color.black},
	};


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        // add new criterion from game logic
        if ((building == true) && (Input.GetKeyDown(KeyCode.B)|| this.transform.parent.GetComponent<roadController>().game.PosInt != 100)  )
        {
            int tempPos = this.transform.parent.GetComponent<roadController>().game.PosInt;
            if (tempPos != 100)
            {
                //check build position correct
                int pos = this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].cur_pos;
				if (pos == tempPos)
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.GetComponent<Renderer>().material.color = colorMap[this.transform.parent.GetComponent<roadController>().game.curPlayer];


                    cube.AddComponent<Rigidbody>();
                    cube.AddComponent<cube>();
                    cube.transform.position = transform.Find("build").transform.position + new Vector3(0, 0.3f, 0);
                    cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

					if (this.transform.parent.GetComponent<roadController> ().game.projectionMode)
						cube.active = false;

                    if (buildStack.Count == 0)
                    {
						lightSource ();
                        this.property = this.transform.parent.GetComponent<roadController>().game.curPlayer + 1;
                        this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].properties.Add(this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].cur_pos);
                    }
                    buildStack.Add(cube);
                    this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].money -= 5000;

                    building = false;
                    this.transform.parent.GetComponent<roadController>().game.turnChange = true;
                }

				this.transform.parent.GetComponent<roadController>().game.PosInt = 100;
            }
            else
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.GetComponent<Renderer>().material.color = colorMap[this.transform.parent.GetComponent<roadController>().game.curPlayer];
                cube.AddComponent<Rigidbody>();
                cube.AddComponent<cube>();
                cube.transform.position = transform.Find("build").transform.position + new Vector3(0, 0.3f, 0);
                cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				if (this.transform.parent.GetComponent<roadController> ().game.projectionMode)
					cube.active = false;

                if (buildStack.Count == 0)
                {
					lightSource ();
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
		else if(mortgage && (Input.GetKeyDown(KeyCode.M) || this.transform.parent.GetComponent<roadController>().game.PosInt != 100) ) { 

            mortgage = false;
            roadController temp = this.transform.parent.GetComponent<roadController>();

			// TODO: fix property based on removed block position

            temp.game.playerOrder[temp.game.curPlayer].money += (5000 + temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().buildStack.Count * 1000);
            temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().property = 10;
			temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().gameObject.GetComponent<Light>().enabled = false;
            for (int j = 0; j < temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().buildStack.Count; j++)
            {
                Destroy(temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().buildStack[j]);
            }
            temp.roads[temp.game.playerOrder[temp.game.curPlayer].properties[0]].GetComponent<block>().buildStack = null;
            temp.panel.buildPanel.SetActive(false);
			this.transform.parent.GetComponent<roadController>().game.PosInt = 100;
			temp.game.turnChange = true;
        }
    }
	void lightSource(){
		this.transform.GetChild(0).gameObject.GetComponent<Light> ().enabled = true;
		if (this.transform.parent.GetComponent<roadController> ().game.curPlayer == 0)
			this.transform.GetChild (0).gameObject.GetComponent<Light> ().color = Color.red;
		else
			this.transform.GetChild (0).gameObject.GetComponent<Light> ().color = Color.blue;
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
	public void AutoCollisionTrigger(){
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
		roadController temp = this.transform.parent.GetComponent<roadController>();
        if (temp.game.playerOrder[temp.game.curPlayer].money - 5000 < 0)
        {
            tolling = true;
            temp.game.startTimer = true;
            temp.panel.question.text = "Not enough money ";
            temp.panel.buildPanel.SetActive(true);
            temp.panel.timeLeft = 3.0f;
            StartCoroutine(LateCall(3));
        }
		else if(temp.roads[temp.game.playerOrder[temp.game.curPlayer].cur_pos].GetComponent<block>().buildStack.Count>=3){
			tolling = true;
			temp.game.startTimer = true;
			temp.panel.question.text = "Build limit reached ";
			temp.panel.buildPanel.SetActive(true);
			temp.panel.timeLeft = 3.0f;
		} 
        else
        {
            building = true;
            temp.game.startTimer = true;
            temp.panel.question.text = "Build the house?";
            temp.panel.buildPanel.SetActive(true);
            temp.panel.timeLeft = 10.0f;
            StartCoroutine(LateCall(10));
        }
    }
    public void Toll() {
        
        this.transform.parent.GetComponent<roadController>().game.playerOrder[this.transform.parent.GetComponent<roadController>().game.curPlayer].money -= (this.buildStack.Count * 1000);
		this.transform.parent.GetComponent<roadController> ().game.playerOrder [this.property-1].money += (this.buildStack.Count * 1000);

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
