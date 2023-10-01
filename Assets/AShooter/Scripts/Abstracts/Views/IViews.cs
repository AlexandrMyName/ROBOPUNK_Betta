

namespace Abstracts
{

    public interface IViews
    {

        IDeathView Death { get; }

        IDashView Dash { get; }

        IStoreView Store { get; }

        IGoldWalletView GoldWallet { get; }

        IExperienceView ExperienceView { get; }

        IShieldView Shield { get; }

        IHealthView HealthView { get; }
        
        IInteractView InteractView { get; }

        IWeaponAbilityView WeaponAbility { get; }

    }
}