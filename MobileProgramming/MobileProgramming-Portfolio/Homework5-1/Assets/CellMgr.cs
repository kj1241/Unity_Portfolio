using UnityEngine;
using System.Collections;

public class CellMgr : MonoBehaviour {
    //일딴 속성이없어서 모노디벨로프를 빼도 상관은없지만 랜더링 따로 셀마다 걸어주려면 있어야한다.
    public static MCell[] mCells;
    //셀을 스태틱함수로 지정 코드영역에 대기 ->사실상 전역변수 모으는 스크립트에 넣어야하지만 일딴 따로 안걸어도되니 생략
    public class MCell
    {
        public int Speed { get; set; }
        //셀에서의 거리/시간 = 1/속도
        public int[] neighborhoods = new int[9];
        // 음 프로퍼티를 만들고싶은데 0~8을 노가다하려는거에 대한 혐오감을 느낍니다. 차라리 퍼블릭으로.... 
        //솔찍이 이부분은 줄이는 방법을 모르겠네요
        //상,하,좌,우, 상좌, 상우, 하좌, 하우, 워프게이트
        //추가 인덱스로 걸지않고 이차 배열로 쓰려고했던이유; 초기화할때부터 힘들어진다.
        //이차배열로 쓰면 xy가 몇개씩 있는지 알수 있지만 인덱스로 쓰게되면 알수없어서 ★범용성이 떨어진다.
        public MCell(int number)
        {
            Speed = 1;
            neighborhoods[0] = number + 10;
            neighborhoods[1] = number + 11;
            neighborhoods[2] = number + 1;
            neighborhoods[3] = number - 9;
            neighborhoods[4] = number - 10;
            neighborhoods[5] = number - 11;
            neighborhoods[6] = number - 1;
            neighborhoods[7] = number + 9;
            neighborhoods[8] = -1;
            //어짜피 여기서는 코드를 줄일수 없다!

           if (number / 10 == 0)
           {
                neighborhoods[3] = -1;
                neighborhoods[4] = -1;
                neighborhoods[5] = -1;
           }
           //사실 이부분에서 10은 그냥 우리가 정한거지 범용성을가지려면 솔찍히 xy로 써써 몇개 있는지 알기만하면 초기화할때부터 넣을수 있기때문.
           //교수님 요구조건으로 그냥 인덱스 쓰기로 결정
            if (number / 10 == 9)
            {
                neighborhoods[0] = -1;
                neighborhoods[1] = -1;
                neighborhoods[7] = -1;
            }
            if (number % 10 == 0)
            {
                neighborhoods[5] = -1;
                neighborhoods[6] = -1;
                neighborhoods[7] = -1;
            }
            if (number % 10 == 9)
            {
                neighborhoods[1] = -1;
                neighborhoods[2] = -1;
                neighborhoods[3] = -1;
            }
        }  
        //추가 셀의 속성자리 (추가)
    }

   
	// Use this for initialization
	void Start () {
        mCells = new MCell[100];
        for(int i=0;i<100;i++)
        {
            mCells[i] = new MCell(i);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
