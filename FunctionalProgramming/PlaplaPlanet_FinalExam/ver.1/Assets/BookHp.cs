using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookHp : MonoBehaviour {
    public int initHP;
    public Image imghpbar;
   static public int Hp;
    // Use this for initialization
    void Start () {
        initHP = 10;
        Hp = 10;
    }
	
	// Update is called once per frame
	void Update () {
        imghpbar.fillAmount = Hp / (float)initHP;
    }
}
