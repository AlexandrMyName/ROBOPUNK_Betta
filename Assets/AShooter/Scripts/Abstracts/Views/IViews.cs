

using System.Collections.Generic;

namespace Abstracts
{

    public interface IViews
    {

        IDeathView Death { get; }

        IDashView Dash { get; }

        IGoldWalletView GoldWallet { get; }

        IExperienceView Experience { get; }

        IShieldView Shield { get; }

        IHealthView Health { get; }
        
        IInteractView Interact { get; }

        IWeaponAbilityView WeaponAbility { get; }

        IPauseMenuView PauseMenu { get; }

        IStoreView StoreMenu { get; }

        public List<IView> GetListView();
        IWinView WinView { get; }


    }
}