using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScen : MonoBehaviour {
    public Text ScoreText;
    public float Finish;
    // Use this for initialization
    void Start () {
        ScoreText = GetComponent<Text>();
        Finish = Score.score;
    }
	
	// Update is called once per frame
	void Update () {
        ScoreText.text = "Score:" + Finish.ToString();
    }
}
