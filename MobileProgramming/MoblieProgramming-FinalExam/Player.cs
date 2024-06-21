using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{
    int playerMoveSpeed;
    Renderer rend;
    GameObject players;

    Coroutine cor;
    MShell gm = null;
    bool moving;
    
    // Use this for initialization
    void Start()
    {
        moving = false;
        players = GameObject.FindGameObjectWithTag("Player");
        playerMoveSpeed = 6; //속도 관련 
        palyerColor();
        gm = gameObject.GetComponent<MShell>() as MShell;
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveEvent();
        RayCast();
    }

    void playerMoveEvent()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // 입력이벤트
        if(moveDirection != Vector3.zero)
        {
            players.transform.Translate(moveDirection * playerMoveSpeed * Time.smoothDeltaTime, Space.World);//이동
            players.transform.LookAt(players.transform.position + moveDirection); //회전
        }
    }
    void palyerColor()
    {
        rend = players.GetComponent<Renderer>();
        rend.material.color = new Vector4(1f, 0.8f, 0.8f, 1);
    }

    void RayCast()
    {// 이번엔 레이캐스트로....
        if (Input.GetMouseButtonUp(0))
        {
           // Coroutine move_coroutine = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
         
            if (Physics.Raycast(ray, out hitInfo))
            {
                // if (move_coroutine != null) StopCoroutine(move_coroutine);
                // move_coroutine = 
                moving = false;
                  
              StartCoroutine(move(players, hitInfo.point));

            }
        }
    }
    int mathRound(float x)
    {
        int k;
        if (x >= 14)
        {
            k = 9;
            return k;
        }
        k = (int)Mathf.Round((x + 14.5f) / 3);
        return k;
    }
    public IEnumerator move(GameObject players, Vector3 destination)
    {

        moving = true;
        int start = mathRound(players.transform.position.z) * 10 + mathRound(players.transform.position.x);
        int end = mathRound(destination.z) * 10 + mathRound(destination.x);

        MShell.bFSList.Clear();
        initMoveline(start, end);
        MShell.bFSList.Clear();
        drawMvoeline(start, end);


        while (moving)
        {

            Vector3 temp = new Vector3((float)(MShell.bFSList[0] / 10) * 3 - 14.5f, 0, (float)(MShell.bFSList[0] % 10) * 3 - 14.5f);


            Vector3[] v = new Vector3[2];
            v[0] = new Vector3((MShell.bFSList[0] % 10) * 3 - 13.5f, 0, (MShell.bFSList[0] / 10) * 3 - 13.5f);
            v[1] = new Vector3((MShell.bFSList[1] % 10) * 3 - 13.5f, 0, (MShell.bFSList[1] / 10) * 3 - 13.5f);

            Debug.Log(v[1]);
            Debug.Log(v[0]);
            players.transform.position = Vector3.MoveTowards(v[0], v[1], 6 * Time.deltaTime);

            MShell.bFSList.RemoveAt(0);
            if (MShell.bFSList == null || MShell.bFSList.Count == 0 || MShell.bFSList[0] == end)
            {
                yield break;
            }

            yield return null; 
      }
        
    }



    
    public void initMoveline(int start, int end)
    {
        int number = start;
        while (true)
        {
            for (int i = 0; i < 9; i++)
            {

                if (MShell.neighBors[number].neighborhoods[i] != -1)
                {
                    int j = MShell.neighBors[number].neighborhoods[i];
                    if (MShell.bfs[j].sumSpeed == 0 || (MShell.bfs[j].sumSpeed > MShell.bfs[i].sumSpeed + MShell.neighBors[j].speed))
                    {
                        
                        MShell.bfs[j].prev = number;
                        MShell.bfs[j].sumSpeed = MShell.bfs[i].sumSpeed + MShell.neighBors[j].speed;
                        MShell.bFSList.Add(j);
                    }

                }
            }
            if (MShell.bFSList.Count == 0)
                break;
           
            number = MShell.bFSList[0];
            MShell.bFSList.RemoveAt(0);
            if (number == end)
                break;
        }
    }
    //제가 뭘 고장내트렸는진 모르겠지만 컴퓨터에서 와일문만 쓰려고하면 느리네요 현제 간단한 while문 두개썼을때 실행이안됩니다....
    //아 속도 측정해서 따로넣을떄 시간걸리네요 속도 모두 동일하게 하겠습니다.
    /*
    
    public void initMoveline(int start, int end, int number)
    {

        int next;
        for (int i = 0; i < 9; i++)
        {

            if (MShell.neighBors[number].neighborhoods[i] != -1)
            {
                int j = MShell.neighBors[number].neighborhoods[i];
                if ((MShell.bfs[j].sumSpeed == 0 || (MShell.bfs[j].sumSpeed > MShell.bfs[i].sumSpeed + MShell.neighBors[j].speed[i])))
                {
                    Debug.Log(MShell.bfs[j].prev);
                    MShell.bfs[j].prev = number;
                    MShell.bfs[j].sumSpeed = MShell.bfs[i].sumSpeed + MShell.neighBors[j].speed[i];
                    MShell.bFSList.Add(j);
                }

            }
        }
        next = MShell.bFSList[0];
        if (MShell.bFSList.Count == 0)
            return;

        MShell.bFSList.RemoveAt(0);
        if (number == end)
            return;
        initMoveline(start, end, next);
    }

    */

    public void drawMvoeline(int start, int end)
    {
        int number = end;
        while (true)
        {
        

            MShell.bFSList.Insert(0, number);
            number = MShell.bfs[number].prev;
            if ( MShell.bFSList[0] == start)
                break;
        }


    }
    
}
