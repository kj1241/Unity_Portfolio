using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class coltest : MonoBehaviour
{
    public Image PowerImg;
  
    Jumsu tmpJumsu; 
    private void Start()
    {
        GameObject tmpObj;
        tmpObj = GameObject.Find("MNG");
        tmpJumsu = tmpObj.transform.GetComponent<Jumsu>();
    }
    private void Update()
    {
        transform.Translate(new Vector3(0, 0, -0.1f));
    }
    private void OnTriggerEnter(Collider other)
    {
     
       
        if(other.gameObject.tag == "Bullet")
        { 
            Destroy(other.gameObject);
            PowerImg.fillAmount -= 0.5f;
        }
        
        if(PowerImg.fillAmount ==0)
        {
            tmpJumsu.AddJumsu(10);
            Destroy(gameObject);
        }
        
    }
    
}
