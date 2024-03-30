using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    public GameObject prefabEnemy;
    public Vector2 limitMIn;
    public Vector2 limitMax;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateEnemy());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CreateEnemy()
    {
        while(true)
        {
            float posX = Random.Range(limitMIn.x, limitMax.x);
            Vector2 createPoint = new Vector2(posX, limitMIn.y);
            Instantiate(prefabEnemy, createPoint, Quaternion.identity);
            //생성
            //기다리는 시간
            float createTime = Random.Range(1.0f,3.0f);
            yield return new WaitForSeconds(createTime);
        }    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(limitMIn, limitMax);
    }
}
