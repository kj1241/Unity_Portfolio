/*
 2016.09.09 모바일증강현실 숙제1
 인스턴스로 받아서 거리 확인하기.
*/
using UnityEngine;
using System.Collections;

public class IsDistance_MonsterB : MonoBehaviour {

    public GameObject myObject;//인스턴스로 자신의 오브젝트를 받아준다.
    GameObject objectMonsterB;//이름으로 찾을수 있는 오브젝트는 하나이기 때문에.

    private string nameMyObject;
    private string nameObjectMonsterB;



    // Use this for initialization
    void Start () {
        objectMonsterB = GameObject.Find("MonsterB");
        nameMyObject = myObject.name;
        nameObjectMonsterB = objectMonsterB.name;


    }

    // Update is called once per frame
    void Update () {
        if (!nameMyObject.Equals(nameObjectMonsterB))
        {
            float far = Vector3.Distance(myObject.transform.position, objectMonsterB.transform.position);
            Debug.DrawLine(myObject.transform.position, objectMonsterB.transform.position, Color.red);
            Debug.Log(far);
        }
	}
}
