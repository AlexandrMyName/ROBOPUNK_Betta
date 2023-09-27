using Abstracts;


namespace Core
{


    public class ComponentsStore : IComponentsStore
    {

        public IAttackable Attackable { get; private set; }

        public IMovable Movable { get; private set; }

        public IDash Dash { get; private set; }

        public IPlayerHP PlayerHP { get; private set; }

        public IViews Views { get; private set; }

        public IGoldWallet GoldWallet { get; private set; }

        public IExperienceHandle ExperienceHandle { get; private set; }

        public IWeaponStorage WeaponStorage { get; private set; }
        public IStoreEnhancement StoreEnhancement { get; private set; }

        public IShield Shield { get; private set; }


        public ComponentsStore(
            IAttackable attackable, 
            IMovable movable, 
            IDash dash, 
            IPlayerHP playerHP, 
            IViews views, 
            IGoldWallet gold, 
            IExperienceHandle exp, 
            IStoreEnhancement store,
            IWeaponStorage weapons,
            IShield shield)
        {

            /////[Construct]\\\\\
            Attackable = attackable;
            Movable = movable;
            Dash = dash;
            PlayerHP = playerHP;
            Views = views;
            GoldWallet = gold;
            ExperienceHandle = exp;
            WeaponStorage = weapons;
            StoreEnhancement = store;
            Shield = shield;
        } 


    }

}
