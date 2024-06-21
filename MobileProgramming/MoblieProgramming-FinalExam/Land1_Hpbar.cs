using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Land1_Hpbar : MonoBehaviour {
   
    private int initHP;
    public Image imghpbar;
    public GameObject hp_Penal;
    private float Amount;
    int hp;
    bool is_Land1OnMouse;
    // Use this for initialization
    void Start () {
        hp = 100;
        
        initHP = 100;
        hp_Penal.SetActive(is_Land1OnMouse);
    }
	
	// Update is called once per frame
	void Update () {
       imghpbar.fillAmount = (float)hp / (float)initHP;

    }
}
