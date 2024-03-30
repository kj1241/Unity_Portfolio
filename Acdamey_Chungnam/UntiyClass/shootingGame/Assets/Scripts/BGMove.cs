using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{

    private Material mat;
    private float currentYoffset = 0.0f;
    private float speed = 0.08f;


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        currentYoffset += speed * Time.deltaTime;
        mat.mainTextureOffset = new Vector2(0, currentYoffset);

    }
}
