using System;

namespace Abstracts
{
    public interface IImprovement : IDisposable
    {
        float Value { get; }
        float Timer { get; }

        void Improve(IImprovable improvable);
        ImprovementTime GetImproveTime();
        ImprovementType GetImproveType();


    }
}