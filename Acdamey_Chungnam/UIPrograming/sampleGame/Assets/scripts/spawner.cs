using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject EnemyObj;

    float curTime = 0f;
    float intervalTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > intervalTime)
        {
            curTime = 0; 
            GameObject tmpObj = Instantiate(EnemyObj)as GameObject;
           // float xPos = Random.Range(-5f, 5f);
            tmpObj.transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }

    }

}
