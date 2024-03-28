using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapCollider : MonoBehaviour {
    GameObject Player;

 
    // Use this for initialization
    void Start () {
        Player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        /*
		if(Player.transform.position.x>gameObject.transform.position.x+20)
        {
            Destroy(gameObject);
        }
        */
	}
    void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Trap")
        {
            if (gameObject.GetComponent<Renderer>().material.color == other.GetComponent<Renderer>().material.color)
            {
                Score.score += 20;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(Player);
                SceneManager.LoadScene("End");
            }

        }


    }
    
}
