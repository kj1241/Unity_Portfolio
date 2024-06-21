using UnityEngine;
using System.Collections;

public class MonsterLineRange : MonoBehaviour {
    public float angles=1.0f;
    public int segments=32;
    public float xRadius=1.0f;
    public float zRadius=1.0f;
    public Color color = Color.red;
    LineRenderer line;
    // Use this for initialization
    void Start ()
    {
        createLine(angles, segments, xRadius, zRadius, color, line);//범위 생성
        gameObject.GetComponent<LineRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            gameObject.GetComponent<LineRenderer>().enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            gameObject.GetComponent<LineRenderer>().enabled = false;
        }
    }
    void createPoint(float Angles, int Segments, float XRadius, float ZRadius, LineRenderer line)
    {
        int segments = Segments;
        float xRadius = XRadius;
        float zRadius = ZRadius;

        float x;
        float y = 0f;
        float z;
        float angle = Angles;
        for (int i = 0; i < (segments + 2); i++)
        {
            x = Mathf.Cos(Mathf.Deg2Rad * angle) * xRadius;
            z = Mathf.Sin(Mathf.Deg2Rad * angle) * zRadius;
            line.SetPosition(i, new Vector3(x, y, z));
            
            angle += (360f / segments);
        }

    }
    void createLine(float Angles, int Segments, float XRadius, float ZRadius, Color Color, LineRenderer line)
    {
     //   LineRenderer line;
        line = gameObject.GetComponent<LineRenderer>();
        line.SetColors(Color, Color);
        line.SetVertexCount(Segments + 2);
        line.useWorldSpace = false;
        line.SetWidth(0.1f, 0.1f);
        createPoint(Angles, Segments, XRadius, ZRadius, line);
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        line.material.color = Color;
    }
}
