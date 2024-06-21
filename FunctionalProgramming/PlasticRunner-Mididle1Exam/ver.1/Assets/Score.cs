using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    static public float score;
    public Text ScoreText;
    float A;
    // Use this for initialization
    void Start () {
        ScoreText = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        score = score + Time.deltaTime * 2;
        A = Mathf.Round(score*100)/100;
        ScoreText.text = "score:" + score.ToString();
    }
}
