using Abstracts;
using UniRx;


namespace Core
{

    public class PlayerShieldComponent : IShield
    {

        public PlayerShieldComponent(float maxConfigurationTime)
        {

            MaxConfigurationTime = maxConfigurationTime;

            IsActivate = new ReactiveProperty<bool>(false);
            ShieldProccessTime = new ReactiveProperty<float>(0);
        }


        public ReactiveProperty<bool> IsActivate { get; set; }

        public ReactiveProperty<float> ShieldProccessTime { get; set; }


        public float MaxConfigurationTime {get; private set;}


        public void SetShield(float maxTime)
        {

            ShieldProccessTime.Value = maxTime;
            IsActivate.Value = true;
        }
        
    }
}