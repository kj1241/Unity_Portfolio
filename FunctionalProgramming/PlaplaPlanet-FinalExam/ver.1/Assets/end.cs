using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class end : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerHp.Hp<=0)
        {//패배
            SceneManager.LoadScene("end");
        }
        if(PlayerControl.temp>=5)
        {//승리
            SceneManager.LoadScene("end");
        }
        if(BookHp.Hp<=0)
        {//패비
            SceneManager.LoadScene("end");
        }
        if(Input.GetKeyDown(KeyCode.P))
        {//넘어가기
            SceneManager.LoadScene("end");
        }

	}
}
