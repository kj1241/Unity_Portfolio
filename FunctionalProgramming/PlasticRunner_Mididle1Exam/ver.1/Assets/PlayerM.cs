using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerM : MonoBehaviour {
    int M_C = 3;
    int I_C = 0;
    // Use this for initialization
	void Start () {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        I_C = 1;

    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(1))
        {
            if (I_C == 0)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else if (I_C == 1)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            }
            I_C = (I_C + 1) % 3;
        }
	}
}
