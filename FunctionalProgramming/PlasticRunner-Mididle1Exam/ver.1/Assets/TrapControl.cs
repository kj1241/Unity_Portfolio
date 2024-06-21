using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapControl : MonoBehaviour {

    private int Trap_Postion = 24;
    private PlayerControl player = null;
    public GameObject blockPrefabs;
   
    private Vector3 Po;

    // Use this for initialization
    void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();

    

        GameObject go = GameObject.Instantiate(this.blockPrefabs) as GameObject;
        go.transform.position = new Vector3(23, 0, 0);
        go.GetComponent<Renderer>().material.color= Color.red;
    }
	
	// Update is called once per frame
	void Update () {
        float Trip_generate_x = this.player.transform.position.x;
        if (Trip_generate_x+24> Trap_Postion)
        {
            int i = Random.Range(0, 40);
            if(i==1||i == 2 )
            {
              
                GameObject go = GameObject.Instantiate(this.blockPrefabs) as GameObject;
                go.transform.position = new Vector3(Trap_Postion, 0, 0);
                go.GetComponent<Renderer>().material.color = Color.red;
            }
            else if(i==3)
            {
                GameObject go = GameObject.Instantiate(this.blockPrefabs) as GameObject;
                go.transform.position = new Vector3(Trap_Postion, 0, 0);
                go.GetComponent<Renderer>().material.color = Color.blue;
            }
            else if (i==4)
            {
                GameObject go = GameObject.Instantiate(this.blockPrefabs) as GameObject;
                go.transform.position = new Vector3(Trap_Postion, 0, 0);
                go.GetComponent<Renderer>().material.color = Color.yellow;
            }
            Trap_Postion = Trap_Postion+1;
        }
	}
}
