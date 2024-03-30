using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public UILabel tempUILabel;
    public UISlider tmpUISlider;
    // Start is called before the first frame update
    void Start()
    {
        tempUILabel = GetComponent<UILabel>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeFont()
    {
        tempUILabel.text = "버튼이 눌렸습니다.";
    }

    public void changeFont2()
    {
        tempUILabel.text = "버튼이 눌렸습니다.";
    }

    public void OnSliderChange()
    {
        tempUILabel.text = tmpUISlider.sliderValue.ToString();
    }

}
