using Abstracts;
using User;


namespace Core.Components
{
    
    public sealed class PlayerStoreEnhancementComponent : IStoreEnhancement
    {

        public StoreItemConfig HealthEnhancement { get; }

        public StoreItemConfig SpeedEnhancement { get; }

        public StoreItemConfig DamageEnhancement { get; }


        public PlayerStoreEnhancementComponent(StoreItemsDataConfig storeItemsDataConfig)
        {
            HealthEnhancement = storeItemsDataConfig.HealthEnhancement;
            SpeedEnhancement = storeItemsDataConfig.SpeedEnhancement;
            DamageEnhancement = storeItemsDataConfig.DamageEnhancement;
        }


    }
}