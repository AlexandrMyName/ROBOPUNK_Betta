using Zenject;
using UnityEngine;
using User;


namespace DI
{

    public class AbilityInstaller : MonoInstaller
    {

        [SerializeField] private ExplosionAbilityConfig _explosionAbilityConfigs;


        public override void InstallBindings()
        {
            Container.Bind<ExplosionAbilityConfig>().FromInstance(_explosionAbilityConfigs);
        }


    }
}
