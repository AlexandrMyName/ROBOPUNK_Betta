using Abstracts;
using User;


namespace Core
{

    public class PlayerHPComponent : IPlayerHP
    {
        public IComponentsStore ComponentsStore { get; }

        public bool _playerAlive { get; set; }
        public float _deathPunchForce { get; set; }

        public PlayerHPComponent(PlayerHPConfig playerHPConfig)
        {
            _deathPunchForce = playerHPConfig.DeathPunchForce;
        }

    }
}