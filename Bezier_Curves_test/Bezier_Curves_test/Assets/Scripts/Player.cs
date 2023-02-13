using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //연동
    public GameObject SkillPrefab;
    public GameObject Enemy;

    public Vector3 endPos;

    [Header("skill")]
    public float skillSpeed = 0.5f; //skill 속도
    public int SkillNumberCount = 12;
    public int SkillNumberCountOneShot = 2; // 한번에 발사하는 갯수
    public float SkillCOuntTime = 0.15f;
    Quaternion Rotatation = Quaternion.Euler(new Vector3(0, 0, 0));

    IEnumerator cor=null;

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(moveX, 0f, moveZ).normalized * 0.03f);


        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (cor == null)
            {
                cor = CreateSkill();
                StartCoroutine(cor);
            }
            else
            {
                StopCoroutine(cor);
                StartCoroutine(cor);
            }
        }

    }

    IEnumerator CreateSkill()
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
        cor = null;
    }
}
