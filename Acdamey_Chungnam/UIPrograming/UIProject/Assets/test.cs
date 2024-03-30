using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    private int _hp = 1000; 
    public UISprite _BarWidget;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _hp--; 
        _BarWidget.fillAmount = _hp * 0.001f;

    }
}
