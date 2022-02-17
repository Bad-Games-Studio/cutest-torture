using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform followTarget;

        public float fixedY;
        public Vector3 offset;

        private void Update()
        {
            var newPosition = followTarget.position + offset;
            newPosition.y = fixedY;
            transform.position = newPosition;
        }
    }
}
