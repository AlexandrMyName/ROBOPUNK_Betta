using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;


namespace Core
{

    public class PlayerShieldSystem : BaseSystem, IDisposable
    {

        private IShield _shield;
        private IAttackable _attackable;
        private List<IDisposable> _disposables = new();


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());
        

        protected override void Awake(IGameComponents components)
        {

            _shield = components.BaseTransform.GetComponent<IPlayer>().ComponentsStore.Shield;
            _attackable = components.BaseTransform.GetComponent<IPlayer>().ComponentsStore.Attackable;

            _shield.ShieldProccessTime.Subscribe(Deactivate).AddTo(_disposables);
            _shield.IsActivate.Subscribe(Activate).AddTo(_disposables);
        }


        private void Activate(bool activate)
        {

            if (activate)
            {
                _attackable.IsIgnoreDamage = true;
            }
        }


        private void Deactivate(float maxTime)
        {

            if(maxTime > 0)
            Observable
                .Timer(TimeSpan.FromSeconds(maxTime))
                .Subscribe( _ =>
                {
                    _attackable.IsIgnoreDamage = false;
                    _shield.ShieldProccessTime.Value = 0;
                })
                .AddTo(_disposables);

        }
    }
}