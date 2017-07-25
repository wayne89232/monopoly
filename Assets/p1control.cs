using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1control : MonoBehaviour {
    
    CharacterController p1;
    public GameObject rc;
    public int moveSpeed = 1;
    
    int cur_pos;

    // Use this for initialization
    void Start () {
        p1 = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
    }
    public void initCharPosition()
    {
        transform.position = rc.transform.GetComponent<roadController>().roads[0].transform.position;
        cur_pos = 0;
    }
    public void movebystep(int steps)
    {
        // Vector3 direction = ((rc.transform.GetComponent<roadController>().roads[(cur_pos+steps)% rc.transform.GetComponent<roadController>().roads.Length].transform.position) - (transform.position)).normalized;
        //print(direction);
        //  p1.transform.forward = Vector3.Lerp(transform.forward, direction, 0.01f);

        // p1.SimpleMove(transform.forward * moveSpeed);
        
        MoveToPoint((cur_pos + steps) % rc.transform.GetComponent<roadController>().roads.Length,steps);
        cur_pos = (cur_pos + steps) % rc.transform.GetComponent<roadController>().roads.Length;
    }
    Vector3 ignoreY(Vector3 v3)
    {
        return new Vector3(v3.x, 0, v3.z);
    }
    void MoveToPoint(int targetPosition,int steps)
    {
        if (targetPosition == cur_pos)
            return;
        
        Transform[] points = new Transform[steps];
        for (int i = 0; i < steps; i++)
        {
            points[i] = rc.transform.GetComponent<roadController>().roads[(cur_pos+i+1)%rc.transform.GetComponent<roadController>().roads.Length].transform;
            
        }

        foreach(Transform t in points)
        {
            Vector3 moveDiff = t.position - transform.position;
            Vector3 moveDir = moveDiff.normalized * 50f * Time.deltaTime;
            if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
            {
                p1.Move(moveDir);
            }
            else
            {
                p1.Move(moveDiff);
            }
        }

    }
    public void teleport(int pos)
    {
        transform.position = rc.transform.GetComponent<roadController>().roads[pos].position;
        cur_pos = (cur_pos + 3) % rc.transform.GetComponent<roadController>().roads.Length;
    }
}
