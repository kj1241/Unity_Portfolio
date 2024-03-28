using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour {
    public Animator anim;
    // Use this for initialization
    void Start () {
        anim.SetTrigger("swordIdleTrigger");
        anim.SetFloat("swordWalkSelect", 0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
