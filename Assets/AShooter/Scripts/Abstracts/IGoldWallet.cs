using UniRx;


namespace Abstracts
{

    public interface IGoldWallet
    {

        ReactiveProperty<float> CurrentGold { get; }

        public void AddGold(float amountGold);
        public void RemoveGold(float amountGold);


    }
}
