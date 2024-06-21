using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MShell : MonoBehaviour
{
    public static Neighbors[] neighBors;//원래는 8방향을전부 메소드로 끄내줘야하는데 시간부족으로 이렇게 만들었습니다.
    public class Neighbors
    {//인스턴스 함수화 시키기
        public int[] neighborhoods= new int[9];
        public int speed;
        public Neighbors(int number)
        {
            neighborhoods = new int[9] { number + 10, number + 11, number + 1, number - 9, number - 10, number - 11, number - 1, number + 9, -1 };
            if (number / 10 == 9) neighborhoods[0] = neighborhoods[1] = neighborhoods[7] = -1;
            if (number / 10 == 0) neighborhoods[3] = neighborhoods[4] = neighborhoods[5] = -1;
            if (number % 10 == 0) neighborhoods[5] = neighborhoods[6] = neighborhoods[7] = -1;
            if (number % 10 == 9) neighborhoods[1] = neighborhoods[2] = neighborhoods[3] = -1;
            speed = 1;
          
            
        }
    

    }
    public class BFS
    {
        public int prev;
        public int sumSpeed;
        
    }
    static public BFS[] bfs;
    static public  List<int> bFSList = new List<int>();

    static public void well(int number)
    {
        for (int i = 0; i < 9; i++)
        {
            neighBors[number].neighborhoods[i] = -1;
            //speed[i] = 0;
        }
    }

    // Use this for initialization
    void Start()
    {
        bfs = new BFS[100];
        neighBors = new Neighbors[100];
        for (int i = 0; i < 100; i++)
        {
            neighBors[i] = new Neighbors(i);
            bfs[i] = new BFS();
        }
        
       
    }

    // Update is called once per frame
    void Update()
    {

    }

   


}
