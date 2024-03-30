using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedUp : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject gameobject;
    public CircleCollider2D gemCollider2D;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player"))
        {
            GemCollected();
            gameobject.transform.localScale = new Vector3(1, 2, 1);
        }
    }
    void GemCollected()
     {
            gemCollider2D.enabled = false;
        gameObject.SetActive(false);


    }

  
    
}
