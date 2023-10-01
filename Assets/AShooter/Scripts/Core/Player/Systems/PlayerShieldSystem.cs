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

        private IShieldView _view;

        private List<IDisposable> _disposables = new();


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());
        

        protected override void Awake(IGameComponents components)
        {

            _shield = components.BaseTransform.GetComponent<IPlayer>().ComponentsStore.Shield;
            _attackable = components.BaseTransform.GetComponent<IPlayer>().ComponentsStore.Attackable;

            _view = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Shield;

            _shield.ShieldProccessTime.Subscribe(Deactivate).AddTo(_disposables);
            _shield.IsActivate.Subscribe(Activate).AddTo(_disposables);
        }


        private void Activate(bool isActivate)
        {

            if (isActivate)
            {
                _attackable.HealthProtection.Value = _shield.MaxProtection;
                _view.Show();
            }

            _attackable.IsIgnoreDamage = isActivate;
        }


        protected override void Update()
        {

            if (_shield.IsActivate.Value)
            {
                _view.RefreshTime(_shield.ShieldProccessTime.Value);
                _view.RefreshProtection(_attackable.HealthProtection.Value,  _shield.MaxProtection);
            }

        }


        private void Deactivate(float maxTime)
        {

            if(maxTime > 0)
            Observable
                .Timer(TimeSpan.FromSeconds(maxTime))
                .Subscribe( _ =>
                {
                    UnityEngine.Debug.LogWarning("RegenerationShield");
                    _attackable.IsIgnoreDamage = false;
                    _shield.IsActivate.Value = false;
                    _shield.ShieldProccessTime.Value = 0;
                    _view.Deactivate();
                })
                .AddTo(_disposables);

        }
    }
}