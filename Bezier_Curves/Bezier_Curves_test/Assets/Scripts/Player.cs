using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Player : MonoBehaviour
{
    //연동
    public GameObject Enemy;

    public Vector3 endPos;

    [Header("skill")]
    public float skillSpeed = 0.5f; //skill 속도
    public int SkillNumberCount = 12;
    public int SkillNumberCountOneShot = 2; // 한번에 발사하는 갯수
    public float SkillCOuntTime = 0.15f;
    Quaternion Rotatation = Quaternion.Euler(new Vector3(0, 0, 0));

    const int Skillcount =2;

    class Skill
    {
        public Image UICoolDownImage;
        public TMPro.TMP_Text UICoolDownCount;
        public int SkillDelayCount = 0;
        public bool isSkillCooldown = false;
    }
    public GameObject[] SkillPrefab = new GameObject[Skillcount];
    public Image[] UICoolDownImage = new Image[Skillcount];
    public TMPro.TMP_Text[] UICoolDownCount = new TMPro.TMP_Text[Skillcount];
    Skill[] skills = new Skill[Skillcount];

    void Start()
    {
        for (int i = 0; i < Skillcount; ++i)
        {
            skills[i] = new Skill(); //맞다 c#에서는 배열로 사용하면 공간창출해야됨
            skills[i].UICoolDownImage = UICoolDownImage[i];
            skills[i].UICoolDownImage.fillAmount = 0;
            skills[i].UICoolDownCount = UICoolDownCount[i];
            skills[i].UICoolDownCount.enabled = false;
        }
    }

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(moveX, 0f, moveZ).normalized * 0.035f);


        if (Input.GetKeyDown(KeyCode.Q) && !skills[0].isSkillCooldown)
        {
            skills[0].isSkillCooldown = true;
            StartCoroutine(CreateSkill(SkillPrefab[0], skills[0]));
        }

        if (Input.GetKeyDown(KeyCode.E) && !skills[1].isSkillCooldown)
        {
            skills[1].isSkillCooldown = true;
            StartCoroutine(CreateSkill(SkillPrefab[1], skills[1]));
        }

    }

    IEnumerator CreateSkill(GameObject SkillPrefab,Skill skills )
    {
        
        int _skillCount = 0;

        while (_skillCount <= (int)SkillNumberCount / SkillNumberCountOneShot)
        {
            for (int i = 0; i < SkillNumberCountOneShot; ++i) //정해진 갯수만큼
            {
                float angle = _skillCount * 360 * SkillNumberCountOneShot / SkillNumberCount; //x 축 회전을 위해서 회전   
                Rotatation = Quaternion.Euler(new Vector3(angle, 0, 0)); //x 축 회전을 사원수 값 만들기
                GameObject _missile = Instantiate(SkillPrefab, this.transform.position, this.transform.rotation); //프리팹 생성
                _missile.GetComponent<Skill_BezierMissile>().Init(Enemy, Rotatation); //초기값 

                _skillCount++;

                yield return new WaitForSeconds(SkillCOuntTime);
            }
        }
        skills.UICoolDownImage.fillAmount = 1;

        StartCoroutine(SkillCooldown(skills));
    }

    IEnumerator SkillCooldown(Skill skills)
    {
        skills.SkillDelayCount = 5;
        skills.UICoolDownImage.enabled = true;
        while (skills.SkillDelayCount >= 0)
        {
            skills.SkillDelayCount--;
            skills.UICoolDownCount.text = skills.SkillDelayCount.ToString();
            skills.UICoolDownImage.fillAmount = (float)skills.SkillDelayCount / 5;
            if (skills.SkillDelayCount == 0) 
                break; // 바로나가기위해

            yield return new WaitForSecondsRealtime(1f);
        }
        skills.isSkillCooldown = false;
        skills.UICoolDownImage.enabled = false;
    }

}
