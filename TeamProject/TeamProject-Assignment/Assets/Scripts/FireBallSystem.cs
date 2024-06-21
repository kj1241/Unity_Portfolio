using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireBall
{
    public delegate void FireCollisionDelegate(FireBallSystem script, Vector3 pos); //파이어볼 콜라이더 델리게이트
   
    public class FireBallSystem : SkillScript, ICollisionHandler
    {

        [Header("FireBallSystem")]
        [Tooltip("충돌 및 물리에 사용될 개체")]
        public GameObject ColliderObject;

        [Tooltip("충돌시 재생되는 소리")]
        public AudioSource CollisionSound;

        [Tooltip("충돌시 재생하는 파티클 시스템")]
        public ParticleSystem ExplosionParticleSystem;

        [Tooltip("충돌시 폭발 반경")]
        public float ExplosionRadius = 0.0f;

        [Tooltip("충돌시 폭발하는 힘")]
        public float ExplosionForce = 0.0f;

        [Tooltip("사전에 시전 에니메이션이 있을겨우 전송 지연")]
        public float ColliderDelay = 0.0f;

        [Tooltip("충돌의 속도")]
        public float ColliderSpeed = 0.0f;

        [Tooltip("콜라이더의 진행 방향")]
        public Vector3 Direction = Vector3.forward;

        [Tooltip("콜라이더가 충돌 할 수 있는 레이어")]
        public LayerMask CollisionLayers = Physics.AllLayers;

        [Tooltip("충돌시 파괴되는 파티클 시스템")]
        public ParticleSystem[] DestroyParticleSystemsOnCollision;

        [HideInInspector]
        public FireCollisionDelegate CollisionDelegate; //델리게이트

        private bool collided=false;

        private IEnumerator PhysicsTransform() //방향 각도 
        {
            yield return new WaitForSeconds(ColliderDelay); //지연시간 후에 시작

            Vector3 dir = Direction * ColliderSpeed; //방향
            dir = ColliderObject.transform.rotation * dir;
            ColliderObject.GetComponent<Rigidbody>().velocity = dir; //한번뿐이니 괜찮음
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            StartCoroutine(PhysicsTransform()); //콜라이더 이후 딜레이
        }

        public void HandleCollision(GameObject obj, Collision c) //인터페이스 상속받은 콜라이더 충돌시
        {
            if (collided) //충돌중이라면
            {
                return; //충돌 리턴
            }

            collided = true;
            Stop(); //멈추고

            // 파티클 시스템 파괴
            if (DestroyParticleSystemsOnCollision != null)
            {
                foreach (ParticleSystem p in DestroyParticleSystemsOnCollision)
                {
                    GameObject.Destroy(p, 0.01f);// 파티클 제거
                }
            }

            if (CollisionSound != null)
            {
                CollisionSound.Play(); //충돌 소리 재생
            }

            if (ExplosionParticleSystem && ExplosionParticleSystem.gameObject.activeSelf != false && c.contacts.Length != 0)
            {
                ExplosionParticleSystem.transform.position = c.contacts[0].point;
                ExplosionParticleSystem.Play();
                SkillScript.CreateExplosion(c.contacts[0].point, ExplosionRadius, ExplosionForce); //폭발
                if (CollisionDelegate != null) //델리게이트가 있다면
                {
                    CollisionDelegate(this, c.contacts[0].point); //전달
                }
            }
        }
    }
}