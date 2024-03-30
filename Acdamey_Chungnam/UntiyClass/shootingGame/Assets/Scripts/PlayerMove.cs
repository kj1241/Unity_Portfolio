using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public Vector2 limitPoint1;
    public Vector2 limitPoint2;

    private Transform tr;
    private Vector2 mousePosition;



    public GameObject prefabBullet;

    private IEnumerator isBool;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        isBool = FireBullet();





    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            if (mousePosition.x < limitPoint1.x)
            {
                mousePosition = new Vector2(limitPoint1.x, mousePosition.y);
                //코드량이 길어지지만 작동됨
                //mousePosition.x = limitPoint1.x;
                //mousePosition.y = mousePosition.y;

            }
            else if (mousePosition.x > limitPoint2.x)
            {
                mousePosition = new Vector2(limitPoint2.x, mousePosition.y);
                // mousePosition.x = limitPoint2.x;
                //mousePosition.y = mousePosition.y;
            }

            if (mousePosition.y < limitPoint1.y)
            {
                mousePosition = new Vector2(mousePosition.x, limitPoint1.y);
                // mousePosition.x = mousePosition.x;
                //mousePosition.y = limitPoint1.y;
            }
            else if (mousePosition.y > limitPoint2.y)
            {
                mousePosition = new Vector2(mousePosition.x, limitPoint2.y);
                //mousePosition.x = mousePosition.x;
                //mousePosition.y = limitPoint2.y;
            }







            tr.position = Vector2.MoveTowards(tr.position, mousePosition, Time.deltaTime * speed);
        }


        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(isBool);
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine(isBool);
        }


    }

    IEnumerator FireBullet()
    {
        

        while(true)
        {

            Instantiate(prefabBullet, tr.position, Quaternion.identity);

            yield return new WaitForSeconds(0.3f);
        
        }



    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(limitPoint1, new Vector2(limitPoint2.x, limitPoint1.y));
        Gizmos.DrawLine(limitPoint1, new Vector2(limitPoint1.x, limitPoint2.y));
        Gizmos.DrawLine(limitPoint2, new Vector2(limitPoint1.x, limitPoint2.y));
        Gizmos.DrawLine(limitPoint2, new Vector2(limitPoint2.x, limitPoint1.y));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("GameOverScenee");
    }
}
