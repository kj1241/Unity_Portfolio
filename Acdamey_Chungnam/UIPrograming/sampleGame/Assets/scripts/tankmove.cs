using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankmove : MonoBehaviour
{
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {
        float mH = Input.GetAxis("Horizontal");
        //Debug.Log(" mh = " + mH);
        cc.Move(new Vector3(mH*0.1f, 0, 0));
    }
}
