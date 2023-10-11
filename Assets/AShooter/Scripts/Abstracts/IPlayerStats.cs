namespace Abstracts
{
    
    public interface IPlayerStats
    {

        int Score { get; }
        
        string Name { get; }

        int Money { get; set; }

        float Experience { get; set; }
        
        void CalculateScore(int amount);

        void ResetScore();


    }
}