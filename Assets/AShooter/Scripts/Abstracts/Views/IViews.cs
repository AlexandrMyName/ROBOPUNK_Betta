

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

        IWinView WinView { get; }

        IRewardMenuView RewardMenu { get; }

        IJetPackView JetPackView { get; }

        public List<IView> GetListView();

        IMP3PlayerView MP3PlayerView { get; }
        public IOptionsView Options { get;  set; }

    }
}