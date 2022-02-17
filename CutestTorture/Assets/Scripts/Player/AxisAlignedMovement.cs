using UnityEngine;

namespace Player
{
    public class AxisAlignedMovement : MonoBehaviour
    {
        public float maxVelocity;

        private Vector3 _directionX;
        private Vector3 _directionZ;
        private int _horizontalAxis;

        private Rigidbody _rigidBody;
    

        private void Start()
        {
            _directionX = new Vector3(maxVelocity, 0, 0);
            _directionZ = new Vector3(0, 0, maxVelocity);
            _horizontalAxis = 1;
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SwitchAxis();
            }
        }

        private void FixedUpdate()
        {   
            var velocity = GetDirection();
            velocity.y = _rigidBody.velocity.y;
            _rigidBody.velocity = velocity;
        }

        
        private Vector3 GetDirection()
        {
            return _horizontalAxis == 0 ? _directionX : _directionZ;
        }

        private void SwitchAxis()
        {
            _horizontalAxis = (~_horizontalAxis & 1);
        }
    }
}
