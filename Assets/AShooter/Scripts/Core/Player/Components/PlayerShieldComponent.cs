using Abstracts;
using UniRx;


namespace Core
{

    public class PlayerShieldComponent : IShield
    {

        public PlayerShieldComponent(float maxProtection)
        {

            MaxProtection = maxProtection;

            IsActivate = new ReactiveProperty<bool>(false);
            ShieldProccessTime = new ReactiveProperty<float>(0);
        }


        public ReactiveProperty<bool> IsActivate { get; set; }

        public ReactiveProperty<float> ShieldProccessTime { get; set; }

        public float MaxProtection {get; private set;}


        public void RefreshProtection(float maxProtection) => MaxProtection = maxProtection;


        public void SetShield(float maxTime)
        {

            ShieldProccessTime.Value = maxTime;
            IsActivate.Value = true;
        }
        
    }
}