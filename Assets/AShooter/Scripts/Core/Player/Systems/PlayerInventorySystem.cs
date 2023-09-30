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

        private bool _canOpenChest;


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


        protected override void Awake(IGameComponents components)
        {

            _disposables = new();
            _components = components;
            _goldWallet = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.GoldWallet;
            _input.Interact.AxisOnChange.Subscribe(value => SwitchInteractInput()).AddTo(_disposables);

            _components.BaseObject.GetComponent<Collider>()
                .OnTriggerStayAsObservable()
                    .Where(x => x.GetComponent<IChest>() != null)
                    .Subscribe(
                        collider =>
                        {
                            //эта переменная из инпута (кнопка нажата) 
                           if(_canOpenChest)
                                ApplyGettingItem(collider.GetComponent<IChest>().GetRandomItem());
                            else
                            {
                                //Тебе здесь надо вывести view с кнопкой E (открыть сундук)

                                // К сожалению я хз как вывести название кнопки из нашего инпута
                                //Лучше все это вынести в метод (не анонимный) 
                               
                            }
                        }).AddTo(_disposables);
            
        }


        private void SwitchInteractInput()
        {
            //Тебе сюда не надо
            _canOpenChest = true;
            Observable.Timer(TimeSpan.FromMilliseconds(200)).Subscribe(

                timer => {
                _canOpenChest = false;
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