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

        private bool _finished;

        private void Start()
        {
            _finished = false;
            _directionX = new Vector3(maxVelocity, 0, 0);
            _directionZ = new Vector3(0, 0, maxVelocity);
            _horizontalAxis = 1;
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            FinishReaching.OnFinishReached += StopOnFinish;
        }

        private void OnDisable()
        {
            FinishReaching.OnFinishReached -= StopOnFinish;
            _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, 0);
            Debug.Log("Da playa is ded.");
        }

        private void StopOnFinish()
        {
            _finished = true;
            Debug.Log("Da playa is WINNING.");
        }

        private void Update()
        {
            if (_rigidBody.position.y < 0)
            {
                gameObject.SetActive(false);
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                SwitchAxis();
            }
        }

        private void FixedUpdate()
        {
            if (_finished)
            {
                _rigidBody.velocity = Vector3.zero;
                return;
            }
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
