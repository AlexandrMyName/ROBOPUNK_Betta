using Zenject;
using UnityEngine;
using User.Presenters;

namespace DI
{
    public class ItemsInstaller : MonoInstaller
    {
        public bool _useImproveItems;
        [SerializeField] private ItemConfigs _itemConfigs;
        [SerializeField] private TimerPool _timerPool;
        [SerializeField] private ImprovablePresenter _improvablePresenter;
        [SerializeField] private GameObject _improvableItemPrefab;

        public override void InstallBindings()
        {
            Container.Bind<TimerPool>().FromInstance(_timerPool).AsCached();
            Container.Bind<ItemConfigs>().FromInstance(_itemConfigs).AsCached();
            Container.Bind<GameObject>().WithId("ImprovableItemView").FromInstance(_improvableItemPrefab).AsSingle();

            Container.Bind<ImprovablePresenter>().FromInstance(_improvablePresenter).AsCached();

        }

    }
}