using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
using User.Components.Repository;


namespace AShooter.Scripts.Core.Player.Components
{
    
    public sealed class PlayerStatsComponent : IPlayerStats, IDisposable
    {

        public int Score { get; private set; }
        
        public string Name { get; private set; }
        
        public int Money { get; set; }
        
        public float Experience { get; set; }

        public float MetaExperience { get; set; }

        public float BaseHealthMultiplier { get; set; }
        
        public float BaseMoveSpeedMultiplier { get; set; }
        
        public float BaseDamageMultiplier { get; set; }
        
        public float BaseShieldCapacityMultiplier { get; set; }
        
        public float BaseDashDistanceMultiplier { get; set; }
        
        public float BaseShootSpeedMultiplier { get; set; }


        private List<IDisposable> _disposables = new();

        private IRepository _repository = new Repository();


        public PlayerStatsComponent(
            string name, int money, float experience, int score, 
            float metaExperience, float baseHealth, float baseDamage, float baseMoveSpeed, 
            float baseShieldCapacity, float baseDashDistance, float baseShootSpeed)
        {
            Name = name;
            Money = money;
            Experience = experience;
            Score = score;
            MetaExperience = metaExperience;
            BaseHealthMultiplier = baseHealth;
            BaseDamageMultiplier = baseDamage;
            BaseMoveSpeedMultiplier = baseMoveSpeed;
            BaseShieldCapacityMultiplier = baseShieldCapacity;
            BaseDashDistanceMultiplier = baseDashDistance;
            BaseShootSpeedMultiplier = baseShootSpeed;
        }
        
        
        public PlayerStatsComponent(string name, int money, float experience, int score, 
            ReactiveProperty<int> moneyReactive, ReactiveProperty<float> experienceReactive,
            float metaExperience, float baseHealth, float baseDamage, float baseMoveSpeed, 
            float baseShieldCapacity, float baseDashDistance, float baseShootSpeed)
        {
            Name = name;
            Money = money;
            Experience = experience;
            Score = score;
            MetaExperience = metaExperience;
            BaseHealthMultiplier = baseHealth;
            BaseDamageMultiplier = baseDamage;
            BaseMoveSpeedMultiplier = baseMoveSpeed;
            BaseShieldCapacityMultiplier = baseShieldCapacity;
            BaseDashDistanceMultiplier = baseDashDistance;
            BaseShootSpeedMultiplier = baseShootSpeed;
            
            moneyReactive.Subscribe(val =>
            {
                Money = val;
                CalculateScore(val);
                _repository.Save(this);
            }).AddTo(_disposables);
            
            experienceReactive.Subscribe(val =>
            {
                Experience = val;
                CalculateScore((int)val);
                _repository.Save(this);
            }).AddTo(_disposables);
        }


        public void CalculateScore(int amount)
        {
            Score += amount;
        }


        public void AddMetaExperience(float val)
        {
            MetaExperience += val;
            _repository.Save(this);
        }

        
        public bool TryDeductMetaExperience(float amount)
        {
            var isOperationSuccess = false;
            if (MetaExperience >= amount)
            {
                MetaExperience -= amount;
                isOperationSuccess = true;
            }

            return isOperationSuccess;
        }

        
        public bool TryDeductGold(int amount)
        {
            var isOperationSuccess = false;
            if (Money >= amount)
            {
                Money -= amount;
                isOperationSuccess = true;
            }

            return isOperationSuccess;
        }


        public void ResetScore()
        {
            Score = 0;
        }

        
        public void ResetAllStats()
        {
            throw new NotImplementedException();
        }

        
        public void SaveStatsInRepository()
        {
            _repository.Save(this);
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }
        
        
    }
}