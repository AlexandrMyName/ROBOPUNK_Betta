using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;
using UniRx.Triggers;
using UniRx;
using Zenject;


namespace Core
{

    public class PickUpItemSystem : BaseSystem, IDisposable
    {

        [Inject] private IInput _input;
        private IGameComponents _components;
        private IWeaponStorage _weaponStorage;
        private IInteractView _interactView;

        private List<IDisposable> _disposables = new();
        private bool _canOpen;


        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _weaponStorage = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.WeaponStorage;

            _interactView = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Interact;
            _interactView.Hide();
        }


        protected override void Start()
        {
            _input.Interact.AxisOnChange.Subscribe(_ => SwitchInteractInput()).AddTo(_disposables);

            _components.BaseObject.GetComponent<Collider>()
                .OnTriggerStayAsObservable()
                    .Where(x => x.GetComponent<IPickUpItem>() != null)
                    .Subscribe(collider => {
                        if (_canOpen)
                            collider.GetComponent<IPickUpItem>().Raise(_weaponStorage);
                        else _interactView.Show();
                    }).AddTo(_disposables);

            _components.BaseObject.GetComponent<Collider>()
                .OnTriggerExitAsObservable()
                    .Subscribe(_ => _interactView.Hide()).AddTo(_disposables);
        }


        private void SwitchInteractInput()
        {
            _canOpen = true;

            Observable.Timer(TimeSpan.FromMilliseconds(200)).Subscribe(
                timer => {
                    _canOpen = false;
                    _interactView.Hide();
                }).AddTo(_disposables);
        }


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


    }
}
