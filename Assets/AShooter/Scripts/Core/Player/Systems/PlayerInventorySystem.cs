using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;
using UniRx.Triggers;
using UniRx;
using User;
using Zenject;


namespace Core
{

    public class PlayerInventorySystem : BaseSystem, IDisposable
    {

        [Inject] IInput _input;
        private IGameComponents _components;
        private List<IDisposable> _disposables;
        private IGoldWallet _goldWallet;
        private IWeaponStorage _weaponStorage;
        private IInteractView _interactView;

        private bool _canOpenChest;


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


        protected override void Awake(IGameComponents components)
        {

            _interactView = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.Interact;
            _interactView.Hide();
            _disposables = new();
            _components = components;
            _goldWallet = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.GoldWallet;
            _weaponStorage = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.WeaponStorage;
            _input.Interact.AxisOnChange.Subscribe(value => SwitchInteractInput()).AddTo(_disposables);

            _components.BaseObject.GetComponent<Collider>()
                .OnTriggerStayAsObservable()
                    .Where(x => x.GetComponent<IChest>() != null)
                    .Subscribe(
                        collider =>
                        {

                            if (_canOpenChest)
                            {
                                var chest = collider.GetComponent<IChest>();
                                var objectItem = (chest.Falling) ? chest.GetItem(ChestContentType.Weapon) : chest.GetRandomItem();

                                ApplyGettingItem(objectItem);
                            }
                            else _interactView.Show();
                               
                            
                        }).AddTo(_disposables);

            _components.BaseObject.GetComponent<Collider>()
                .OnTriggerExitAsObservable()
                    .Subscribe( collider =>{

                            _interactView.Hide();

                        }).AddTo(_disposables);
        }


        private void SwitchInteractInput()
        {
            _canOpenChest = true;
            Observable.Timer(TimeSpan.FromMilliseconds(200)).Subscribe(

                timer => {
                _canOpenChest = false;
                    _interactView.Hide();
                }).AddTo(_disposables);
        }


        private void ApplyGettingItem(object objItem)
        {
             
            if(objItem == null)
            {
                ApplayGettingEmpty();
                return;
            }

            if(objItem.GetType() == typeof(CoinMeta))
            {
                ApplayGettingGold((ICoinMeta)objItem);
            }

            if (objItem.GetType() == typeof(float))
            {
                ApplayGettingMedicineChest();
            }

            if (objItem.GetType() == typeof(PickUpItemModel))
            {
                ApplayGettingWeapon((PickUpItemModel)objItem);
            }

            if (objItem.GetType() == typeof(ImprovableItemConfig))
            {
                ApplayGettingImprovable();
            }
        }


        private void ApplayGettingImprovable()
        {
#if UNITY_EDITOR
            Debug.Log("Get Improvable");
#endif
        }


        private void ApplayGettingEmpty()
        {
#if UNITY_EDITOR
            Debug.Log("Chest is empty))");
#endif
        }


        private void ApplayGettingWeapon(PickUpItemModel pickUpItemModel)
        {
            _weaponStorage.GetPickUpItem(pickUpItemModel);

#if UNITY_EDITOR
            Debug.Log($"Get {pickUpItemModel.PickUpItemType} {pickUpItemModel.WeaponConfig.WeaponType}");
#endif
        }


        private void ApplayGettingMedicineChest()
        {
#if UNITY_EDITOR
            Debug.Log("Get Medicine Chest");
#endif
        }


        private void ApplayGettingGold(ICoinMeta coin)
        {
            _goldWallet.AddGold(coin.Value);

#if UNITY_EDITOR
            Debug.Log($"Add Gold -> {coin.Value}, Gold Account -> {_goldWallet.CurrentGold}");
#endif
        }


    }
}