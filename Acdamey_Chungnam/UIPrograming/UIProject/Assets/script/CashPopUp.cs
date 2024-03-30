using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CashPopUp : MonoBehaviour
{
    public GameObject hartPopUpObject;

    public UILabel goldText;
    int CashGold = 0;

    public UILabel HartText;
    int HartCount = 10;

    public bool isHartPop = false;

    // Start is called before the first frame update
    void Start()
    {
        goldText.text = CashGold.ToString();
        hartPopUpObject.SetActive(isHartPop);
    }

    // Update is called once per frame
    void Update()
    {
     if(HartCount>30)
        {
            SceneManager.LoadScene("homeworkEnd");
        }
    }
    public void CashPopUpButtonOn()
    {
        if (!isHartPop)
        {
            isHartPop = true;
            hartPopUpObject.SetActive(isHartPop);
        }
    }
    public void CashPopUpButtonExit()
    {
        if (isHartPop)
        {
            isHartPop = false;
            hartPopUpObject.SetActive(isHartPop);
        }
    }

    public void CashGetFreeGold()
    {
        CashGold += 500;
        goldText.text = CashGold.ToString();
    }
    
    public void BuyHart1()
    {
        if(CashGold>=500)
        {
            CashGold -= 500;
            HartCount += 5;
            goldText.text = CashGold.ToString();
            HartText.text = HartCount.ToString();
        }
    }
}
