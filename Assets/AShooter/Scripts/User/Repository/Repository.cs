using Abstracts;
using AShooter.Scripts.Core.Player.Components;
using UnityEngine;


namespace User.Components.Repository
{
    
    public class Repository : IRepository
    {
        
        public void Save(IPlayerStats playerStats)
        {
            PlayerPrefs.SetString("TopDown_Name", "Player");
            PlayerPrefs.SetInt("TopDown_Score", playerStats.Score);
            PlayerPrefs.SetInt("TopDown_Money", playerStats.Money);
            PlayerPrefs.SetFloat("TopDown_Experience", playerStats.Experience);
            PlayerPrefs.SetFloat("TopDown_MetaExperience", playerStats.MetaExperience);
            PlayerPrefs.SetFloat("TopDown_BaseHealthMultiplier", playerStats.BaseHealthMultiplier);
            PlayerPrefs.SetFloat("TopDown_BaseDamageMultiplier", playerStats.BaseDamageMultiplier);
            PlayerPrefs.SetFloat("TopDown_BaseMoveSpeedMultiplier", playerStats.BaseMoveSpeedMultiplier);
            PlayerPrefs.SetFloat("TopDown_BaseShieldCapacityMultiplier", playerStats.BaseShieldCapacityMultiplier);
            PlayerPrefs.SetFloat("TopDown_BaseDashDistanceMultiplier", playerStats.BaseDashDistanceMultiplier);
            PlayerPrefs.SetFloat("TopDown_BaseShootSpeedMultiplier", playerStats.BaseShootSpeedMultiplier);
            PlayerPrefs.Save();
        }

        
        public IPlayerStats Load()
        {
            string name = PlayerPrefs.GetString("TopDown_Name", "Player");
            int score = PlayerPrefs.GetInt("TopDown_Score");
            int money = PlayerPrefs.GetInt("TopDown_Money");
            float experience = PlayerPrefs.GetFloat("TopDown_Experience");
            float metaExperience = PlayerPrefs.GetFloat("TopDown_MetaExperience");
            float baseHealth = PlayerPrefs.GetFloat("TopDown_BaseHealthMultiplier");
            float baseDamage = PlayerPrefs.GetFloat("TopDown_BaseDamageMultiplier");
            float baseMoveSpeed = PlayerPrefs.GetFloat("TopDown_BaseMoveSpeedMultiplier");
            float baseShieldCapacity = PlayerPrefs.GetFloat("TopDown_BaseShieldCapacityMultiplier");
            float baseDashDistance = PlayerPrefs.GetFloat("TopDown_BaseDashDistanceMultiplier");
            float baseShootSpeed = PlayerPrefs.GetFloat("TopDown_BaseShootSpeedMultiplier");
            
            return new PlayerStatsComponent(
                name, money, experience, score, 
                metaExperience, baseHealth, baseDamage, baseMoveSpeed, 
                baseShieldCapacity, baseDashDistance, baseShootSpeed);
        }
        
        
    }
}