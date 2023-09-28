using Abstracts;
using Zenject;


namespace User.Components
{

    public class ViewsComponent : IViews
    {

        [Inject] public IDeathView Death { get;  }

        [Inject] public IDashView Dash { get; }

        [Inject] public IStoreView Store { get; }

        [Inject] public IGoldWalletView GoldWallet { get; }

        [Inject] public IExperienceView ExperienceView { get; }

        [Inject] public IHealthView HealthView { get; }
    }
}