using System;
using UnityEngine;
using User;
using User.Presenters;


namespace AShooter.Scripts.User.Presenters
{
    
    public sealed class MainStorePresenter : IDisposable
    {

        public MainStoreView MainStoreView { get; private set; }
        public StoreItemsDataConfigs StoreItemsData { get; private set; }


        public MainStorePresenter(MainStoreView mainStoreView, StoreItemsDataConfigs storeItemsDataConfigs)
        {
            MainStoreView = mainStoreView;
            StoreItemsData = storeItemsDataConfigs;
            MakeSubscriptions();
            ProcessItemsData(StoreItemsData);
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


        private void ProcessItemsData(StoreItemsDataConfigs storeData)
        {
            storeData.PassiveUpgradeItemsConfigs.ForEach(data => { InitStoreItem(data); });
        }

        
        private void InitStoreItem(StoreItemConfig itemConfig)
        {
            var itemViewObject = GameObject.Instantiate(
                StoreItemsData.StoreItemPrefab,
                MainStoreView.PassiveSkillsContent.transform);
            
            itemViewObject.transform.localScale = Vector3.one;
            itemViewObject.transform.localPosition = Vector3.zero;
            itemViewObject.transform.localRotation = Quaternion.identity;

            StoreItemView storeItem = itemViewObject.GetComponent<StoreItemView>();
            storeItem.Init(itemConfig);
            storeItem.SubscribeClickButton(OnItemClick);
        }


        private void OnItemClick(StoreItemView itemView)
        {
            
        }


        private void ResolveUpgradeByItemType()
        {
            
        }


        public void Dispose()
        {
            UnsubscribeAll();
        }
        
        
    }
}