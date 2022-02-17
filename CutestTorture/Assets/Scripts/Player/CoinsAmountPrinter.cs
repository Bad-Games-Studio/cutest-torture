using UnityEngine;

namespace Player
{
    public class CoinsAmountPrinter : MonoBehaviour
    {
        private void OnEnable()
        {
            CoinPickup.OnCoinPickupEvent += PrintAmountOfCoins;
        }

        private void OnDisable()
        {
            CoinPickup.OnCoinPickupEvent -= PrintAmountOfCoins;
        }

        private static void PrintAmountOfCoins(int amount)
        {
            Debug.Log(amount);
        }
    }
}
