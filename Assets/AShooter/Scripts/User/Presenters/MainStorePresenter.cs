using System;
using Abstracts;
using UnityEngine;
using User;
using User.Presenters;


namespace AShooter.Scripts.User.Presenters
{
    
    public sealed class MainStorePresenter : IDisposable
    {

        public MainStoreView MainStoreView { get; private set; }
        
        public StoreItemsData StoreItemsData { get; private set; }

        public IPlayerStats PlayerStats { get; private set; }


        public MainStorePresenter(MainStoreView mainStoreView, StoreItemsData storeItemsData, IPlayerStats playerStats)
        {
            MainStoreView = mainStoreView;
            StoreItemsData = storeItemsData;
            PlayerStats = playerStats;
            MakeSubscriptions();
            ProcessItemsData(StoreItemsData);
            MainStoreView.MetaExperienceValue.text = $"{playerStats.MetaExperience}";
            MainStoreView.GoldValue.text = $"{PlayerStats.Money}";
        }


        private void MakeSubscriptions()
        {
            MainStoreView.BackButton.onClick.AddListener(HideStore);
            MainStoreView.PassiveSkillsButton.onClick.AddListener(MainStoreView.OnClickPassiveSkills);
            MainStoreView.WeaponsButton.onClick.AddListener(MainStoreView.OnClickWeapons);
            MainStoreView.SkinsButton.onClick.AddListener(MainStoreView.OnClickSkins);
        }


        private void UnsubscribeAll()
        {
            MainStoreView.BackButton.onClick.RemoveAllListeners();
            MainStoreView.PassiveSkillsButton.onClick.RemoveAllListeners();
            MainStoreView.WeaponsButton.onClick.RemoveAllListeners();
            MainStoreView.SkinsButton.onClick.RemoveAllListeners();
        }


        private void HideStore()
        {
            MainStoreView.Hide();
        }


        private void ProcessItemsData(StoreItemsData storeData)
        {
            storeData.PassiveUpgradesData.ForEach(data => { InitStoreItem(data); });
        }

        
        private void InitStoreItem(StoreItemConfig itemConfig)
        {
            StoreItemView itemView = GameObject.Instantiate(
                StoreItemsData.StoreItemPrefab,
                MainStoreView.PassiveSkillsContent.transform);
            itemView.transform.localScale = Vector3.one;
            itemView.transform.localPosition = Vector3.zero;
            itemView.transform.localRotation = Quaternion.identity;
            
            itemView.Init(itemConfig, ResolveCurrentMultiplierValueByItem(itemView));
            itemView.SubscribeClickButton(OnItemClick);
        }


        private void OnItemClick(StoreItemView itemView)
        {
            switch (itemView.ItemData.ItemType)
            {
                case StoreItemType.BaseDamage:
                    ResolveBuyingProcessByItem(itemView);
                    break;
                
                case StoreItemType.BaseHealth:
                    ResolveBuyingProcessByItem(itemView);
                    break;
                
                case StoreItemType.BaseSpeed:
                    ResolveBuyingProcessByItem(itemView);
                    break;
                
                case StoreItemType.BaseShootSpeed:
                    ResolveBuyingProcessByItem(itemView);
                    break;
                
                case StoreItemType.ShieldCapacity:
                    ResolveBuyingProcessByItem(itemView);
                    break;
                    
                case StoreItemType.DashDistance:
                    ResolveBuyingProcessByItem(itemView);
                    break;
                
                default:
                    throw new ArgumentException($"Unknown item type [{itemView.ItemData.ItemType}]");;
            }
        }


        private bool ProcessBuying(StoreItemView itemView, ref float multiplier)
        {
            bool isOperationSucceed = false;
            
            if (PlayerStats.TryDeductMetaExperience(Int32.Parse(itemView.Price.text)))
            {
                if (multiplier >= 1)
                    multiplier = (itemView.ItemData.UpgradeCoefficient / 100 + multiplier);
                else
                    multiplier = 1;
                
                itemView.UpdateItemByMultiplier(multiplier);
                isOperationSucceed = true;
            }

            return isOperationSucceed;
        }
        
        
        private float ResolveCurrentMultiplierValueByItem(StoreItemView itemView)
        {
            float multiplier = 0.0f;
            
            switch (itemView.ItemData.ItemType)
            {
                case StoreItemType.BaseDamage:
                    multiplier = PlayerStats.BaseDamageMultiplier;
                    break;
                
                case StoreItemType.BaseHealth:
                    multiplier = PlayerStats.BaseHealthMultiplier;
                    break;
                
                case StoreItemType.BaseSpeed:
                    multiplier = PlayerStats.BaseMoveSpeedMultiplier;
                    break;
                
                case StoreItemType.BaseShootSpeed:
                    multiplier = PlayerStats.BaseShootSpeedMultiplier;
                    break;
                
                case StoreItemType.ShieldCapacity:
                    multiplier = PlayerStats.BaseShieldCapacityMultiplier;
                    break;
                    
                case StoreItemType.DashDistance:
                    multiplier = PlayerStats.BaseDashDistanceMultiplier;
                    break;
                
                default:
                    throw new ArgumentException($"Unknown item type [{itemView.ItemData.ItemType}]");
            }

            return multiplier;
        }
        
        
        private void ResolveBuyingProcessByItem(StoreItemView itemView)
        {
            float multiplier = 0.0f;
            
            switch (itemView.ItemData.ItemType)
            {
                case StoreItemType.BaseDamage:
                    multiplier = PlayerStats.BaseDamageMultiplier;
                    if (ProcessBuying(itemView, ref multiplier))
                    {
                        PlayerStats.BaseDamageMultiplier = multiplier;
                        PlayerStats.SaveStatsInRepository();
                    }
                    break;
                
                case StoreItemType.BaseHealth:
                    multiplier = PlayerStats.BaseHealthMultiplier;
                    if (ProcessBuying(itemView, ref multiplier))
                    {
                        PlayerStats.BaseHealthMultiplier = multiplier;
                        PlayerStats.SaveStatsInRepository();
                    }
                    break;
                
                case StoreItemType.BaseSpeed:
                    multiplier = PlayerStats.BaseMoveSpeedMultiplier;
                    if (ProcessBuying(itemView, ref multiplier))
                    {
                        PlayerStats.BaseMoveSpeedMultiplier = multiplier;
                        PlayerStats.SaveStatsInRepository();
                    }
                    break;
                
                case StoreItemType.BaseShootSpeed:
                    multiplier = PlayerStats.BaseShootSpeedMultiplier;
                    if (ProcessBuying(itemView, ref multiplier))
                    {
                        PlayerStats.BaseShootSpeedMultiplier = multiplier;
                        PlayerStats.SaveStatsInRepository();
                    }
                    break;
                
                case StoreItemType.ShieldCapacity:
                    multiplier = PlayerStats.BaseShieldCapacityMultiplier;
                    if (ProcessBuying(itemView, ref multiplier))
                    {
                        PlayerStats.BaseShieldCapacityMultiplier = multiplier;
                        PlayerStats.SaveStatsInRepository();
                    }
                    break;
                    
                case StoreItemType.DashDistance:
                    multiplier = PlayerStats.BaseDashDistanceMultiplier;
                    if (ProcessBuying(itemView, ref multiplier))
                    {
                        PlayerStats.BaseDashDistanceMultiplier = multiplier;
                        PlayerStats.SaveStatsInRepository();
                    }
                    break;
                
                default:
                    throw new ArgumentException($"Unknown item type [{itemView.ItemData.ItemType}]");
            }
        }
        
        
        public void Dispose()
        {
            UnsubscribeAll();
        }
        
        
    }
}