﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class selectCharacter : MonoBehaviour {
    public int curPlayer;
    public List<int> playerSelect = new List<int>();
    public Text title = null;
    public GameObject[] list;
    // Use this for initialization
	void Start () {
        curPlayer = 1;
        list = new GameObject[4];
        list[0] = transform.Find("p1").gameObject;
        list[1] = transform.Find("p2").gameObject;
        list[2] = transform.Find("p3").gameObject;
        list[3] = transform.Find("p4").gameObject;

    }
	
	// Update is called once per frame
	void Update () {
        title.text = "Player"+curPlayer.ToString()+" choose character";
        GameObject go;
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerSelect.Add(1);
            go = (GameObject)Instantiate(Resources.Load("EthanGreen 1"));
            go.transform.position = list[curPlayer-1].transform.position;
            curPlayer++;
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            
            for(int i =0; i < playerSelect.Count; i++)
            {
                PlayerPrefs.SetInt("Player" + (i+1).ToString(), playerSelect[i]);
            }
            PlayerPrefs.SetInt("PlayerNum", curPlayer-1);
            SceneManager.LoadScene("main");

        }

        //this.transform.Find("title").transform.Find("Panel").transform.Find("title").text = ;
    }
}
