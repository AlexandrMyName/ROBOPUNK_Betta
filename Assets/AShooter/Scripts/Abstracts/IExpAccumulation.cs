using UniRx;


namespace Abstracts
{

    public interface IExpAccumulation
    {

        ReactiveProperty<float> CurrentExp { get; }

        public void AddExp(float amountExp);


    }
}
