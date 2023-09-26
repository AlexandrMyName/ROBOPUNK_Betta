using Abstracts;
using UniRx;


namespace Core
{

    public class PlayerGoldWalletComponent : IGoldWallet
    {

        public ReactiveProperty<int> CurrentGold { get; }

        public PlayerGoldWalletComponent()
        {
            CurrentGold = new ReactiveProperty<int>(100);
        }


        public void AddGold(int amountGold)
        {
            CurrentGold.Value += amountGold;
        }


        public void DeductGold(int amountGold)
        {
            CurrentGold.Value -= amountGold;
        }


    }
}