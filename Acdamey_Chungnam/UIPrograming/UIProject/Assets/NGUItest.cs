using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUItest : MonoBehaviour
{
    float rotationSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            rotationSpeed = Random.Range(30, 50);

        }
        Debug.Log(rotationSpeed);
        transform.Rotate(0, 0, rotationSpeed);

        rotationSpeed *= 0.98f;
    }
}
