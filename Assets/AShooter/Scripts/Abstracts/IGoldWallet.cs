using UniRx;


namespace Abstracts
{

    public interface IGoldWallet
    {

        ReactiveProperty<int> CurrentGold { get; }

        public void AddGold(int amountGold);
        public void DeductGold(int amountGold);


    }
}
