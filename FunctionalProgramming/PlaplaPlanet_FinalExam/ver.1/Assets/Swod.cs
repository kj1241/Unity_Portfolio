using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swod : MonoBehaviour {
     public GameObject item;
	void Start () {
 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        if (PlayerControl.bAttcek == true)
        {
            if (collision.gameObject.tag == "wolf")
            {
                wolfControal WolfControal = collision.gameObject.GetComponent<wolfControal>();
                WolfControal.Hp = WolfControal.Hp - 1;

            }
            if (collision.gameObject.tag == "Zombi")
            {
                Instantiate(item, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(collision.gameObject);
            }
        }
    }

}
