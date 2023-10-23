namespace Abstracts
{
    
    public interface IPlayerStats
    {

        int Score { get; }
        
        string Name { get; }

        int Money { get; set; }

        float Experience { get; set; }

        float MetaExperience { get; set; }

        float BaseHealthMultiplier { get; set; }

        float BaseMoveSpeedMultiplier { get; set; }

        float BaseDamageMultiplier { get; set; }

        float BaseShieldCapacityMultiplier { get; set; }

        float BaseDashDistanceMultiplier { get; set; }

        float BaseShootSpeedMultiplier { get; set; }

        void CalculateScore(int amount);
        
        void AddMetaExperience(float amount);

        bool TryDeductMetaExperience(float amount);

        bool TryDeductGold(int amount);

        void ResetScore();

        void ResetAllStats();

        void SaveStatsInRepository();


    }
}