

namespace Abstracts
{

    public interface IViews
    {

        IDeathView Death { get; }

        IDashView Dash { get; }

        IStoreView Store { get; }

        IGoldWalletView GoldWallet { get; }

        IExperienceView ExperienceView { get; }

        IHealthView HealthView { get; }

        IWeaponAbilityView WeaponAbility { get; }

    }
}