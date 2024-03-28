using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerColloer : MonoBehaviour
{
    public GameObject item;
    // Use this for initialization
    public GameObject[] wolfs;
    public GameObject[] zombi;
    // Use this for initialization
    void Start()
    {
        wolfs = GameObject.FindGameObjectsWithTag("wolf");
        zombi = GameObject.FindGameObjectsWithTag("Zombi");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "BookStore")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                wolfControal.Abool = false;
                ZombifEnemy.Abool = false;
            }

        }

    }
}


