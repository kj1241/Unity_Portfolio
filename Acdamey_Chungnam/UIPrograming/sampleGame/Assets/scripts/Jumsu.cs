using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Jumsu : MonoBehaviour
{
    public TextMeshProUGUI JumsuText;
    int JumsuCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        JumsuText.text = "0";
    }

    public void AddJumsu(int n)
    {
        JumsuCount+=n;
        JumsuText.text = JumsuCount.ToString();
    }
}
