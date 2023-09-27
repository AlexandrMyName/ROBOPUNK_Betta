using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;


namespace Core
{

    public class PlayerGoldWalletSystem : BaseSystem , IDisposable
    {
        
        private List<IDisposable> _disposables;
        private IGoldWalletView _goldWaletView;
        private IStoreView _store;


        protected override void Awake(IGameComponents components)
        {
            _disposables = new();

            _goldWaletView = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Views.GoldWallet;
            var currentGold = components.BaseObject.GetComponent<IPlayer>().ComponentsStore.GoldWallet.CurrentGold;
            currentGold.Subscribe(UpdateDisplay);

            _goldWaletView.Show();
            _goldWaletView.ChangeDisplay(currentGold.Value);
        }


        private void UpdateDisplay(int value)
        {
            _goldWaletView.ChangeDisplay(value);
        }


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


    }
}

