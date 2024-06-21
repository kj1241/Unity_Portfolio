using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BFSMgr : MonoBehaviour {
    
    public static BFS[] bFS;
    //스태틱을 써써 데이터 영역위에 놓기. 속도계선효과 아마도...
    public static List<int> bFSList = new List<int>();
    //public static List<BFS> bFSList = new List<BFS>();
    //클래스로 넣을수 있지만 인덱스를 넣어서 조금이라도 속도를 빠르게 계선시켜보자. (인덱스 접근)
    public int Start_number { get; set; }
    public int End_number { get; set; }
  
    public class BFS
    {
        public int Prev { get; set; }
        public int SumSpeed { get; set; }
        public BFS()
        {
            Prev = 0;
            SumSpeed = 0;
        }
    };


    public void list_while(int number)
    {
        int next;
        if (number == End_number)
        {
            return;
            //지긋지긋한 제귀용법 리턴 조건
        }
        for (int i = 0; i < 9; i++)
        {
            if (CellMgr.mCells[number].neighborhoods[i] != -1)
            {
                int j = CellMgr.mCells[number].neighborhoods[i];
                //코드길어짐으로 새로운 변수를 이용해 인덱스 번호 창출;
                if (bFS[j].SumSpeed == 0 || bFS[j].SumSpeed> bFS[i].SumSpeed+ CellMgr.mCells[j].Speed)
                {
                    bFS[j].Prev = number;
                    //지금의 번호를 넣자;
                    bFS[j].SumSpeed = bFS[i].SumSpeed + CellMgr.mCells[j].Speed;
                    //지금까지 스피드합 갱신
                    bFSList.Add(j);
                    //리스트에 제이 번호 갱신
                }
                //합계스피드가 0이면 아직 안간 셀, 스피드의 합이 간셀보다 작으면 다시 제이에 들어갈자격이 있다.
            }
        }
        if(bFSList.Count==0)
        {
            return;
        }
        //리스트에 더이상 뽑을게 없으면 제귀함수 종료
        next = bFSList[0];
        bFSList.RemoveAt(0);
        //다음 리스트에있는거 출력하고 제거하라
        list_while(next);
        //제귀용법 실행;
    }

    public void list_finsh(int number)
    {
      
        //if (bFSList[0] != number)
        bFSList.Insert(0, number);
        Debug.Log(bFSList[0]);
        //삽입조건
        if (number == Start_number|| bFS[number].Prev==0)
            return;
        //종료조건 
        list_finsh(bFS[number].Prev);
        //제귀용법
    }

	// Use this for initialization
	void Start () {
       // BFS[] bFS=new BFS[100];
        //사실상 100 이넘이나 디파인으로 정해주는게 맞긴하지만 귀찮음으로.!
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
