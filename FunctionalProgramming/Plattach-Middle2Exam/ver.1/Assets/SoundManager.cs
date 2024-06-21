using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioClip Effect1;
    public AudioClip Effect2;
    public AudioClip Effect3;
    AudioSource myAudio;

    public static SoundManager instance;

    private void Awake()
    {
        
        {
            if (SoundManager.instance == null)
                SoundManager.instance = this;
        }
    }


    // Use this for initialization
    void Start () {
        myAudio = GetComponent<AudioSource>();
	}
    public void PlayEffect1()
    {
        myAudio.PlayOneShot( Effect1);
    }

    public void PlayEffect2()
    {
        myAudio.PlayOneShot(Effect2);
    }

    public void PlayEffect3()
    {
        myAudio.PlayOneShot(Effect3);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
