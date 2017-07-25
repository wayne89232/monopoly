using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Modal : MonoBehaviour {

    // Use this for initialization
    public GameObject modalPanel;

    private static Modal modal1;

    public static Modal Instance()
    {
        if(!modal1)
        {
            modal1 = FindObjectOfType(typeof(Modal)) as Modal;
        }
        return modal1;
    }
    void ClosePanel()
    {
        modalPanel.SetActive(false);
    }
}
