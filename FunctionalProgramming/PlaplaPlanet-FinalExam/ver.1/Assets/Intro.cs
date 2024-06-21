using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
    public void LoadGame()
    {
        SceneManager.LoadScene("main");
    }


	// Use this for initialization
	void Start () {
        Invoke("LoadGame", 3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
