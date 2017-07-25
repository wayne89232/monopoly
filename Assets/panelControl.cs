using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class panelControl : MonoBehaviour {

    public GameLogic game;
    public GameObject buildPanel = null;
    public GameObject turnPanel = null;
    public Text question = null;
    public Text timeL = null;
    public Text playerTurn = null;
    public float timeLeft = 0;
   
    bool buildStatus = false;
   

    // Use this for initialization
    void Start() {
        buildPanel.SetActive(false);
        //turnPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update() {
        playerTurn.text = "Player " + (game.curPlayer + 1) + "'s Turn";
        if (game.startTimer)
        {
            timeLeft -= Time.deltaTime;
            timeL.text = timeLeft.ToString();
            if (game.turnChange)
            {
                game.startTimer = false;
                buildPanel.SetActive(false);
            }
            else if (timeLeft < 0)
            {
                game.startTimer = false;
                buildPanel.SetActive(false);
                game.turnChange = true;
                
            }

        }


    }


    
}
