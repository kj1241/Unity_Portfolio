using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    UnityEngine.AI.NavMeshAgent agent;
    Animator amim;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        amim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
        amim.SetFloat("Speed", agent.velocity.magnitude);
    }
}
