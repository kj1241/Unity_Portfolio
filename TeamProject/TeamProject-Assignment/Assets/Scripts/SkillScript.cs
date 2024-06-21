using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireBall
{

    [System.Serializable]
    public struct RangeOfIntegers
    {
        public int Minimum;
        public int Maximum;
    }

    [System.Serializable]
    public struct RangeOfFloats
    {
        public float Minimum;
        public float Maximum;
    }

    public enum SkillStatePipLine
    {
        error = -1,
        start = 0,
        play = 1,
        end = 2
    }

    public class SkillScript : MonoBehaviour
    {
        [Tooltip("스크립트가 시작될때 재생되는 오디오 소스")]
        public AudioSource AudioSource;

        [Tooltip("선 딜레이")]
        public float StartTime = 1.0f;

        [Tooltip("후 딜레이")]
        public float StopTime = 3.0f;

        [Tooltip("총 에니메이션 시간")]
        public float Duration = 2.0f;

        [Tooltip("폭발에서 생성할 힘의 크기")]
        public float ForceAmount;

        [Tooltip("힘의 반지름")]
        public float ForceRadius;

        [Tooltip("개체가 투사체")]
        public bool IsProjectile;

        [Tooltip("수동으로 시작해야되며 재생되지 않는 파티클 시스템")]
        public ParticleSystem[] ManualParticleSystems;

        private float startTimeMultiplier; //시작 시간 계수
        private float startTimeCurrent=0.0f; // 시작까지 걸리는 현재 시간 = 선딜레이

        private float stopTimeMultiplier; //정지 시간 계수
        private float stopTimeCurrent=0.0f; // 정지까지 걸리는 현재 시간 = 후딜레이

        public SkillStatePipLine SkillState
        {
            get;
            set;
        }
        public float StartCurrenDelayTime
        {
            get;
            private set;
        }

        public float StopCurrenDelayTime
        {
            get;
            private set;
        }


        protected virtual void Awake()
        {
            SkillState = SkillStatePipLine.start; //시작
            //Starting = true; //시작했는가?
            int fireLayer = UnityEngine.LayerMask.NameToLayer("FireLayer");
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (AudioSource != null) //오디오 소스가 존재하면
            {
                AudioSource.Play(); //시작
            }

            // 프레임에 대해서 사용하기 위해서 계수를 미리 제작
            stopTimeMultiplier = 1.0f / StopTime; //계수
            startTimeMultiplier = 1.0f / StartTime; //계수

            // 이효과가 폭발하면 적용
            CreateExplosion(gameObject.transform.position, ForceRadius, ForceAmount);

            // 파티클 시스템 시작
            StartParticleSystems();

            //콜라이더 핸들 인터페이스에 전달
            //충돌 이벤트연결
            ICollisionHandler handler = (this as ICollisionHandler); //충돌핸들 인터페이스가져오고
            if (handler != null) //충돌 핸들 인터페이스가 존재한다면
            {
                FireBallCollder collision = GetComponentInChildren<FireBallCollder>();
                if (collision != null)
                {
                    collision.CollisionHandler = handler; //자식 오브젝트에 인터페이스 연동
                }
            }

        }

        // 프레임에 따른 시간으로 관리함
        protected virtual void Update()
        {
            Duration -= Time.deltaTime; // 총 에니메이션 시간
            if (SkillState==SkillStatePipLine.end) //정지라면
            {
                stopTimeCurrent += Time.deltaTime; //후 딜레이를 향해 간다.
                if (stopTimeCurrent < StopTime) //후 딜레이가 아니라면
                {
                    StopCurrenDelayTime = stopTimeCurrent * stopTimeMultiplier; //계수를 곱해서 알려줌
                }
            }
            else if (SkillState==SkillStatePipLine.start) // 스킬 상태가 진행중이라면
            {
                startTimeCurrent += Time.deltaTime;
                if (startTimeCurrent < StartTime) //선 딜레이 시간 안됬으면 
                {
                    StartCurrenDelayTime = startTimeCurrent * startTimeMultiplier; 
                }
                else
                {
                    SkillState = SkillStatePipLine.play; //플레이 상태로 변환
                }
            }
            else if (Duration <= 0.0f) //지속시간이 다됬다면 
            {
                Stop();//정지
            }
        }


        private IEnumerator CleanupEverything()
        {
            // 에니메이션과 그래픽이 확인되기 위해서 2초 추가
            yield return new WaitForSeconds(StopTime + 2.0f);
            GameObject.Destroy(gameObject); // 제거
        }

        //파티클 시스템 시작
        private void StartParticleSystems()
        {
            //파티클 시스템 찾기(자식오브젝트에 있는)
            foreach (ParticleSystem p in gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                if (ManualParticleSystems == null || ManualParticleSystems.Length == 0 || System.Array.IndexOf(ManualParticleSystems, p) < 0)
                {
                    if (p.startDelay == 0.0f)
                    {
                        //변경될 수 있음으로 다음프레이까지 대기
                        p.startDelay = 0.01f;
                    }
                    p.Play(); //파티클 시스템 시작하기
                }
            }
        }

        //폭발 생성 위치 반경 힘
        public static void CreateExplosion(Vector3 pos, float radius, float force)
        {
            if (force <= 0.0f || radius <= 0.0f) //힘과 반경이 0보다 작으면 리턴
            {
                return;
            }

            // 주변에 있는 콜라이더를 추출하여 가져오기 (몬스터를 때리면 주변에 있는 몬스터들도 찾아올수 있음)
            Collider[] objects = UnityEngine.Physics.OverlapSphere(pos, radius);
            foreach (Collider h in objects) //모든 콜라이더에게 적용시킴
            {
                Rigidbody r = h.GetComponent<Rigidbody>(); //리지드 바디를 얻어오고
                if (r != null)
                {
                    r.AddExplosionForce(force, pos, radius); //백터반경의 힘을 추가한다.
                }
            }
        }

        public virtual void Stop()
        {
            if (SkillState == SkillStatePipLine.end) //이미 중지단계라면 여기 들어오면 안되니 리턴
            {
                return;
            }
            SkillState = SkillStatePipLine.end; //중지 단계로 만들기

            //파티클 시스템 정리 (중지단계 한번만 사용됨으로 괜찮음)
            foreach (ParticleSystem p in gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }

            StartCoroutine(CleanupEverything()); //지운다.
        }

      
    }
}

