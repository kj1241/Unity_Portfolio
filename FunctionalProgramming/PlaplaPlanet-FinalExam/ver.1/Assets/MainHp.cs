﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHp : MonoBehaviour {
    public Transform target;
   
    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        

    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target);
    }
}
