using Abstracts;
using System.Collections.Generic;
using Zenject;


namespace User.Components
{

    public class ViewsComponent : IViews
    {

        [Inject] public IDeathView Death { get;  }

        [Inject] public IDashView Dash { get; }

        [Inject] public IGoldWalletView GoldWallet { get; }

        [Inject] public IExperienceView Experience { get; }

        [Inject] public IShieldView Shield { get; }

        [Inject] public IHealthView Health { get; }
     
        [Inject] public IInteractView Interact { get; }

        [Inject] public IWeaponAbilityView WeaponAbility { get; }

        [Inject] public IPauseMenuView PauseMenu { get; }

        [Inject] public IStoreView StoreMenu { get; }


        public List<IView> GetListView()
        {
            var views = new List<IView>();

            views.Add(Death);
            views.Add(Dash);
            views.Add(PauseMenu);
            views.Add(GoldWallet);
            views.Add(Experience);
            views.Add(Shield);
            views.Add(Health);
            views.Add(Interact);
            views.Add(WeaponAbility);
            views.Add(StoreMenu);

            return views;
        }


    }
}