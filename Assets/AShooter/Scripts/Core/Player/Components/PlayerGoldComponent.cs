using Abstracts;
using UniRx;


namespace Core
{


    public class PlayerGoldComponent : IGoldWallet
    {

        public ReactiveProperty<int> CurrentGold { get; }

        public PlayerGoldComponent()
        {
            CurrentGold = new ReactiveProperty<int>(0);
        }


        public void AddGold(int amountGold)
        {
            CurrentGold.Value += amountGold;
        }


        public void RemoveGold(int amountGold)
        {
            CurrentGold.Value -= amountGold;
        }


    }
}