using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerStat : MonoBehaviour {

    public Text playerMoney = null;
    private int money = 50000;
    // Use this for initialization
    void Start () {
        playerMoney.text = money.ToString() ;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void Spend(int m) {
        money += m;
        playerMoney.text = money.ToString();
    }
}
