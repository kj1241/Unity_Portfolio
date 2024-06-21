using UnityEngine;
using System.Collections;

public class MonsterX : MonoBehaviour {
    int monsterMoveSpeed;
    Renderer rend;
    // GameObject[] monsters;
    GameObject player;
    float dis;
    Vector3? savePos=null;
    Quaternion saveRot;

    // Use this for initialization
    void Start () {
        monsterMoveSpeed = 4; //속도 관련 
        monsterColor();
        player = GameObject.FindGameObjectWithTag("Player");
       // monsters = Resources.LoadAll("/monster", typeof(GameObject)) as GameObject[];
    }

    // Update is called once per frame
    void Update ()
    {
        dis = Vector3.Distance(transform.position, player.transform.position);
        if(dis<=5)
        {
            tracking();
        }
        else if(dis<=1)
        {
            //attck;
        }
        else
        {
            restoreMove();
            move();
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
    void restoreMove()
    {
        if(savePos != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, (Vector3)savePos, 4 * Time.deltaTime);
            if (transform.position== savePos)//도착할시
            {
                transform.rotation = saveRot;
                savePos = null;
            }
        }
       

    }
    void move()
    {
        if (savePos == null)
        {
            if (transform.position.x <= -14.5 || transform.position.x >= 14.5|| transform.position.z <= -14.5 || transform.position.z >= 14.5)
            {
                {
                    transform.Rotate(0, 180, 0);
                }
            }
            transform.Translate(monsterMoveSpeed * Time.deltaTime, 0, 0);
        }
    }
   
}


