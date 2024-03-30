using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LastWall : MonoBehaviour
{
    public Image playerPowerImg;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "ETag")
        {
            playerPowerImg.fillAmount -= 0.1f;
            Debug.Log(other);
        }

        if(playerPowerImg.fillAmount==0)
        {
            SceneManager.LoadScene("game2");
        }
        
    }
}
