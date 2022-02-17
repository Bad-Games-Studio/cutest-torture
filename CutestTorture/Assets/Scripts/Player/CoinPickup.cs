using UnityEngine;

namespace Player
{
    public class CoinPickup : MonoBehaviour
    {
        private int _nCoins;
        public delegate void NotifyCoinsAmountUpdate(int amount);
        public static event NotifyCoinsAmountUpdate OnCoinPickupEvent;
    
        private void Start()
        {
            _nCoins = 0;
        }

        private void OnTriggerEnter(Collider coin)
        {
            if (coin.gameObject.name != "Coin(Clone)") // ehh...
            {
                return;
            }
            Destroy(coin.gameObject);
            _nCoins += 1;
            OnCoinPickupEvent?.Invoke(_nCoins);
        }
    }
}
