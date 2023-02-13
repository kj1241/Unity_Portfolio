using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Divide and Conquer �ϳ��� �Ϲ�ȭ ��
//�ʱ� ��ǥ�� 3������ 2�������� �ٲ㼭 xy �׷����� ���󰡴� ��
public class Skill_BezierMissile : MonoBehaviour
{
    private Vector3 startPos;
    //private Vector3 endPos = Vector3.zero;
    public GameObject enmey;


    float totalTime = 0; // �ѽð�

    [SerializeField]
    private float speed = 0.5f; // ���ǵ� ����
    private const int BezierNumber = 3; //���� �������� ����� ������
   // private Vector3[] points; //������ ������ �������� ������ �ø��� �ֱ� ������
    public Vector3 PointAlpha = new Vector3(-2.5f,1f,0f);
    public Vector3 PointBeta = new Vector3(0f, 1.5f, 0f);
    public Vector3 beforVector;
   // private Vector3 PointAddEnd = new Vector3(10f, 0f, 0f);

    void Start()
    {
        startPos = this.transform.position;
        beforVector = this.transform.position;
        //points = new Vector3[BezierNumber+1]; // n�� �������� n+1�� ������ ������ �ִ�
        //for(int i=0; i<BezierNumber+1; ++i) 
        //{
        //    points[i] = Vector3.zero; //�ʱ�ȭ
        //}

        // points[0] = startPos;  // this.transform.position; //�������� ������ ��ġ
        // points[1] = points[0] + PointAlpha;
        // points[2] = points[0] + PointBeta;
        // points[3] = endPos;  //this.transform.position + PointAddEnd; // ������ ������ġ
    }

    void Update()
    {
        if (totalTime < 1f) //������ġ�� �������� ���ߴٸ�
        {
            totalTime += Time.deltaTime * speed;
            //this.transform.position= BezierFunction(points); //��ġ�� �����ġ

            //������ �
            this.transform.position =
                new Vector3(BezierFunction(startPos.x, startPos.x + PointAlpha.x, startPos.x + PointBeta.x, enmey.transform.position.x),
                BezierFunction(startPos.y, startPos.y + PointAlpha.y, startPos.y + PointBeta.y, enmey.transform.position.y),
               BezierFunction(startPos.z, startPos.z + PointAlpha.z, startPos.z + PointBeta.z, enmey.transform.position.z));

            //������ ��� ���� ����ǥ ȸ��
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
   
    //���߿� ����ȭ �����ٶ� �ٲٱ� �ϵ� ���� 3�� ������ � �������ҷ� 2���� xy���� ����ϰ� ��������..
    Vector3 BezierFunction(Vector3[] points) 
    {
        //x��
        float X_LinearAB = Mathf.Lerp(points[0].x, points[1].x, totalTime);
        float X_LinearBC = Mathf.Lerp(points[1].x, points[2].x, totalTime);
        float X_LinearCD = Mathf.Lerp(points[2].x, points[3].x, totalTime);

        float X_LinearABC = Mathf.Lerp(X_LinearAB, X_LinearBC, totalTime);
        float X_LinearBCD = Mathf.Lerp(X_LinearBC, X_LinearCD, totalTime);

        float X_LinearABCD = Mathf.Lerp(X_LinearABC, X_LinearBCD, totalTime);

        //y��
        float Y_LinearAB = Mathf.Lerp(points[0].y, points[1].y, totalTime);
        float Y_LinearBC = Mathf.Lerp(points[1].y, points[2].y, totalTime);
        float Y_LinearCD = Mathf.Lerp(points[2].y, points[3].y, totalTime);

        float Y_LinearABC = Mathf.Lerp(Y_LinearAB, Y_LinearBC, totalTime);
        float Y_LinearBCD = Mathf.Lerp(Y_LinearBC, Y_LinearCD, totalTime);

        float Y_LinearABCD = Mathf.Lerp(Y_LinearABC, Y_LinearBCD, totalTime);

        return new Vector3(X_LinearABCD, Y_LinearABCD, points[0].z); // 3������ ���� ����ϴ°��� �ƴ����� 2�������� ���
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

    public void Init( GameObject Enmey , Quaternion rotation) //�ʱ�ȭ ���� , ������ ���� � x �� ȸ����
    {
        enmey = Enmey;
        //x������ ������ ���� Ǯ�� = ���� �ٶ󺸴� ���⺤�Ϳ� ���� ȸ��   //ȸ�� ������(x�� ���� , ���� ����(���� - ����))
        Quaternion temp = Quaternion.FromToRotation(new Vector3(1, 0, 0), Vector3.Normalize(Enmey.transform.position- this.transform.position));

        PointAlpha = temp * rotation * PointAlpha; // ȸ�� ���� ���� �������ֱ�
        PointBeta = temp * rotation * PointBeta; //ȸ�� �� ���� �������ֱ�
    }

    void OnTriggerEnter(Collider collision) //
    {
        Destroy(this.gameObject, 0.35f);
    }

}
