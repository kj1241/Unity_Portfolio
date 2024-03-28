using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End : MonoBehaviour {
    public Text scoreText;
    public int socre=1;
    // Use this for initialization
    void Start () {
        
        scoreText.GetComponent<Text>().text = "스코어 : " + ScoreCounter.last.total_socre;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
