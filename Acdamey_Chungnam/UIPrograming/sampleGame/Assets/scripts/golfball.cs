using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class golfball : MonoBehaviour
{
    public Image PowerImage;
    bool curDirFlg = true;              // true :  filldown      false :  fillup 
    float curPower = 0f;
    bool startFlg = true;
    void Start()
    {
        curPower = 1;
        curDirFlg = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startFlg = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            startFlg = false;
        }
        if(Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("game1");
        }
        if (startFlg)
        {
            if (curPower == 0)
            {
                curDirFlg = false;
            }
            else if (curPower == 1)
            {
                curDirFlg = true;
            }
            if (curDirFlg)
            {
                filldown();
                curPower = PowerImage.fillAmount;
            }
            else
            {
                fillup();
                curPower = PowerImage.fillAmount;
            }

        }       
        if(!startFlg)
        {
            ballStart();
        }
       // 
    }
    void filldown()
    {
        PowerImage.fillAmount -= 0.1f;
    }
    void fillup()
    {
        PowerImage.fillAmount += 0.1f;
    }
    void ballStart()
    {
        transform.Translate(new Vector3(0, curPower*1000*Time.deltaTime, 0));
        curPower *= 0.98f;
    }
}
