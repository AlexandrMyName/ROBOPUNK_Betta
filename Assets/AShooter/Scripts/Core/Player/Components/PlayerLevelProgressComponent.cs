using Abstracts;


namespace Core.Components
{

    public sealed class PlayerLevelProgressComponent : ILevelProgress
    {

        public float RequiredExperienceForNextLevel { get; private set; }
        public float ProgressRate { get; private set; }

        public PlayerLevelProgressComponent(float requiredExperienceForNextLevel, float progressRate)
        {
            RequiredExperienceForNextLevel = requiredExperienceForNextLevel;
            ProgressRate = progressRate;
        }

    }
}