using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageObject : MonoBehaviour
{
    private static readonly int _TrailDirNumber = Shader.PropertyToID("_TrailDir"); //����ȭ�� ���� ID ��������

    [SerializeField] private Renderer _renderer; //������ 
    [SerializeField] private Material _material; //���͸���
    [SerializeField] private Vector3 _trailPos;  //�˵���ġ
    [SerializeField] private float _trailRate = 10f; //�˵� ����

    void Awake()
    {
        _material = _renderer.material; //������ ������ ��������
        _trailPos = this.transform.position; //�ʱ� ������ �������� ��ġ ������

    }

    void Update()
    {
        _trailPos = Vector3.Lerp(_trailPos, transform.position, Mathf.Clamp01(Time.deltaTime * _trailRate));
        Vector3 dir = transform.InverseTransformDirection(_trailPos - transform.position);
        _material.SetVector(_TrailDirNumber, dir); //���ͳֱ�
    }
}
