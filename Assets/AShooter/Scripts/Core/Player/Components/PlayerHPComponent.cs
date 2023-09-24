using Abstracts;
using UniRx;
using User;


namespace Core
{

    public class PlayerHPComponent : IPlayerHP
    {

        public PlayerHPComponent(PlayerHPConfig playerHPConfig)
        {

            PunchForce = playerHPConfig.DeathPunchForce;
            IsAlive = new(true);
            //IsAlive.SkipLatestValueOnSubscribe();
        }
         

        public ReactiveProperty<bool> IsAlive { get; set; }
        public float PunchForce { get; private set; }

    }
}