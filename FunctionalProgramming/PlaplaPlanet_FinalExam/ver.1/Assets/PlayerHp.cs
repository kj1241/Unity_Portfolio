using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour {
    public float initHP;
    public Image imghpbar;
    static public float Hp;
    // Use this for initialization
    void Start () {
        initHP = 1000;
        Hp = 1000;
    }
	
	// Update is called once per frame
	void Update () {
        Hp = Hp - Time.deltaTime;
        imghpbar.fillAmount = Hp / (float)initHP;
    }
}
