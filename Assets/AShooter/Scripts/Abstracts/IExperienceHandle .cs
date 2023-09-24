using UniRx;


namespace Abstracts
{

    public interface IExperienceHandle
    {

        ReactiveProperty<float> CurrentExperience { get; }

        public void AddExperience(float amountExp);


    }
}
