using Zenject;
using UnityEngine;
using System.Collections.Generic;
using Core.DTO;
using User;


namespace DI
{
    
    public class WeaponInstaller : MonoInstaller
    {

        [SerializeField] private List<WeaponConfig> _weaponConfigs;


        public override void InstallBindings()
        {
            Container.Bind<List<WeaponConfig>>().FromInstance(_weaponConfigs);
            Container.Bind<WeaponState>().FromInstance(new WeaponState()).AsCached();
        }
        

    }
}
 
