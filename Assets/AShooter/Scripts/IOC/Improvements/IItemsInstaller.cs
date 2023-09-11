using Zenject;
using UnityEngine;

namespace DI
{
    public class IItemsInstaller : MonoInstaller
    {
        public bool _useImproveItems;
        [SerializeField] private TimerPool _timerPool;


        public override void InstallBindings()
        {
            Container.Bind<TimerPool>().FromInstance(_timerPool).AsCached();
        }

    }
}