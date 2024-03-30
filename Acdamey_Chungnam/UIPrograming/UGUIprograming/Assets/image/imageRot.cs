using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imageRot : MonoBehaviour
{
    bool textBool = false;
    private bool isClick = false;
    private Transform imgPos;

    public float roSpeed = 500f;
    public GameObject roulettelImage;

    public Text mText;

    float rotationSpeed = 0;

    float routionZ = 0;



    // Start is called before the first frame update
    void Start()
    {
        imgPos = roulettelImage.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isClick == true)
        {

            imgPos.Rotate(new Vector3(0, 0, roSpeed * Time.deltaTime));
            textBool = true;
            // rotationSpeed = Random.Range(30f, 50f);
            // Debug.Log(rotationSpeed);
            //  _imgpos.Rotate(0, 0, rotationSpeed);
            //  rotationSpeed *= 0.5f;
        }



        else
        {
            
           if(textBool)
            {
                routionZ = imgPos.eulerAngles.z;
                switch (Mathf.Floor((routionZ+30)/60))
                {
    
                    case 1:
                        mText.text = "�������";
                        textBool = false;
                        break;
                    case 2:
                        mText.text = "�������ſ쳪�ݺ���";
                        textBool = false;
                        break;
                    case 3:
                        mText.text = "�������";
                        textBool = false;
                        break;
                    case 4:
                        mText.text = "�������";
                        textBool = false;
                        break;
                    case 5:
                        mText.text = "�������";
                        textBool = false;
                        break;
                    default:
                        mText.text = "�������";
                        textBool = false;
                        break;


                }
            }

          

        }



    }

    public void RotatingImage()
    {
        isClick = !isClick;
    }


}
