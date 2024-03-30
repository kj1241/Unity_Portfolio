using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove_test : MonoBehaviour
{
    // Start is called before the first frame update
    private float posY;
    int g = 0;
    void Start()
    {
        posY = this.gameObject.transform.position.y;
        int a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        posY -= Time.deltaTime * 200;

        this.gameObject.transform.position = new Vector3(0.0f, posY, 0.0f);

        if (this.gameObject.transform.position.y < -2560.0f)
        {
            this.gameObject.transform.position = new Vector3(0.0f, posY + 2560.0f * 2, 0.0f);

            posY = this.gameObject.transform.position.y;
        }
    }
}
