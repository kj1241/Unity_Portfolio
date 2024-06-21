using UnityEngine;
using System.Collections;

public class MonsterBoss : MonoBehaviour {
    float monsterMoveSpeed;
    Renderer rend;
    // GameObject[] monsters;
    GameObject player;
    float dis;
    Vector3? savePos = null;
    Quaternion saveRot;

    // Use this for initialization
    void Start()
    {
        monsterMoveSpeed = 5f; //속도 관련 
        monsterColor();
        player = GameObject.FindGameObjectWithTag("Player");
        // monsters = Resources.LoadAll("/monster", typeof(GameObject)) as GameObject[];
    }

    // Update is called once per frame
    void Update()
    {
        dis = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(dis);
        if (dis <= 30)//너무 큼
        {
            tracking2();
        }

        else if (dis <= 10)
        {
            tracking();
        }
        else if (dis <= 1)
        {
            //attck;
        }
       else
       {
            restoreMove();
        }
        

    }


    void monsterColor()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
    }

    void tracking()
    {
        if (savePos == null)
        {
            savePos = transform.position;
            saveRot = transform.rotation;
        }
        transform.LookAt(player.transform);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, monsterMoveSpeed * Time.deltaTime);
    }
    void tracking2()
    {
        Vector3 pPos = player.transform.position;
        if (savePos == null)
        {
            savePos = transform.position;
            saveRot = transform.rotation;
        }
        transform.LookAt(player.transform);
        transform.Translate(2 * Time.deltaTime, 0, 0);

    }
    void restoreMove()
    {
        if (savePos != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, (Vector3)savePos, 4 * Time.deltaTime);
            if (transform.position == savePos)//도착할시
            {
                transform.rotation = saveRot;
                savePos = null;
            }
        }

    }
   
    

}
