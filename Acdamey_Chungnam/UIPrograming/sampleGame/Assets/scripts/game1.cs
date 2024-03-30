using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class game1 : MonoBehaviour
{
    public GameObject ballObj;
    public GameObject holeObj;
    public TextMeshProUGUI distanceText;

    void Update()
    {
        float distance = Vector3.Distance(ballObj.transform.position, holeObj.transform.position);

        if (distance > 25)
        {
            distanceText.text = distance.ToString("F2") + "m";
        }
        else
        {
            distanceText.text = " !!!!!!!!!!!!!!!! 골 !!!!!!!!!!!!!!!!";
        }
    }
}
