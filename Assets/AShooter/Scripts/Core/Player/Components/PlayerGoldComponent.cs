using Abstracts;
using UniRx;


namespace Core
{


    public class PlayerGoldComponent : IGoldWallet
    {

        public ReactiveProperty<float> CurrentGold { get; }


        public PlayerGoldComponent()
        {
            CurrentGold = new ReactiveProperty<float>(0);
        }


        public void AddGold(float amountGold)
        {
            CurrentGold.Value += amountGold;
        }


        public void RemoveGold(float amountGold)
        {
            CurrentGold.Value -= amountGold;
        }


    }
}