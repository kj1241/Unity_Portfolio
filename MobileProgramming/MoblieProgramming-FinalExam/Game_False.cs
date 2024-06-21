using UnityEngine;
using System.Collections;

public class Game_False : MonoBehaviour {
    public GameObject obj;
	// Use this for initialization
	void Start () {
        obj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void onClick()
    {
        Application.LoadLevel("0.start");
    }
}
