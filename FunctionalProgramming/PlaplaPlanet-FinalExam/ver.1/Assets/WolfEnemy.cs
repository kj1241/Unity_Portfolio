using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfEnemy : MonoBehaviour {

    public GameObject target; // Public 변수는 inspector에서 값을 지정하거나 수정할 수 있음
    NavMeshAgent agent;

    // Use this for initialization
    void Start () {
        target=GameObject.FindGameObjectWithTag("Player");
        
        agent = GetComponent<NavMeshAgent>();
        
        Animator ani = gameObject.GetComponent<Animator>();
        ani.Play("Run");
    }
	
	// Update is called once per frame
	void Update () {
        agent.destination = target.transform.position;
    }
}
