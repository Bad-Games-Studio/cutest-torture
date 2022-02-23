using Level;
using UnityEngine;

namespace Player
{
    public class FinishReaching : MonoBehaviour
    {
        public delegate void NotifyReachedFinish();
        public static event NotifyReachedFinish OnFinishReached;

        private void OnTriggerEnter(Collider finish)
        {
            if (!finish.TryGetComponent<FinishMarkBehavior>(out _))
            {
                return;
            }
            OnFinishReached?.Invoke();
        }
    }
}
