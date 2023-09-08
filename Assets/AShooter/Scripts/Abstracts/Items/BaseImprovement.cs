
using UnityEngine;

namespace Abstracts
{
    public abstract class BaseImprovement : MonoBehaviour, IImprovement
    {
        protected ImprovementTime Time;
        protected ImprovementType Type;

        public float Value { get; protected set; }
        public float Timer { get; protected set; }


        public abstract void Improve(IImprovable improvable);

        public ImprovementTime GetImproveTime() => Time;
 
        public ImprovementType GetImproveType() => Type;

        public void Dispose() => Destroy(gameObject);
    }
}
 