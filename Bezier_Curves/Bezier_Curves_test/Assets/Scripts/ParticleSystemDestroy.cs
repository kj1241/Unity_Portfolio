using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemDestroy : MonoBehaviour
{
    ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = this.gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particle.isPlaying)
            Destroy(this.gameObject);
    }
}
