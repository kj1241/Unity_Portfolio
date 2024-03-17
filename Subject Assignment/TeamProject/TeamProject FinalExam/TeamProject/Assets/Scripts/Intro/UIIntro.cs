using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIIntro : MonoBehaviour
{
    enum UINumber
    {
        Text1 = 0,
        Text2 = 1,
        MoveSlate = 2,
        end = 3
    }

    public Image[] UIOjbect = new Image[4];
    bool isEnd = false;
    UINumber numberUI = UINumber.Text1;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        UIOjbect[0].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isEnd)
        {
            switch (numberUI)
            {
                case UINumber.Text1:
                    ++numberUI;
                    UIOjbect[0].gameObject.SetActive(false);
                    UIOjbect[1].gameObject.SetActive(true);
                    break;

                case UINumber.Text2:
                    ++numberUI;
                    UIOjbect[1].gameObject.SetActive(false);
                    UIOjbect[2].gameObject.SetActive(true);
                    UIOjbect[3].gameObject.SetActive(true);
                    break;
            }
            //SceneManager.LoadSceneAsync("Intro");
        }

        if (UIOjbect[2].gameObject.activeSelf == true && UIOjbect[3].gameObject.activeSelf == true)
        {
            if (UIOjbect[2].rectTransform.anchoredPosition.y > 200f)
            {
                Vector3 temp = UIOjbect[2].rectTransform.anchoredPosition;
                temp.y -= 30;

                if (temp.y <= 200f)
                {
                    UIOjbect[2].rectTransform.anchoredPosition = new Vector3(0f, 200f, 0f);
                    SceneManager.LoadSceneAsync("MainScenes");
                }
                else
                {
                    UIOjbect[2].rectTransform.anchoredPosition = temp;
                }
            }
            if (UIOjbect[3].rectTransform.anchoredPosition.y < -200f)
            {
                Vector3 temp = UIOjbect[3].rectTransform.anchoredPosition;
                temp.y += 30;

                if (temp.y >= -200f)
                {
                    UIOjbect[3].rectTransform.anchoredPosition = new Vector3(0f, -200f, 0f);
                    SceneManager.LoadSceneAsync("MainScenes");
                }
                else
                {
                    UIOjbect[3].rectTransform.anchoredPosition = temp;
                }
            }

        }
    }
}
