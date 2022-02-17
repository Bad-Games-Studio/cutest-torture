using UnityEngine;

namespace Player
{
    public class FinishReaching : MonoBehaviour
    {
        public delegate void NotifyReachedFinish();
        public static event NotifyReachedFinish OnFinishReached;

        private void OnTriggerEnter(Collider coin)
        {
            if (coin.gameObject.name != "FinishMark")
            {
                return;
            }
            OnFinishReached?.Invoke();
        }
    }
}
