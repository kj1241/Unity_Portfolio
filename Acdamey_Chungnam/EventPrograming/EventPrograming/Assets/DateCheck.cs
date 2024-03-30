using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateCheck : MonoBehaviour
{
    System.DateTime now;
    int nowMonth;
    int nowDay;

    private AudioSource univoice;
    public AudioClip voicoBirthady;

    // Start is called before the first frame update
    void Start()
    {
        now = System.DateTime.Now;
        nowMonth = now.Month;
        nowDay = now.Day;

        univoice = GetComponent<AudioSource >();
        univoice.PlayOneShot(voicoBirthady);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
