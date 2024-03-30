using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
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
        tr.Translate(Vector2.down * speed);
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(20.0f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("GameManager").GetComponent<Score>().score += 10;

        Destroy(this.gameObject);
        Destroy(collision.gameObject);
    }
}
