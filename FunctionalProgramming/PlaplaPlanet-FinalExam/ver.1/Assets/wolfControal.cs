using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wolfControal : MonoBehaviour {

    public int initHP;
    public Image imghpbar;
    public int Hp;
    public bool Xattect;
    public GameObject item;
    static public bool Abool = true;
 
    // Use this for initialization
    void Start () {
        initHP = 4;
        Hp = 4;
        //item = GameObject.FindGameObjectWithTag("LifePot");
        if (Abool == false)
        {
            Abool = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        imghpbar.fillAmount = Hp / (float)initHP;
        if(Hp<=0)
        {
            Instantiate(item, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
        if(Abool==false)
        {
             Destroy(gameObject);
        }
     
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            StartCoroutine("wolfAtteck");

        }



    }

    IEnumerator wolfAtteck()
    {
        yield return new WaitForSeconds(2f);
   
        
        PlayerHp.Hp = PlayerHp.Hp - 10;
        Destroy(gameObject);
    }

}
