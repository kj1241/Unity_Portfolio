using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageObject : MonoBehaviour
{
    private static readonly int _TrailDirNumber = Shader.PropertyToID("_TrailDir"); //최적화를 위해 ID 가저오기

    [SerializeField] private Renderer _renderer; //랜더러 
    [SerializeField] private Material _material; //머터리얼
    [SerializeField] private Vector3 _trailPos;  //궤도위치
    [SerializeField] private float _trailRate = 10f; //궤도 비율

    void Awake()
    {
        _material = _renderer.material; //본인의 렌더러 가져오기
        _trailPos = this.transform.position; //초기 본인의 포지션이 위치 포지션

    }

    void Update()
    {
        _trailPos = Vector3.Lerp(_trailPos, transform.position, Mathf.Clamp01(Time.deltaTime * _trailRate));
        Vector3 dir = transform.InverseTransformDirection(_trailPos - transform.position);
        _material.SetVector(_TrailDirNumber, dir); //백터넣기
    }
}
