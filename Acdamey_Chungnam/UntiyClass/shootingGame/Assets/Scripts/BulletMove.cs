﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{

    private Transform tr;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector2.up * speed);
    }
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }
}
