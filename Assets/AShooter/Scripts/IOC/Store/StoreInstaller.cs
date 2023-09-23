using Zenject;
using UnityEngine;
using User;
using User.Presenters;


namespace DI
{

    public class StoreInstaller : MonoInstaller
    {

        [SerializeField] private StoreDataConfig _storeDataConfig;
        [SerializeField] private StorePresenter _storePresenter;


        public override void InstallBindings()
        {
            Container.Bind<StoreDataConfig>().FromInstance(_storeDataConfig).AsCached();
            Container.Bind<StorePresenter>().FromInstance(_storePresenter).AsCached();
        }


    }
}