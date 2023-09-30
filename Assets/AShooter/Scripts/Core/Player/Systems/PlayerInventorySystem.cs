using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;
using UniRx.Triggers;
using UniRx;
using User;


namespace Core
{

    public class PlayerInventorySystem : BaseSystem, IDisposable
    {

        private IGameComponents _components;
        private List<IDisposable> _disposables;
        private IGoldWallet _goldWallet;

        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _components = components;
            _goldWallet = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.GoldWallet;
            
            /*_components.BaseObject.GetComponent<Collider>()
                .OnTriggerEnterAsObservable()
                    .Where(x => x.GetComponent<IChest>() != null)
                    .Subscribe(
                        collider =>
                        {
                            ApplyGettingItem(collider.GetComponent<IInteractable>().Interact());
                        }).AddTo(_disposables);*/
            
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

            if (objItem.GetType() == typeof(WeaponConfig))
            {
                ApplayGettingWeapon();
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


        private void ApplayGettingWeapon()
        {
#if UNITY_EDITOR
            Debug.Log("Get Weapon");
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