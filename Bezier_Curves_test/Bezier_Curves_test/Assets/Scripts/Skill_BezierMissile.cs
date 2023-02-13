using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Divide and Conquer 하나씩 일반화 전
//초기 목표는 3차원을 2차원으로 바꿔서 xy 그래프로 날라가는 것
public class Skill_BezierMissile : MonoBehaviour
{
    private Vector3 startPos;
    //private Vector3 endPos = Vector3.zero;
    public GameObject enmey;


    float totalTime = 0; // 총시간

    [SerializeField]
    private float speed = 0.5f; // 스피드 조정
    private const int BezierNumber = 3; //몇차 방정식을 사용할 것인지
   // private Vector3[] points; //구현한 이유는 방정식의 차원을 늘릴수 있기 때문에
    public Vector3 PointAlpha = new Vector3(-2.5f,1f,0f);
    public Vector3 PointBeta = new Vector3(0f, 1.5f, 0f);
    public Vector3 beforVector;
   // private Vector3 PointAddEnd = new Vector3(10f, 0f, 0f);

    void Start()
    {
        startPos = this.transform.position;
        beforVector = this.transform.position;
        //points = new Vector3[BezierNumber+1]; // n차 방정식은 n+1의 갯수를 가지고 있다
        //for(int i=0; i<BezierNumber+1; ++i) 
        //{
        //    points[i] = Vector3.zero; //초기화
        //}

        // points[0] = startPos;  // this.transform.position; //시작점은 본인의 위치
        // points[1] = points[0] + PointAlpha;
        // points[2] = points[0] + PointBeta;
        // points[3] = endPos;  //this.transform.position + PointAddEnd; // 마지막 도착위치
    }

    void Update()
    {
        if (totalTime < 1f) //현재위치가 도착하지 못했다면
        {
            totalTime += Time.deltaTime * speed;
            //this.transform.position= BezierFunction(points); //위치는 계산위치

            //베지어 곡선
            this.transform.position =
                new Vector3(BezierFunction(startPos.x, startPos.x + PointAlpha.x, startPos.x + PointBeta.x, enmey.transform.position.x),
                BezierFunction(startPos.y, startPos.y + PointAlpha.y, startPos.y + PointBeta.y, enmey.transform.position.y),
               BezierFunction(startPos.z, startPos.z + PointAlpha.z, startPos.z + PointBeta.z, enmey.transform.position.z));

            //베지어 곡선에 따른 모델좌표 회전
            this.transform.rotation = Quaternion.FromToRotation(beforVector, this.transform.position);
            beforVector = this.transform.position;
        }
        else
        {
            totalTime = 1f;
            this.transform.position =
            new Vector3(BezierFunction(startPos.x, startPos.x + PointAlpha.x, startPos.x + PointBeta.x, enmey.transform.position.x),
            BezierFunction(startPos.y, startPos.y + PointAlpha.y, startPos.y + PointBeta.y, enmey.transform.position.y),
            BezierFunction(startPos.z, startPos.z + PointAlpha.z, startPos.z + PointBeta.z, enmey.transform.position.z));
        }
    }
   
    //나중에 정규화 시켜줄때 바꾸기 일딴 지금 3차 베지어 곡선 차원감소로 2차원 xy축을 사용하고 있음으로..
    Vector3 BezierFunction(Vector3[] points) 
    {
        //x축
        float X_LinearAB = Mathf.Lerp(points[0].x, points[1].x, totalTime);
        float X_LinearBC = Mathf.Lerp(points[1].x, points[2].x, totalTime);
        float X_LinearCD = Mathf.Lerp(points[2].x, points[3].x, totalTime);

        float X_LinearABC = Mathf.Lerp(X_LinearAB, X_LinearBC, totalTime);
        float X_LinearBCD = Mathf.Lerp(X_LinearBC, X_LinearCD, totalTime);

        float X_LinearABCD = Mathf.Lerp(X_LinearABC, X_LinearBCD, totalTime);

        //y축
        float Y_LinearAB = Mathf.Lerp(points[0].y, points[1].y, totalTime);
        float Y_LinearBC = Mathf.Lerp(points[1].y, points[2].y, totalTime);
        float Y_LinearCD = Mathf.Lerp(points[2].y, points[3].y, totalTime);

        float Y_LinearABC = Mathf.Lerp(Y_LinearAB, Y_LinearBC, totalTime);
        float Y_LinearBCD = Mathf.Lerp(Y_LinearBC, Y_LinearCD, totalTime);

        float Y_LinearABCD = Mathf.Lerp(Y_LinearABC, Y_LinearBCD, totalTime);

        return new Vector3(X_LinearABCD, Y_LinearABCD, points[0].z); // 3차원을 전부 사용하는것이 아님으로 2차원으로 계산
    }

    float BezierFunction(float x1, float x2, float x3, float x4)
    {
        float X_LinearAB = Mathf.Lerp(x1, x2, totalTime);
        float X_LinearBC = Mathf.Lerp(x2, x3, totalTime);
        float X_LinearCD = Mathf.Lerp(x3, x4, totalTime);

        float X_LinearABC = Mathf.Lerp(X_LinearAB, X_LinearBC, totalTime);
        float X_LinearBCD = Mathf.Lerp(X_LinearBC, X_LinearCD, totalTime);

        float X_LinearABCD = Mathf.Lerp(X_LinearABC, X_LinearBCD, totalTime);

        return X_LinearABCD;
    }

    public void Init( GameObject Enmey , Quaternion rotation) //초기화 적군 , 베지어 월드 곡선 x 축 회전값
    {
        enmey = Enmey;
        //x축으로 고정된 백터 풀기 = 적을 바라보는 방향벡터에 따른 회전   //회전 각도값(x축 백터 , 단위 백터(적군 - 본인))
        Quaternion temp = Quaternion.FromToRotation(new Vector3(1, 0, 0), Vector3.Normalize(Enmey.transform.position- this.transform.position));

        PointAlpha = temp * rotation * PointAlpha; // 회전 값들 전부 갱신해주기
        PointBeta = temp * rotation * PointBeta; //회전 값 전부 갱신해주기
    }

    void OnTriggerEnter(Collider collision) //
    {
        Destroy(this.gameObject, 0.35f);
    }

}
