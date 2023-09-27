namespace Abstracts
{


    public interface IComponentsStore
    {

        IAttackable Attackable { get; }
        IMovable Movable { get; }
        IDash Dash { get;  }
        IPlayerHP PlayerHP { get; }
        IViews Views { get; }
        IGoldWallet GoldWallet { get; }
        IExperienceHandle ExperienceHandle { get; }
        IWeaponStorage WeaponStorage { get; }

        IStoreEnhancement StoreEnhancement { get; }

        IShield Shield { get; }

    }
}