using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombifEnemy : MonoBehaviour {


    public GameObject target; // Public 변수는 inspector에서 값을 지정하거나 수정할 수 있음
    NavMeshAgent agent;
    static public bool Abool = true;

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Rocket");

        agent = GetComponent<NavMeshAgent>();
        
        Animator ani = gameObject.GetComponent<Animator>();
        ani.Play("mutant walk");
        if (Abool == false)
        {
            Abool = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
        if (Abool == false)
        {
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "BookStore")
        {
            StartCoroutine("zombiAtteck");
            

        }
            
            
        
    }
    IEnumerator zombiAtteck()
    {
        yield return new WaitForSeconds(2f);
        BookHp.Hp = BookHp.Hp - 1;
        Destroy(gameObject);
    }
}
