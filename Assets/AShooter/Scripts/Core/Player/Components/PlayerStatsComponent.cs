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


        private List<IDisposable> _disposables = new();

        private IRepository _repository = new Repository();


        public PlayerStatsComponent(string name, int money, float experience, int score)
        {
            Name = name;
            Money = money;
            Experience = experience;
            Score = score;
        }
        
        
        public PlayerStatsComponent(string name, int money, float experience, int score, 
            ReactiveProperty<int> moneyReactive, ReactiveProperty<float> experienceReactive)
        {
            Name = name;
            Money = money;
            Experience = experience;
            Score = score;
            
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

        
        public void ResetScore()
        {
            Score = 0;
        }


        public void Dispose()
        {
            _disposables.ForEach(d => d.Dispose());
        }
        
        
    }
}