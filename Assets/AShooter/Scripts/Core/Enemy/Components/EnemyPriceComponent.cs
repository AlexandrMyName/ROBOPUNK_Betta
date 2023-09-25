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


        public float GetExperienceValue()
        {
            return Random.Range(5f, 10f);
        }


        public int GetGoldValue()
        {
            return (Random.Range(1, 100) <= _goldDropRate) ? Random.Range(1, 10) : 0;
        }


    }
}