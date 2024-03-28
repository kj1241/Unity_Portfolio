using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timelimit : MonoBehaviour {
    public Text TimeText;
    public float TimeCount=0;
    string timeStr;

    // Use this for initialization
    void Start () {
        TimeText= GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        TimeCount += Time.deltaTime;
       
        timeStr = "" + TimeCount.ToString("00.00");
        timeStr = timeStr.Replace(".", ":");
        TimeText.text =  timeStr;
    }
    
    
}
