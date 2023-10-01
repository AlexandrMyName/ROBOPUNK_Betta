using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using Zenject;
using UnityEngine;


namespace Core
{

    public class PlayerDashSystem : BaseSystem , IDisposable
    {

        [Inject] private IInput _input;
        private IDash _dash;
        private IDashView _view;
        private IMovable _movable;
        private IAttackable _attackable;

        private List<IDisposable> _inputsDisposables = new();
        private List<IDisposable> _timersDisposables = new();

       
        protected override void Awake(IGameComponents components)
        {

            _dash = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Dash;
            _movable = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Movable;
            _view = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Dash;
            _attackable = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Attackable;

            _view.Regenerate(_dash.RegenerationTime);
            _view.Show();

        }


        protected override void Start()
        {

            _input.DashClick.AxisOnChange
                .Subscribe(value => {
                    Dash();
                     
                }).AddTo(_inputsDisposables);
            
        }


        protected override void Update()
        {

            if (_dash.IsProccess) 
                _view.Refresh();

        }


        private void RegenerateDash()
        {

            Observable.Timer(TimeSpan.FromSeconds(_dash.RegenerationTime))
                .Subscribe(timer => { 

                    _dash.IsProccess = false;
                    DisposeTimers();
                    _view.Regenerate(_dash.RegenerationTime); })
                .AddTo(_timersDisposables);
 
        }


        private void Dash()
        {

            if (_dash.IsProccess) return;
 
            IgnoreHealth();

            _dash.IsProccess = true;
            
            _movable.Rigidbody.AddForce(_movable.MoveDirection * _dash.DashForce, ForceMode.Impulse);

            RegenerateDash();
        }


        private void IgnoreHealth()
        {

            _attackable.IsIgnoreDamage = true;

            Observable.Timer(TimeSpan.FromMilliseconds(700))
                .Subscribe(val => {

                _attackable.IsIgnoreDamage = false;

            }).AddTo(_timersDisposables);
        }



        public void Dispose()
        {

            DisposeInput();
            DisposeTimers();
        }


        private void DisposeTimers()
        {

            _timersDisposables.ForEach(timer => timer.Dispose());
            _timersDisposables.Clear();
        }


        private void DisposeInput()
        {

            _inputsDisposables.ForEach(disposable => disposable.Dispose());
            _inputsDisposables.Clear();
        }

    }
}