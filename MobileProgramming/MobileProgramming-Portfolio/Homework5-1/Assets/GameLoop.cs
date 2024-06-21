using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {
    Camera kCamera;
    bool isMove;
    // Use this for initialization
    void Start () {
        kCamera = GetComponent<Camera>();
        this.kCamera = Camera.main;
        StartCoroutine(click());
        isMove = false;

    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            Vector3 point = kCamera.ScreenToWorldPoint(new Vector3(x, y, 10));
           
        }
        */
        //요렇게 쓰는거랑 엄청난 클릭반응속도 차이가 난다.
    }
    IEnumerator click()
    {
        Vector3 point=Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {

            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            point = kCamera.ScreenToWorldPoint(new Vector3(x, y, kCamera.transform.position.y));
            //이 z 깊이로 부터 물리적 계산을해야함 비틀려면 로테인션값을 포지션갑에 곱해서 전부 더해줘야함 하지만귀찮음으로 패스
            // 이렇게 되면 0,0까지의 깊이를 계산할수 있다.
            // 뷰잉좌표에서 월드좌표까지 계산. 비틀려면 수학적 계산필요.
            Debug.Log(point);
            if ((point.x <= 5 && point.x > -5) && (point.z <= 5 && point.z > -5))
            {
                BFSMgr bFSMgr = new BFSMgr();
                BFSMgr.bFSList.Clear();
                BFSMgr.bFS = new BFSMgr.BFS[100];
                for (int i = 0; i < 100; i++)
                {
                    BFSMgr.bFS[i] = new BFSMgr.BFS();
                }
                //초기화 해주기
                bFSMgr.Start_number = (int)(transform.position.x + 5) * 10 + (int)(transform.position.z + 5);
                bFSMgr.End_number = (int)(point.x + 5) * 10 + (int)(point.z + 5);
                bFSMgr.list_while((int)(transform.position.x + 5) * 10 + (int)(transform.position.z + 5));
                BFSMgr.bFSList.Clear();
                bFSMgr.list_finsh((int)(point.x + 5) * 10 + (int)(point.z + 5));

                isMove = true;
            }
        }
        yield return new WaitForFixedUpdate();
        //코루틴중에 가장낳음 하지만 원래 클릭은 업데이트에 들어가야 가장 빠르게 동작할수가 있다.
        //저번에 교수님꼐서 업데이트에 넣지 말라고해서 코루틴쓰면 그나마 낳은 차선택으로 고름

        if (isMove ==true)
        {
            StartCoroutine(moveObject((int)(point.x + 5) * 10 + (int)(point.z + 5)));
        }
        else
        {
          
            StartCoroutine(click());
        }//코루틴은 계속 돌기때문에 엘스문으로 오류처리 안해주면 밑으로 다른코루틴생성하면서 시작됨.
    }
    IEnumerator moveObject(int number)
    {
        
        int x0 = BFSMgr.bFSList[0] / 10;
        int z0 = BFSMgr.bFSList[0] % 10;

        int x1 = BFSMgr.bFSList[1] / 10;
        int z1 = BFSMgr.bFSList[1] % 10;

        float x = transform.position.x;
        float z = transform.position.y;

        
        transform.position = new Vector3((x1 - x0) + x, 0, (z1 - z0) + z);
        BFSMgr.bFSList.RemoveAt(0);
        Debug.Log(BFSMgr.bFSList[0]);
        //삭제됬음으로 나머진 리스트 O 에 존재할것이다.
        yield return new WaitForFixedUpdate();

        if (BFSMgr.bFSList[0] == number)
        {
            isMove = false;
            StartCoroutine(click());
        }
        else
        {
            StartCoroutine(moveObject(number));
        }
       
    }
}
//교수님한테 너무까여서 맨날 프로토 타입만 작성하다 요번에 재대로 작성해 봅니다....
//음 이번 과제는 제가 생각하지 못한부분 알려주시면 감사합니다!
//코루틴에서 물리엔진 끝나고 렌더링하는거라서 너무 빨리 광클릭하면 참고로 자동으로 꺼집니다. 아마 재귀용법에 +코루틴으로 수도없는 스택이 쌓어서 그냥 종료되는걸로 보입니다.