using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireBall
{
    //콜라이더 핸들러 인터페이스
    public interface ICollisionHandler
    {        
        void HandleCollision(GameObject obj, Collision c);
    }

    public class FireBallCollder : MonoBehaviour
    {
        public ICollisionHandler CollisionHandler; //인터페이스

        public void OnCollisionEnter(Collision col) //충돌트리거에 들어갔을대 알려줌
        {
            CollisionHandler.HandleCollision(gameObject, col);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
