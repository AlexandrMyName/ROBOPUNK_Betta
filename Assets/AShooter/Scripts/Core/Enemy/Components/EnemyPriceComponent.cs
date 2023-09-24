using Abstracts;
using UnityEngine;


namespace Core
{


    public class EnemyPriceComponent : IEnemyPrice
    {

        private int _goldDropRate;


        public EnemyPriceComponent(int goldDropRate)
        {
            _goldDropRate = goldDropRate;
        }


        public (bool, int) CheckingForGold()
        {
            bool goldAvailability;
            int goldValue = 0;

            if (goldAvailability = (UnityEngine.Random.Range(1, 100) <= _goldDropRate))
            {
                goldValue = UnityEngine.Random.Range(1, 10);
            }

            return (goldAvailability, goldValue);
        }


        public float GetExperienceValue()
        {
            return Random.Range(5f, 10f);
        }


    }
}