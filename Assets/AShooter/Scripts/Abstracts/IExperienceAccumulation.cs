using UniRx;


namespace Abstracts
{

    public interface IExperienceAccumulation
    {

        ReactiveProperty<float> CurrentExp { get; }

        public void AddExp(float amountExp);


    }
}
