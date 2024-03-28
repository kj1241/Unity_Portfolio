using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIMain : MonoBehaviour
{
    public TMP_Text logText = null;
    public ScrollRect scroll_rect = null;
    public Image EnemyUIImage;
    public Sprite[] WinImage = new Sprite[2];
    public Sprite[] EnemyUISprite = new Sprite[3];
    public GameObject BeckButton;
    public GameObject[] Buttons = new GameObject[4];
    public GameObject[] EnemyHpBar = new GameObject[3];
    public Image[] EnemyHpBarImage = new Image[3];
    public GameObject PlayerHpBar;
    public GameObject EffectBar;
    public Image PlayerHpBarImage;
    private int PlayerHp = 100;
    public GameObject[] Enemy = new GameObject[3];
    public GameObject PlayerLand;
    private Collider playerCollder;
    public GameObject DamgeBar;
    public Image DamgeBarImage;
    int damge = 0;
    bool damageImageRight = true;

    public class EnemyInfo
    {
        public int HP = 100;
        public bool isTeam = false;
        public int number;
    }
    Dictionary<GameObject, EnemyInfo> EnemyTable = new Dictionary<GameObject, EnemyInfo>(); //참조테이블
    private GameObject taget;

    public GameObject[] HeelParticle = new GameObject[2];
    GameObject[] heel = new GameObject[2];
    bool isHeelCoolTime = false;
    public Image[] HeelCoolTimeImage = new Image[2];
    public TMP_Text HeelCoolTimeText;
    public int HeelCoolTimeNumber;

    public GameObject FireParticle;
    public GameObject ExplosionParticle;

    bool[] frends = new bool[3];
    public Image[] FrendsCoolTimeImage = new Image[2];
    public TMP_Text FrendsCoolTimeText;
    public int FrendsCoolTimeNumber;

    public Image[] Slider = new Image[2];
    public Sprite Startintro;

    public enum PlayerState
    {
        none = 0,
        EnemySelect = 1,
        SkillSelect = 2,
        End=3
    }

    public enum enemySelect
    {
        none = 0,
        enemy1Select = 1,
        enemy2Select = 2,
        enemy3Select = 3
    }

    public enum GameState
    {
        stop =0,
        play =1
    }

    GameState gameState = (GameState)0;
    static public PlayerState UIState = (PlayerState)0;
    static public enemySelect EnemyState = 0;

    int Gold = 0;
    int peple = 0;
    int Land = 1;
    public Image[] informationUI = new Image[3];
    public TMP_Text[] informationText = new TMP_Text[3];

    public AudioClip[] endAudioClips = new AudioClip[2];
    public AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(StartIntro());

        for (int i = 0; i < 3; ++i)
        {
            frends[i] = false;
            EnemyInfo enemyInfo = new EnemyInfo();
            enemyInfo.number = i;
            EnemyTable.Add(Enemy[i], enemyInfo);
        }

        playerCollder = PlayerLand.GetComponent<Collider>();
        StartCoroutine(Land1AI(0, 7));
        StartCoroutine(Land1AI(1, 9));
        StartCoroutine(Land1AI(2, 15));

        informationUI[0].fillAmount = (float)Land / 4;
        informationUI[1].fillAmount = (float)peple / 99;
        informationUI[2].fillAmount = (float)Gold / 999999;

        informationText[0].text = Land.ToString() + "/4";
        informationText[1].text = peple.ToString() + "/ 99";
        informationText[2].text = Gold.ToString() + "/ 999,999";

    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == (GameState)0)
            return;
            
        switch (UIState)
        {
            case (PlayerState)0:

                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.CompareTag("Enemy"))
                        {
                            logText.text += "Enemy " + hit.transform.gameObject.name + "Select \n";
                            scroll_rect.verticalNormalizedPosition = 0.0f;

                            if (hit.transform.gameObject.name.Equals("Land1") && !frends[0])
                            {
                                taget = hit.transform.gameObject;
                                EnemyUIImage.gameObject.SetActive(true);
                                BeckButton.SetActive(true);
                                Buttons[0].SetActive(true);
                                Buttons[1].SetActive(true);
                                EnemyUIImage.sprite = EnemyUISprite[0];
                                UIState = (PlayerState)1;
                            }
                            else if (hit.transform.gameObject.name.Equals("Land2") && !frends[1])
                            {
                                taget = hit.transform.gameObject;
                                EnemyUIImage.gameObject.SetActive(true);
                                BeckButton.SetActive(true);
                                Buttons[0].SetActive(true);
                                Buttons[1].SetActive(true);
                                EnemyUIImage.sprite = EnemyUISprite[1];
                                UIState = (PlayerState)1;
                            }
                            else if (hit.transform.gameObject.name.Equals("Land3") && !frends[2])
                            {
                                taget = hit.transform.gameObject;
                                EnemyUIImage.gameObject.SetActive(true);
                                BeckButton.SetActive(true);
                                Buttons[0].SetActive(true);
                                Buttons[1].SetActive(true);
                                EnemyUIImage.sprite = EnemyUISprite[2];
                                UIState = (PlayerState)1;
                            }
                        }
                    }
                }
                break;
        }
        for (int i = 0; i < 3; ++i)
        {
            EnemyHpBarImage[i].fillAmount = EnemyTable[Enemy[i]].HP / (float)100;
        }

        if (DamgeBar.activeSelf == true)
        {
            if (damageImageRight)
                damge += 5;
            else
                damge -= 5;

            if (damageImageRight && damge > 100)
                damageImageRight = false;
            if (!damageImageRight && damge < 0)
                damageImageRight = true;

            DamgeBarImage.fillAmount = (float)damge / (float)100;
        }


        if (PlayerHp > 100)
            PlayerHp = 100;
        PlayerHpBarImage.fillAmount = (float)PlayerHp / 100;

        informationUI[0].fillAmount = (float)Land / 4;
        informationUI[1].fillAmount = (float)peple / 99;
        informationUI[2].fillAmount = (float)Gold / 999999;

        informationText[0].text = Land.ToString() + "/4";
        informationText[1].text = peple.ToString() + "/ 99";
        informationText[2].text = Gold.ToString() + "/ 999,999";

        Gold += 100;
        peple += 1;
        if (Gold > 999999)
            Gold = 999999;
        if (peple > 99)
            peple = 99;

        if (PlayerHp <= 0)
        {
            audio.Stop();
            audio.clip = endAudioClips[1];
            audio.Play();
            Invoke("TimeStop", 5f);
            EnemyUIImage.gameObject.SetActive(true);
            EnemyUIImage.sprite = WinImage[1];
            UIState = (PlayerState)3;
            BeckButton.SetActive(true);

            

        }
        if (Land >= 4)
        {
            audio.Stop();
            audio.clip = endAudioClips[0];
            audio.Play();

            Invoke("TimeStop", 5f);
            EnemyUIImage.gameObject.SetActive(true);
            EnemyUIImage.sprite = WinImage[0];
            UIState = (PlayerState)3;
            BeckButton.SetActive(true);
        }


    }

    public void TimeStop()
    {
        Time.timeScale = 0;
    }


    public void ClickBackButton()
    {
        switch (UIState)
        {
            case (PlayerState)1:
                logText.text += "Click The BackButton\n";
                EnemyUIImage.gameObject.SetActive(false);
                Buttons[0].SetActive(false);
                Buttons[1].SetActive(false);
                BeckButton.SetActive(false);
                UIState = (PlayerState)0;
                break;
            case (PlayerState)2:
                logText.text += "Click The BackButton\n";
                Buttons[0].SetActive(true);
                Buttons[1].SetActive(true);
                Buttons[2].SetActive(false);
                Buttons[3].SetActive(false);            
                
                //BeckButton.SetActive(false);
                UIState = (PlayerState)1;
                break;
            case (PlayerState)3:
                SceneManager.LoadSceneAsync("StartScenes");
                break;
        }
    }

    public void FightButton()
    {
        logText.text += "Click The FightButton\n";
        Buttons[0].SetActive(false);
        Buttons[1].SetActive(false);
        Buttons[2].SetActive(true);
        Buttons[3].SetActive(true);
        UIState = (PlayerState)2;
    }


    public void AtteckButtonDown()
    {
        if (peple > 50)
        {
            peple -= 50;
            damageImageRight = true;
            damge = 0;
            Buttons[3].SetActive(false);
            BeckButton.SetActive(false);
            DamgeBar.SetActive(true);
        }
        else
        {
            logText.text += "haven't People\n";
        }

    }

    public void AtteckButtonUP()
    {
        //Buttons[3].SetActive(false);
        //BeckButton.SetActive(false);

        DamgeBar.SetActive(false);

        logText.text += "Click The AtteckButton\n";
        EnemyUIImage.gameObject.SetActive(false);
        Buttons[2].SetActive(false);
        UIState = (PlayerState)0;
        StartCoroutine(Fireball(taget, (int)damge / 5));
    }

    public void HeelButton()
    {
        if (Gold > 5000)
        {
            Gold -= 5000;
            logText.text += "Click The HeelButton\n";
            for (int i = 0; i < 2; ++i)
                heel[i] = (GameObject)Instantiate(HeelParticle[i], playerCollder.bounds.center, Quaternion.identity);

            EnemyUIImage.gameObject.SetActive(false);
            Buttons[2].SetActive(false);
            Buttons[3].SetActive(false);
            BeckButton.SetActive(false);
            UIState = (PlayerState)0;

            HeelCoolTimeImage[0].gameObject.SetActive(false);
            HeelCoolTimeImage[1].gameObject.SetActive(true);
            HeelCoolTimeText.gameObject.SetActive(true);
            HeelCoolTimeNumber = 20;
            HeelCoolTimeText.text = HeelCoolTimeNumber.ToString();
            StartCoroutine(Heeling());
            StartCoroutine(HeelingCoolTime());
        }
        else
        {
            logText.text += "haven't Gold\n";
        }
    }
    IEnumerator HeelingCoolTime()
    {
        while (HeelCoolTimeNumber>0)
        {
            HeelCoolTimeNumber -= 1;
            HeelCoolTimeText.text = HeelCoolTimeNumber.ToString();
            HeelCoolTimeImage[1].fillAmount = (float)(20-HeelCoolTimeNumber) / 20;
            yield return new WaitForSeconds(1f);
        }
        HeelCoolTimeImage[0].gameObject.SetActive(true);
        HeelCoolTimeImage[1].gameObject.SetActive(false);
        HeelCoolTimeText.gameObject.SetActive(false);

    }

    IEnumerator Heeling()
    {
        int count = 0;
        while (count < 10)
        {
            PlayerHp += 2;
            ++count;
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 2; ++i)
        {
            if (heel[i] != null)
            {
                Destroy(heel[i]);
                heel[i] = null;
            }
        }
    }

    IEnumerator Fireball(GameObject tagetObject,int damge)
    {
        GameObject fireball = (GameObject)Instantiate(FireParticle, playerCollder.bounds.center, Quaternion.identity);
        Collider tagetCollider = tagetObject.GetComponent<Collider>();
        while (fireball.transform.position != tagetCollider.bounds.center)
        {
            fireball.transform.position=Vector3.MoveTowards( fireball.transform.position, tagetCollider.bounds.center, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        EnemyInfo info = EnemyTable[tagetObject];
        info.HP -= damge;
        if (info.HP <= 0)
            Land += 1;
        //Vector3 tr = EnemyHpBarImage[info.number].transform.position+new Vector3(0, (EnemyHpBarImage[info.number].fillAmount*100)-50, 0);

        GameObject EventBar = (GameObject)Instantiate(EffectBar, EnemyHpBarImage[info.number].transform.position, Quaternion.identity);
        Image EventBarImage = EventBar.GetComponent<Image>();
        EventBarImage.fillAmount = (float)damge / 100;
        EventBarImage.rectTransform.parent = EnemyHpBarImage[info.number].transform.parent.transform;
        EventBarImage.rectTransform.localScale = new Vector3(1, 1, 1);
        EventBarImage.rectTransform.anchoredPosition3D += new Vector3(info.HP, 0, 0);

        GameObject explosionParticle = (GameObject)Instantiate(ExplosionParticle, fireball.transform.position, Quaternion.identity);
        StartCoroutine(explosion(EventBarImage, explosionParticle));
        Destroy(fireball);
    }

    IEnumerator explosion(Image tagetObject, GameObject explosion)
    {
        while (tagetObject.fillAmount > 0)
        {
            tagetObject.fillAmount -= 0.002f;
            yield return new WaitForEndOfFrame();
        }
       Destroy(explosion);
       Destroy(tagetObject.gameObject);
    }


    public void FrendsButton()
    {
        if (Gold > 50000)
        {
            Gold -= 50000;
            Land += 1;
            logText.text += "Click The FrendsButton\n";
            EnemyInfo info = EnemyTable[taget];
            frends[info.number] = true;
            Sprite temp = EnemyHpBarImage[info.number].sprite;
            EnemyHpBarImage[info.number].sprite = PlayerHpBarImage.sprite;

            EnemyUIImage.gameObject.SetActive(false);
            BeckButton.SetActive(false);
            Buttons[0].SetActive(false);
            Buttons[1].SetActive(false);
            FrendsCoolTimeImage[0].gameObject.SetActive(false);
            FrendsCoolTimeImage[1].gameObject.SetActive(true);
            FrendsCoolTimeText.gameObject.SetActive(true);
            FrendsCoolTimeNumber = 100;
            FrendsCoolTimeText.text = FrendsCoolTimeNumber.ToString();
            UIState = (PlayerState)0;
            StartCoroutine(FrendsCoolTime(info, temp));
        }
        else
        {
            logText.text += "haven't Gold\n";
        }
    }

    IEnumerator FrendsCoolTime(EnemyInfo info, Sprite temp)
    {
        while (FrendsCoolTimeNumber > 0)
        {
            if (FrendsCoolTimeNumber ==100- 20)
            {
                frends[info.number] = false;
                EnemyHpBarImage[info.number].sprite = temp;
                Land -= 1;
            }

            FrendsCoolTimeNumber -= 1;
            FrendsCoolTimeText.text = FrendsCoolTimeNumber.ToString();
            FrendsCoolTimeImage[1].fillAmount = (float)(100 - FrendsCoolTimeNumber) / 100;
            yield return new WaitForSeconds(1f);
        }
        FrendsCoolTimeImage[0].gameObject.SetActive(true);
        FrendsCoolTimeImage[1].gameObject.SetActive(false);
        FrendsCoolTimeText.gameObject.SetActive(false);

    }


    IEnumerator Land1AI(int EnmeyNumber,int CoolTime)
    {
        float AIFireBalltime=0;
        while (EnemyTable[Enemy[EnmeyNumber]].HP > 0)
        {
            if (AIFireBalltime > CoolTime && gameState == (GameState)1)
            {
                if(!frends[EnmeyNumber])
                    StartCoroutine(AIFireball(Enemy[EnmeyNumber], EnmeyNumber));
                AIFireBalltime = 0;
            }
            AIFireBalltime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator AIFireball(GameObject Enemy,int number)
    {
        logText.text += "AI" + Enemy + "Atteck Player\n";
        Collider AICollider = Enemy.GetComponent<Collider>();
        GameObject fireball = (GameObject)Instantiate(FireParticle, AICollider.bounds.center, Quaternion.identity);
        while (fireball.transform.position != playerCollder.bounds.center)
        {
            fireball.transform.position = Vector3.MoveTowards(fireball.transform.position, playerCollder.bounds.center, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        PlayerHp -= 10; 
        GameObject explosionParticle = (GameObject)Instantiate(ExplosionParticle, fireball.transform.position, Quaternion.identity);
        StartCoroutine(DetroyTime(5f, explosionParticle));
        Destroy(fireball);
    }
    IEnumerator DetroyTime(float time, GameObject objects)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(objects);
    }

    IEnumerator StartIntro()
    {
        Vector3 temp;
        float timeStep = 0;
        while (Slider[0].rectTransform.anchoredPosition.y <= 600f)
        {
            temp = Slider[0].rectTransform.anchoredPosition;
            temp.y += 30;
            Slider[0].rectTransform.anchoredPosition = temp;

            temp = Slider[1].rectTransform.anchoredPosition;
            temp.y -= 30;
            Slider[1].rectTransform.anchoredPosition = temp;
            //Slider[0].rectTransform.anchoredPosition = Vector3.MoveTowards(new Vector3(0f, 600f, 0f), Slider[0].rectTransform.anchoredPosition,  Time.deltaTime);
            //Slider[1].rectTransform.anchoredPosition = Vector3.MoveTowards(new Vector3(0f, -600f, 0f),Slider[1].rectTransform.anchoredPosition, Time.deltaTime);

            timeStep += 0.2f;
            yield return new WaitForEndOfFrame();
        }

        EnemyUIImage.gameObject.SetActive(true);
        EnemyUIImage.sprite = Startintro;

        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                EnemyUIImage.gameObject.SetActive(false);
                Time.timeScale = 1;
                gameState = (GameState)1;
                break;
            }
            yield return new WaitForEndOfFrame();
        }


    }






}
