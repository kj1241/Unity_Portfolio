using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager1 : MonoBehaviour {
    public AudioClip[] soundExplosion=new AudioClip[2]; //재생할 소리를 변수로 담습니다.
    AudioSource[] myAudio=new AudioSource[2]; //AudioSorce 컴포넌트를 변수로 담습니다.
    public static soundManager1 instance;  //자기자신을 변수로 담습니다.
    void Awake() //Start보다도 먼저, 객체가 생성될때 호출됩니다
    {
        if (soundManager1.instance == null) //incetance가 비어있는지 검사합니다.
        {
            soundManager1.instance = this; //자기자신을 담습니다.
        }
    }
    // Use this for initialization
    void Start () {
        myAudio[0] = this.gameObject.GetComponent<AudioSource>(); //AudioSource 오브젝트를 변수로 담습니다.
        myAudio[1] = this.gameObject.GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        myAudio[0].PlayOneShot(soundExplosion[0]); //soundExplosion을 재생합니다.
        myAudio[1].PlayOneShot(soundExplosion[1]); //soundExplosion을 재생합니다.
    }
    // Update is called once per frame
    void Update () {
        myAudio[0].PlayOneShot(soundExplosion[0]);
        myAudio[1].PlayOneShot(soundExplosion[1]);
    }
}
