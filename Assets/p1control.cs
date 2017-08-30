using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1control : MonoBehaviour {
    
    CharacterController p1;
	public GameLogic game;
    public GameObject rc;
    public int moveSpeed = 1;
    public int money = 0;
    public List<int> properties = new List<int>();

    public int cur_pos;

    // Use this for initialization
    void Start () {
        p1 = GetComponent<CharacterController>();
        this.money = 20000;
    }

    // Update is called once per frame
    void Update() {
    }
    public void initCharPosition()
    {
        transform.position = rc.transform.GetComponent<roadController>().roads[0].transform.position + new Vector3(Random.Range(-0.01f, 0.01f), 0, Random.Range(-0.01f, 0.01f));
        cur_pos = 0;
    }
    public void movebystep(int steps)
    {
        // Vector3 direction = ((rc.transform.GetComponent<roadController>().roads[(cur_pos+steps)% rc.transform.GetComponent<roadController>().roads.Length].transform.position) - (transform.position)).normalized;
        //print(direction);
        //  p1.transform.forward = Vector3.Lerp(transform.forward, direction, 0.01f);

        // p1.SimpleMove(transform.forward * moveSpeed);
        
        rc.transform.GetComponent<roadController>().roads[(cur_pos + steps) % rc.transform.GetComponent<roadController>().roads.Length].transform.GetComponent<block>().target = true;
        MoveToPoint((cur_pos + steps) % rc.transform.GetComponent<roadController>().roads.Length,steps);
		int temp = cur_pos;
        cur_pos = (cur_pos + steps) % rc.transform.GetComponent<roadController>().roads.Length;

		colorSource (temp);

        //this.GetComponent<Rigidbody>().isKinematic = false;
    }
	void colorSource(int temp){
		//light off
		if (rc.transform.GetComponent<roadController> ().roads [temp].gameObject.GetComponent<Light> ().color == Color.yellow) {
			if (game.curPlayer == 0)
				rc.transform.GetComponent<roadController> ().roads [temp].gameObject.GetComponent<Light> ().color = Color.blue;
			else
				rc.transform.GetComponent<roadController> ().roads [temp].gameObject.GetComponent<Light> ().color = Color.red;
		} else {
			rc.transform.GetComponent<roadController> ().roads [temp].gameObject.GetComponent<Light> ().enabled = false;
		}


		//light on
		if(game.playerOrder [0].cur_pos == game.playerOrder [1].cur_pos){
			rc.transform.GetComponent<roadController> ().roads [cur_pos].gameObject.GetComponent<Light> ().color = Color.yellow;
		}
		else if(game.curPlayer == 0){
			rc.transform.GetComponent<roadController> ().roads [cur_pos].gameObject.GetComponent<Light> ().color = Color.red;
		}
		else if(game.curPlayer == 1){
			rc.transform.GetComponent<roadController> ().roads [cur_pos].gameObject.GetComponent<Light> ().color = Color.blue;
		}
		rc.transform.GetComponent<roadController> ().roads [cur_pos].gameObject.GetComponent<Light> ().enabled = true;
	}
    Vector3 ignoreY(Vector3 v3)
    {
        return new Vector3(v3.x, 0, v3.z);
    }
    void MoveToPoint(int targetPosition,int steps)
    {
        if (targetPosition == cur_pos)
            return;

        /*Transform[] points = new Transform[steps];
        for (int i = 0; i < steps; i++)
        {
            if ((cur_pos + i + 1) % rc.transform.GetComponent<roadController>().roads.Length == 0)
                this.money += 2000;
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
        }*/
        StartCoroutine(WalkAsync(steps));

    }
    public void Teleport(int pos)
    {
        transform.position = rc.transform.GetComponent<roadController>().roads[pos].position;
        cur_pos = (cur_pos + 3) % rc.transform.GetComponent<roadController>().roads.Length;
    }
    IEnumerator WalkAsync(int steps) {
        Transform[] points = new Transform[steps];
        for (int i = 0; i < steps; i++)
        {
            if ((cur_pos + i + 1) % rc.transform.GetComponent<roadController>().roads.Length == 0)
                this.money += 2000;
            points[i] = rc.transform.GetComponent<roadController>().roads[(cur_pos + i + 1) % rc.transform.GetComponent<roadController>().roads.Length].transform;

        }

        foreach (Transform t in points)
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
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
