using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piverMode : MonoBehaviour {

    float count = 3.0f;


	// Use this for initialization
	void Start () {
		
	}
    void piveroff()
    {
        ScoreCounter.piver = false;
    }
    // Update is called once per frame
    void Update () {
      
        if (ScoreCounter.last.ignite > 10)
        {
           
            ScoreCounter.piver = true;
            count = 3.0f;
        }
        count -= Time.deltaTime;

        if(count <=0)
        {
            ScoreCounter.piverMoff = false;
        }

        if (ScoreCounter.piver == true && ScoreCounter.piverMoff ==false)
        {
            ScoreCounter.piverMoff = true;
            ScoreCounter.piver = false;
        }
    }
}
