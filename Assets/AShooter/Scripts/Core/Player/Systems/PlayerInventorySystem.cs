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


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _components = components;
 
            _components.BaseObject.GetComponent<Collider>()
                .OnTriggerEnterAsObservable()
                    .Where(x => x.GetComponent<IChest>() != null)
                    .Subscribe(
                        collider =>
                        {
                            ApplyGettingItem(collider.GetComponent<IChest>().GetRandomItem());
                        }).AddTo(_disposables);

        }


        private void ApplyGettingItem(object objItem)
        {
            if(objItem == null)
            {

                Debug.LogWarning("Chest is empty))");
                return;
            }
            if(objItem.GetType() == typeof(int))
            {
                ICoinMetta coinMetta = new CoinMetta((int)objItem);
                Debug.LogWarning("Get Coin");
            }

            if (objItem.GetType() == typeof(float))
            {
                IHealth coinMetta = new HealthObject((float)objItem);
                Debug.LogWarning("Get Health");
            }

            if (objItem.GetType() == typeof(WeaponConfig))
            {
                Debug.LogWarning("Get Weapon");
            }

            if (objItem.GetType() == typeof(ImprovableItemConfig))
            {
                Debug.LogWarning("Get Improvable");
            }

            Debug.Log($"Get item from chest : {objItem.GetType()}");
        }

    }
}