using UniRx;


namespace Abstracts
{

    public interface IExperienceHandle
    {

        public ReactiveProperty<float> CurrentExperience { get; }

        public ReactiveProperty<int> CurrentLevel { get; }

        public void AddExperience(float amountExp);

    }
}
