using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireBall
{
    [System.Serializable]
    public class IntensityMaxRange
    {
        public float min = 0.0f;
        public float max = 1.0f;
    }

    //페이드 인 페이드 아웃 효과 
    public class LightFade : MonoBehaviour
    {

        [Header("Move")]
        [Tooltip("움직임에 대한 시드/ 움직임이 없으면 0")]
        public float Seed = 100.0f;

        [Tooltip("광도에 대한 제곱값")]
        public float IntensityModifier = 2.0f;

        [Tooltip("광도의 범위")]
        public IntensityMaxRange IntensityMax = new IntensityMaxRange();

        private Light firePointLight;
        private float lightIntensity;
        private float seed;
        private SkillScript fireSkill;
        private float firePointLightY; //높이값

        // 1프레임
        private void Awake()
        {
            firePointLight = gameObject.GetComponentInChildren<Light>(); //자식 오브젝트의 빛가저오기
            if (firePointLight != null) //존재하면
            {
                lightIntensity = firePointLight.intensity; // 광도 값 가저오기
                firePointLight.intensity = 0.0f; //시작값 0
                firePointLightY = firePointLight.gameObject.transform.position.y; //높이값 가저오기
            }
            seed = UnityEngine.Random.value * Seed; // 랜덤값
            fireSkill = gameObject.GetComponent<SkillScript>();
        }

        // 2프레임
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (firePointLight == null) // 없다면 리턴
                return;
            if (seed != 0)
            {
                bool isIntensity = true;
                float inputIntensity = 1.0f;

                if (fireSkill != null) //존재한다면
                {
                    if (fireSkill.SkillState == SkillStatePipLine.end) //정지상태라면
                    {
                        isIntensity = false;
                        firePointLight.intensity = Mathf.Lerp(firePointLight.intensity, 0.0f, fireSkill.StopCurrenDelayTime);//러프값으로 페이드아웃 적용
                    }
                    else if (fireSkill.SkillState == SkillStatePipLine.start) //시작단계라면
                    {
                        inputIntensity = fireSkill.StartCurrenDelayTime; //선딜레이가 밝기의 상태가 된다.(페이드 인)
                    }
                }

                if (isIntensity)
                {
                    float intensity = Mathf.Clamp(IntensityModifier * inputIntensity * Mathf.PerlinNoise(seed + Time.time, seed + 1 + Time.time), IntensityMax.min, IntensityMax.max);
                    firePointLight.intensity = intensity; //불빛 값
                }

                //펄린 노이즈(구름등 난수 생성)
                float timeAcceleration = 1.5f;
                float x = Mathf.PerlinNoise(seed + 0 + Time.time * timeAcceleration, seed + 1 + Time.time * timeAcceleration) - 0.5f; //0,1번
                float y = /*firePointLightY +*/ Mathf.PerlinNoise(seed + 2 + Time.time * timeAcceleration, seed + 3 + Time.time * timeAcceleration) - 0.5f; //2,3번
                float z = Mathf.PerlinNoise(seed + 4 + Time.time * timeAcceleration, seed + 5 + Time.time * timeAcceleration) - 0.5f; //4 ,5 번
                firePointLight.gameObject.transform.localPosition = Vector3.up + new Vector3(x, y, z);
            }
            else if (fireSkill.SkillState == SkillStatePipLine.end) //멈췄다면
            {
                //페이드 아웃
                firePointLight.intensity = Mathf.Lerp(firePointLight.intensity, 0.0f, fireSkill.StopCurrenDelayTime);
            }
            else if (fireSkill.SkillState == SkillStatePipLine.start) //진행중이라면
            {
                // 페이드 인
                firePointLight.intensity = Mathf.Lerp(0.0f, lightIntensity, fireSkill.StartCurrenDelayTime);
            }
        }
    }
}
