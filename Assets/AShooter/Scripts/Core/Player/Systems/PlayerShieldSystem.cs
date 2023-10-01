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
        private List<IDisposable> _regenerationTimers = new();


        public void Dispose()
        {

            _disposables.ForEach(disposable => disposable.Dispose());
            StopRegenerationProccess();
        }
        

        protected override void Awake(IGameComponents components)
        {

            _shield = components.BaseTransform.GetComponent<IPlayer>().ComponentsStore.Shield;
            _attackable = components.BaseTransform.GetComponent<IPlayer>().ComponentsStore.Attackable;

            _view = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Shield;

            _attackable.HealthProtection.Subscribe(RefreshProtection).AddTo(_disposables);
            _attackable.HealthProtection.Value = _shield.MaxProtection;
            _attackable.HealthProtection.SkipLatestValueOnSubscribe();
            _view.Show();
        }


        protected void RefreshProtection(float protection)
        {

            if (_shield.IsRegeneration.Value == true)
            {
                StopRegenerationProccess();
                _shield.IsRegeneration.Value = false;
            }
            CheckProtection(protection);
        }


        private void CheckProtection(float protection)
        {

            if (protection > 0)
            {
                _view.RefreshProtection(_attackable.HealthProtection.Value, _shield.MaxProtection);
            }
            else if (protection <= 0)
            {
                _view.Deactivate();
                _view.RefreshProtection(0, _shield.MaxProtection);
                StartRegenerationProccess();
            }
        }


        private void StartRegenerationProccess()
        {

                Observable.Timer(TimeSpan.FromSeconds(_shield.MaxRegenerationSeconds)).Subscribe(

                    value =>
                    {
                
                        _attackable.HealthProtection.Value = _shield.MaxProtection;
                        _shield.IsRegeneration.Value = false;
                        _view.Show();

                    }).AddTo(_regenerationTimers);
        }


        private void StopRegenerationProccess()
        => _regenerationTimers.ForEach(timer => timer.Dispose());

        
    }
}