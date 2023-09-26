using Abstracts;
using UnityEngine;
using Zenject;


public class UIInstaller : MonoInstaller
{

    [SerializeField] private Transform _containerForUI;

    [SerializeField] private GameObject _deathViewPrefab;
    [SerializeField] private GameObject _storeViewPrefab;
    [SerializeField] private GameObject _dashViewPrefab;
    [SerializeField] private GameObject _goldWalletViewPrefab;
    [SerializeField] private GameObject _experienceViewPrefab;


    public override void InstallBindings()
    {

        Container
            .Bind<IDeathView>()
            .FromInstance(InstantiateView<IDeathView>(_deathViewPrefab))
            .AsCached();

        Container
            .Bind<IStoreView>()
            .FromInstance(InstantiateView<IStoreView>(_storeViewPrefab))
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

    }


    private  T InstantiateView <T>(GameObject prefab)
    {
        GameObject viewInstance = Instantiate(prefab, _containerForUI);
        T view = viewInstance.GetComponent<T>();
        return view;
    }


}