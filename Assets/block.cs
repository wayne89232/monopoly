using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour {

    //public panelControl panel = null;
    private string belong = null;
    private bool building = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if((building == true) && Input.GetKeyDown(KeyCode.B))
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<Rigidbody>();
            cube.AddComponent<cube>();
            cube.transform.position = transform.Find("build").transform.position + new Vector3(0, 0.3f, 0);
            cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            building = false;
            this.transform.parent.GetComponent<roadController>().game.turnChange = true;

        }
    }
    void OnCollisionEnter(Collision collision) {
        print(collision.gameObject.tag);
        if(this.tag == "empty")
        {
            this.transform.parent.GetComponent<roadController>().game.turnChange = true;
        }
        else if(this.tag == "goal")
        {

        }
        else
        {
            BuildDialogue(1);
        }
        

       
    }
    public void BuildDialogue(int player)
    {
        building = true;
        this.transform.parent.GetComponent<roadController>().game.startTimer = true;
        if (player != 1)
            this.transform.parent.GetComponent<roadController>().panel.question.text = "Update the house?";
        else
        {
            this.transform.parent.GetComponent<roadController>().panel.question.text = "Build the house?";
        }
        this.transform.parent.GetComponent<roadController>().panel.buildPanel.SetActive(true);
        this.transform.parent.GetComponent<roadController>().panel.timeLeft = 10.0f;
    }

}
