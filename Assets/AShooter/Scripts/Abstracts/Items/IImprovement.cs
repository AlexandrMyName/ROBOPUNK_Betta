 

namespace Abstracts
{
    public interface IImprovement
    {
        float Value { get; }
        float Timer { get; }

        void Improve(IImprovable improvable);
        ImprovementTime GetImproveTime();
        ImprovementType GetImproveType();


    }
}