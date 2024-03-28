﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager2 : MonoBehaviour {
    public AudioClip soundExplosion; //재생할 소리를 변수로 담습니다.
    AudioSource myAudio ; //AudioSorce 컴포넌트를 변수로 담습니다.
    public static soundManager2 instance;  //자기자신을 변수로 담습니다.
                                           // Use this for initialization
    void Awake() //Start보다도 먼저, 객체가 생성될때 호출됩니다
    {
        if (soundManager2.instance == null) //incetance가 비어있는지 검사합니다.
        {
            soundManager2.instance = this; //자기자신을 담습니다.
        }
    }
    void Start () {
        myAudio = this.gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        myAudio.PlayOneShot(soundExplosion);
    }
}
