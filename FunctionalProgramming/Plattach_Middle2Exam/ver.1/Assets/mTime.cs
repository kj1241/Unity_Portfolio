using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class mTime : MonoBehaviour {
    private float initTime;
    static public float time;
    public Image imghpbar;
    public GameObject hp_penal;
    // Use this for initialization
    void Start () {
        time = 90;
        initTime= 90;

    }
   

    // Update is called once per frame
    void Update () {
        time -= Time.deltaTime;
  
        imghpbar.fillAmount = time / initTime;
    }
}
