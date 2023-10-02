using Abstracts;
using UnityEngine;
using User;
using Zenject;


public class UIInstaller : MonoInstaller
{

    [SerializeField] private Transform _containerForUI;

    [SerializeField] private GameObject _deathViewPrefab;
    [SerializeField] private GameObject _storeViewPrefab;
    [SerializeField] private GameObject _dashViewPrefab;
    [SerializeField] private GameObject _goldWalletViewPrefab;
    [SerializeField] private GameObject _experienceViewPrefab;
    [SerializeField] private GameObject _shieldViewPrefab;
    [SerializeField] private GameObject _playerHpViewPrefab;
    [SerializeField] private GameObject _mainMenuViewPrefab;
    [SerializeField] private GameObject _interactViewPrefab;
    [SerializeField] private GameObject _weaponAbilityViewPrefab;


    public override void InstallBindings()
    {
        if (_deathViewPrefab != null)
        {
            Container
                .Bind<IDeathView>()
                .FromInstance(InstantiateView<IDeathView>(_deathViewPrefab))
                .AsCached();
        }

        if (_storeViewPrefab != null)
        {
            Container
                .Bind<IStoreView>()
                .FromInstance(InstantiateView<IStoreView>(_storeViewPrefab))
                .AsCached();
        }

        if (_dashViewPrefab != null)
        {
            Container
                .Bind<IDashView>()
                .FromInstance(InstantiateView<IDashView>(_dashViewPrefab))
                .AsCached();
        }

        if (_experienceViewPrefab != null)
        {
            Container
                .Bind<IExperienceView>()
                .FromInstance(InstantiateView<IExperienceView>(_experienceViewPrefab))
                .AsCached();
        }

        if (_goldWalletViewPrefab != null)
        {
            Container
                .Bind<IGoldWalletView>()
                .FromInstance(InstantiateView<IGoldWalletView>(_goldWalletViewPrefab))
                .AsCached();
        }

        if (_playerHpViewPrefab != null)
        {
            Container
                .Bind<IHealthView>()
                .FromInstance(InstantiateView<IHealthView>(_playerHpViewPrefab))
                .AsCached();
        }

        if (_mainMenuViewPrefab != null)
        {
            Container
                .Bind<IMainMenuView>()
                .FromInstance(InstantiateView<IMainMenuView>(_mainMenuViewPrefab))
                .AsCached();
        }

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

    }


    private  T InstantiateView <T>(GameObject prefab)
    {
        GameObject viewInstance = Instantiate(prefab, _containerForUI);
        T view = viewInstance.GetComponent<T>();
        return view;
    }


}