using Abstracts;
using Zenject;


namespace User.Components
{

    public class ViewsComponent : IViews
    {

        [Inject] public IDeathView Death { get;  }

        [Inject] public IDashView Dash { get; }

    }
}