using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
        initplane();
        initNoGreedMap();
        //well();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void initplane()
    {
        for (int i = 0; i < 9; i++)
        {
            float k = i / 3;
            float j = i % 3;
            GameObject map = GameObject.CreatePrimitive(PrimitiveType.Plane);
            map.transform.position = new Vector3(j * 10 - 10, 0, k * 10 - 10);
        }
    }
    void initNoGreedMap()
    {//for 루프 두번돌려서 간단하게 표현하고싶었지만 for루프안에 for 루프 쓰지말라고하셔서
        for (int i = 0; i < 30; i++)
        {
            GameObject map = GameObject.CreatePrimitive(PrimitiveType.Cube);
            map.transform.position = new Vector3(-15.5f, 0,-14.5f+i);//왼쪽
        }
        for (int i = 0; i < 30; i++)
        {
            GameObject map = GameObject.CreatePrimitive(PrimitiveType.Cube);
            map.transform.position = new Vector3(-14.5f + i, 0,  - 15.5f);//아래
        }
        for (int i = 0; i < 30; i++)
        {
            GameObject map = GameObject.CreatePrimitive(PrimitiveType.Cube);
            map.transform.position = new Vector3(-14.5f + i, 0, +15.5f);//위
        }
        for (int i = 0; i < 30; i++)
        {
            GameObject map = GameObject.CreatePrimitive(PrimitiveType.Cube);
            map.transform.position = new Vector3(+15.5f, 0, -14.5f + i);//오른쪽
        }
        for(int i=0; i<4; i++)
        {
            float k = i / 2;
            float j = i % 2;
            GameObject map = GameObject.CreatePrimitive(PrimitiveType.Cube);
            map.transform.position = new Vector3(j*31 -15.5f, 0, k * 31 -15.5f);
        }
    }
    /*
    void well()
    {
        int[] txt_data = new int[100];
        for(int i=0; i<100; i++)
        {
            txt_data[i] = i;
        }
        List<int> list = new List<int>(txt_data);
        for (int i = 0; i < 20; i++)
        {
            int tagetIndex = Random.Range(0, list.Count);

     

            GameObject map = GameObject.CreatePrimitive(PrimitiveType.Cube);
            map.transform.position = new Vector3((list[tagetIndex] % 10)*3 - 14.5f, 0, (list[tagetIndex] / 10)*3 - 14.5f);
            MShell.well(list[tagetIndex]);
            list.Remove(list[tagetIndex]);
        }
    }
    */
  

}
