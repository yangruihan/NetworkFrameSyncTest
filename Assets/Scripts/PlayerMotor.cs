using UnityEngine;
using System.Collections;

namespace Ruihanyang.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 5f;

        private Vector2 targetPosition;

        private Rigidbody2D rigid;

        void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            rigid.MovePosition(Vector3.Lerp(transform.position, targetPosition, 1 / moveSpeed * Time.deltaTime));
        }

        public void Move(Vector3 _direction)
        {
            targetPosition = transform.position + _direction;
        }
    }
}
