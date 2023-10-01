using Abstracts;
using UniRx;

namespace Core
{

    public class PlayerShieldComponent : IShield
    {

        public PlayerShieldComponent(float maxProtection, float maxRegenerationTime)
        {

            MaxProtection = maxProtection;
            MaxRegenerationSeconds = maxRegenerationTime;
            IsRegeneration = new ReactiveProperty<bool>(false);
        }


        public float MaxProtection {get; private set;}

        public float MaxRegenerationSeconds { get; private set; }

        public ReactiveProperty<bool> IsRegeneration {get; private set;}


        public void SetMaxProtection(float maxProtection) => MaxProtection = maxProtection;

        public void SetRegenerationTime(float seconds) => MaxRegenerationSeconds = seconds;
       
    }
}