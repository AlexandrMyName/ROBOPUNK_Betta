using Abstracts;
using UnityEngine;
using User;
using Zenject;


namespace DI
{
    
    public class UIInstaller : MonoInstaller
    {

        [SerializeField] private Transform _containerForUI;

    [SerializeField] private GameObject _deathViewPrefab;
    [SerializeField] private GameObject _dashViewPrefab;
    [SerializeField] private GameObject _goldWalletViewPrefab;
    [SerializeField] private GameObject _experienceViewPrefab;
    [SerializeField] private GameObject _shieldViewPrefab;
    [SerializeField] private GameObject _playerHpViewPrefab;
    [SerializeField] private GameObject _interactViewPrefab;
    [SerializeField] private GameObject _weaponAbilityViewPrefab;

    [Space(10)]
    [Header("Menu:")]
    [SerializeField] private GameObject _pauseMenuViewPrefab;
    [SerializeField] private GameObject _storeMenuViewPrefab;
    [SerializeField] private GameObject _winView_Prefab;


    public override void InstallBindings()
    {
        
        Container
            .Bind<IDeathView>()
            .FromInstance(InstantiateView<IDeathView>(_deathViewPrefab))
            .AsCached();
        
        Container
            .Bind<IPauseMenuView>()
            .FromInstance(InstantiateView<IPauseMenuView>(_pauseMenuViewPrefab))
            .AsCached();

        Container
            .Bind<IStoreView>()
            .FromInstance(InstantiateView<IStoreView>(_storeMenuViewPrefab))
            .AsCached();

        Container
                .Bind<IDashView>()
                .FromInstance(InstantiateView<IDashView>(_dashViewPrefab))
                .AsCached();
        
        Container
            .Bind<IExperienceView>()
            .FromInstance(InstantiateView<IExperienceView>(_experienceViewPrefab))
            .AsCached();
        
        Container
            .Bind<IGoldWalletView>()
            .FromInstance(InstantiateView<IGoldWalletView>(_goldWalletViewPrefab))
            .AsCached();
        
        Container
            .Bind<IHealthView>()
            .FromInstance(InstantiateView<IHealthView>(_playerHpViewPrefab))
            .AsCached();
        

            var weaponAbilityPresenter = InstantiateView<WeaponAbilityPresenter>(_weaponAbilityViewPrefab);
            IWeaponAbilityView weaponAbilityView = weaponAbilityPresenter.GetComponent<WeaponAbilityView>();

            Container.Bind<IWeaponAbilityView>()
                .FromInstance(weaponAbilityView)
                .AsCached();

            Container
                .Bind<IShieldView>()
                .FromInstance(InstantiateView<IShieldView>(_shieldViewPrefab))
                .AsCached();

            Container.Bind<WeaponAbilityPresenter>().FromInstance(weaponAbilityPresenter).AsCached();

            Container
                .Bind<IInteractView>()
                .FromInstance(InstantiateView<IInteractView>(_interactViewPrefab))
                .AsCached();

            Container
                .Bind<IWinView>()
                .FromInstance(InstantiateView<IWinView>(_winView_Prefab))
                .AsCached();
        }


        private T InstantiateView<T>(GameObject prefab)
        {
            GameObject viewInstance = Instantiate(prefab, _containerForUI);
            T view = viewInstance.GetComponent<T>();
            return view;
        }


    }
}