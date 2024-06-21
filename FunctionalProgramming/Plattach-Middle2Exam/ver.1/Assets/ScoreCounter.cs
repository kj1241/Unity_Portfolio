using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour {
    public struct Count
    {
        public int ignite; // 연쇄 수
        public int score; // 점수
        public int total_socre; // 합계 점수
    };
    static public Count last; // 마지막(이번) 점수
    public Count best; // 최고 점수.
    public static int QUOTA_SCORE = 1000; // 클리어 하는 데 필요한 점수.
    public GUIStyle guistyle;
    static public int final_Socre;
    public static bool piver;
    public static bool piverMoff;

    public void print_value(int x, int y, string label, int value)
    {
        // label을 표시.
        GUI.Label(new Rect(x, y, 100, 20), label, guistyle);
        y += 15;
        // 다음 행에 value를 표시.
        GUI.Label(new Rect(x + 20, y, 100, 20), value.ToString(), guistyle);
        y += 15;

    }
    private void update_score()
    {
        if (piver==true)
        {
            last.score = last.ignite * 10*10; // 점수 갱신.
        }

        else
            last.score = last.ignite * 10; // 점수 갱신.
        
    }
    // 합계 점수를 갱신
    public void updateTotalScore()
    {
        last.total_socre += last.score;
        
    }
    static public void ScoreUp(int Scores)
    {
        last.total_socre += Scores;

    }
 
    // 게임을 클리어했는지 판정 (SceneControl에서 사용)
    public bool isGameClear()
    {
        bool is_clear = false;
        // 현재 합계 점수가 클리어 기준보다 크면.
        if (mTime.time < 0)
        {
            is_clear = true;
        }
        return (is_clear);
    }


    // 연쇄 횟수를 가산
    public void addIgniteCount(int count)
    {
        last.ignite += count; // 연쇄 수에 count를 합산.
        this.update_score(); // 점수 계산.
    }
    // 연쇄 횟수를 리셋
    public void clearIgniteCount()
    {
        last.ignite = 0; // 연쇄 횟수 리셋.
    }
    void OnGUI()
    {
        int x = 20;
        int y = 50;
        GUI.color = Color.black;
        this.print_value(x + 20, y, "연쇄 카운트", last.ignite);
        y += 30;
        this.print_value(x + 20, y, "가산 스코어", last.score);
        y += 30;
        this.print_value(x + 20, y, "합계 스코어", last.total_socre);
        y += 30;

        if(piver==true)
            GUI.Label(new Rect(x, y, 100, 20), "피버 모드", guistyle);
      
        y += 30;
    }

    // Use this for initialization
    void Start () {
        last.ignite = 0;
        last.score = 0;
        last.total_socre = 0;
        this.guistyle.fontSize = 16;
        piver = false;
        piverMoff = true;
    }
	
	// Update is called once per frame
	void Update () {

	}

   
}
