using Abstracts;
using UnityEngine;
using static User.EnemyConfig;


namespace Core
{


    public class EnemyPriceComponent : IEnemyPrice
    {

        private int _goldDropRate;
        private Range _goldValueRange;
        private Range _experienceValueRanhe;


        public EnemyPriceComponent(int goldDropRate, Range goldValueRange, Range experienceValueRanhe)
        {
            _goldDropRate = goldDropRate;
            _goldValueRange = goldValueRange;
            _experienceValueRanhe = experienceValueRanhe;
        }


        public float GetExperienceValue()
        {
            return Random.Range(_experienceValueRanhe.min, _experienceValueRanhe.max);
        }


        public int GetGoldValue()
        {
            return (Random.Range(1, 100) <= _goldDropRate) ? Random.Range(_goldValueRange.min, _goldValueRange.max) : 0;
        }


    }
}