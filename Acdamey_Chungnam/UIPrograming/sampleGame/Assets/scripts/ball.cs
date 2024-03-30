using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ball : MonoBehaviour
{
    bool startflg;
    bool currentDir;
    public Image powerImg;
    float currentPower;

    // Start is called before the first frame update
    void Start()
    {
        startflg = false;
        currentDir = true;
    }

    // Update is called once per frame
    void Update()
    {
   
        if (Input.GetMouseButtonDown(0))
        {
            startflg = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            startflg = false;
        }
        if (startflg)
        {
            if (currentPower == 0)
            {
                currentDir = false;
            }
            else if (currentPower == 1)
            {
                currentDir = true;
            }
            if (currentDir)
            {
                powerImg.fillAmount -= 0.1f;
                currentPower = powerImg.fillAmount;
            }
            else
            {
                powerImg.fillAmount += 0.1f;
                currentPower = powerImg.fillAmount;
            }
        }
        if (!startflg)
        {
            transform.Translate(0, currentPower * 1000 * Time.deltaTime, 0);
            currentPower *= 0.98f;
        }

    }
}
