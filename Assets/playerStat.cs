using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerStat : MonoBehaviour {

    public Text playerMoney = null;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		if (this.transform.parent.GetComponent<panelControl>().game.playerOrder[System.Int32.Parse(this.tag) - 1].gameover)
        {
            playerMoney.text = "Bankrupted";
        }
        else
        {
            playerMoney.text = this.transform.parent.GetComponent<panelControl>().game.playerOrder[System.Int32.Parse(this.tag) - 1].money.ToString();
        }
    }

}
