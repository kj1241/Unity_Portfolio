using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Rigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.back * 5f);
        Invoke("EndSceen", 5f);
    }
    void EndSceen()
    {
        Destroy(this.gameObject);
    }
}
