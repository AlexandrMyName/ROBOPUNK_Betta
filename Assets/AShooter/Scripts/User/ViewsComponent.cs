using Abstracts;
using System.Collections.Generic;
using Zenject;


namespace User.Components
{

    public class ViewsComponent : IViews
    {

        [Inject] public IDeathView Death { get;  }

        [Inject] public IDashView Dash { get; }

        [Inject] public IPauseMenuView PauseMenu { get; }

        [Inject] public IGoldWalletView GoldWallet { get; }

        [Inject] public IExperienceView ExperienceView { get; }

        [Inject] public IShieldView Shield { get; }

        [Inject] public IHealthView HealthView { get; }
     
        [Inject] public IInteractView InteractView { get; }

        [Inject] public IWeaponAbilityView WeaponAbility { get; }


        public List<IView> GetListView()
        {
            var views = new List<IView>();

            views.Add(Death);
            views.Add(Dash);
            views.Add(PauseMenu);
            views.Add(GoldWallet);
            views.Add(ExperienceView);
            views.Add(Shield);
            views.Add(HealthView);
            views.Add(InteractView);
            views.Add(WeaponAbility);

            return views;
        }



    }
}