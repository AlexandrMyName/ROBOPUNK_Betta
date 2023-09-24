using Abstracts;
using UnityEngine;
using User;
using Zenject;


public class UIInstaller : MonoInstaller
{

    [SerializeField] private Transform _containerForUI;

    [SerializeField] private GameObject _deathViewPrefab;


    public override void InstallBindings()
    {

        Container
            .Bind<IDeathView>()
            .FromInstance(InstantiateView<IDeathView>(_deathViewPrefab))
            .AsCached();
        

        



    }


    private  T InstantiateView <T>(GameObject prefab)
    {

        GameObject viewInstance = Instantiate(prefab, _containerForUI);
        T view = viewInstance.GetComponent<T>();
        return view;
    }

}
