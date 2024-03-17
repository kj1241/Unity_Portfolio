using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonLisnnter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool _pressed = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("버튼이 눌려지고 있음");
        _pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("버튼 눌림이 해제됨");
        _pressed = false;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_pressed)
        {
            //버튼이 눌려진동안 액션 
        }
    }
}
