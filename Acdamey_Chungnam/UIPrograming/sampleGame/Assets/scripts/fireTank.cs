using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class fireTank : MonoBehaviour
{
    public Rigidbody BulletObj;
    public Transform BulletPos;


    public Image button2;
    public Text button2Text;
    public bool isButton2Cooltime;
    public float button2Seconed;


    public Image hpBar;


    private void Start()
    {
        isButton2Cooltime = false;
        button2Text.gameObject.SetActive(false);
    }
    void Update()
    {
       if (isButton2Cooltime)
        {
            Debug.Log(button2Seconed);
            button2Seconed -= Time.deltaTime;

            button2Text.text = Mathf.Floor(button2Seconed).ToString();
            if(button2Seconed<=0)
            {
                button2cooltimeOn();
                button2Text.gameObject.SetActive(isButton2Cooltime);
                
            }
        }
        /*
        if(Input.GetMouseButtonDown(0))
        {
            FireObj();
        }
        */
    }
   public void FireObj()
    {
        Rigidbody bulletInst;
        bulletInst = Instantiate(BulletObj,BulletPos.position,BulletPos.rotation) as Rigidbody;
        bulletInst.AddForce(new Vector3(0,0,3000f));
    }

    public void FireObj2()
    {
        if (!isButton2Cooltime)
        {
            button2cooltimeOn();
            button2Text.gameObject.SetActive(isButton2Cooltime);
            Rigidbody bulletInst;
            bulletInst = Instantiate(BulletObj, BulletPos.position, BulletPos.rotation) as Rigidbody;
            bulletInst.AddForce(new Vector3(0, 0, 6000f));
            button2Seconed = 5.0f;
            
           
        }

    }
    void button2cooltimeOn()
    {
        isButton2Cooltime = !isButton2Cooltime;
    }

    private void OnTriggerEnter(Collider other)
    {
      


        if (other.gameObject.tag == "ETag")
        {
            Destroy(other.gameObject);
            hpBar.fillAmount -= 0.2f;
            Debug.Log(hpBar.fillAmount);
            if (hpBar.fillAmount<=0)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("2.end");
            }
        }

     

    }
}
