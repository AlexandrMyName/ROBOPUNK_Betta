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
            PlayerPrefs.Save();
        }

        
        public IPlayerStats Load()
        {
            string name = PlayerPrefs.GetString("TopDown_Name", "Player");
            int score = PlayerPrefs.GetInt("TopDown_Score");
            int money = PlayerPrefs.GetInt("TopDown_Money");
            float experience = PlayerPrefs.GetFloat("TopDown_Experience");

            return new PlayerStatsComponent(name, money, experience, score);
        }
        
        
    }
}