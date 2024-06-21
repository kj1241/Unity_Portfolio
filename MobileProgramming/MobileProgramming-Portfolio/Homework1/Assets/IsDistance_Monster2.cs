/*
 질문)
 유니티에서는 큐브를 생성하는순간 오브젝트에 이름과 자신의 상태 행렬값을 저장하게 됩니다.
 따라서 게임오브젝트로 이름으로 찾는것이 가능합니다.
 다른 방법으로 생각해도 위에 문제 같이 풀게되면 아무리 자신의상태를 두번 저장하는것이 된다고 생각합니다.
 또한 그게 자신인지 확인해야하기때문에 불필요한 메모리 증가를 사용하게 된다고 생각합니다.
 그러나 빈 오브젝트를 생성후에 한번에 불러주는 것이 더 효과가 있을거 같다는 생각이 듭니다.
 또한 빈오브젝트는 렌더링을 하지않음으로 컴퓨터상 안나오게 됨으로 백그라운드처럼 생각할수 있습니다. 더욱 게산이 빨라질수 있다고 생각합니다.
 근데 왜 이 방법이 안 좋은지 모르겠습니다.
 */

using UnityEngine;
using System.Collections;

public class IsDistance_Monster2 : MonoBehaviour {
    private GameObject myObject;
    private GameObject enmyObject;

	// Use this for initialization
	void Start () {
        myObject = GameObject.Find("Monster1");
        enmyObject = GameObject.Find("Monster2");
    }
	
	// Update is called once per frame
	void Update () {
        float far = Vector3.Distance(myObject.transform.position, enmyObject.transform.position);
        Debug.DrawLine(myObject.transform.position, enmyObject.transform.position, Color.red);
        Debug.Log(far);

    }
}
