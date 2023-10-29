using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using User;
using User.Presenters;
using Object = UnityEngine.Object;


namespace Core
{

    public class PlayeLevelRewardSystem : BaseSystem , IDisposable
    {
        
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private IComponentsStore _componentsStore;
        private GameObject _baseObject;
        private IExperienceHandle _experienceHandle;
        private IRewardMenuView _rewardMenuUI;
        private List<LevelRewardItemConfig> _levelRewardItemConfigs;
        private int _numberOfActiveRewardItems;

        private bool _isMenuActive = false;

        private UnityAction<float>[] _listOfRewardFunctions;
        private GameObject[] _rewardButtonGameObjects;
        private UnityAction[] _availableRewards;
        private ParticleSystem[] _rewardsEffects;
        private List<GameObject> _chosenRewards = new List<GameObject>();

        protected override void Awake(IGameComponents components)
        {
            _baseObject = components.BaseObject;
            _componentsStore = _baseObject.GetComponent<IPlayer>().ComponentsStore;

            _rewardMenuUI = _componentsStore.Views.RewardMenu;
            _experienceHandle = _componentsStore.ExperienceHandle;
            _levelRewardItemConfigs = _componentsStore.LevelReward.RewardItems;
            _numberOfActiveRewardItems = _componentsStore.LevelReward.NumberOfActiveRewardItems;

            CreatingListOfRewardFunctions();

            FillAvailableRewards();

            AddRewardEffectsToPlayerObject();

            SubscribeToExperienceChanges();

            CustomizeRewardButtons();
        }


        private void CreatingListOfRewardFunctions()
        {
            _listOfRewardFunctions = new UnityAction<float>[_levelRewardItemConfigs.Count];

            _listOfRewardFunctions[0] = SpeedUpgrade;
            _listOfRewardFunctions[1] = DamageUpgrade;
            _listOfRewardFunctions[2] = HealthUpgrade;
            _listOfRewardFunctions[3] = GetBonusGold;
            _listOfRewardFunctions[4] = GetBonusExperience;
            _listOfRewardFunctions[5] = ShieldStrengthUpgrade;
            _listOfRewardFunctions[6] = DistanceDashUpgrade;
            _listOfRewardFunctions[7] = RechargeDashUpgrade;
            _listOfRewardFunctions[8] = ReplenishmentOfAmmunition;
        }


        private void FillAvailableRewards()
        {
            _availableRewards = new UnityAction[_levelRewardItemConfigs.Count];

            for (int i = 0; i < _levelRewardItemConfigs.Count; i++)
            {
                int rewardIndex = i;
                _availableRewards[i] = () => {
                    var upgradeCoefficient = _levelRewardItemConfigs[rewardIndex].upgradeCoefficient;

                    _listOfRewardFunctions[rewardIndex].Invoke(upgradeCoefficient);

                    UseEffect(_rewardsEffects[rewardIndex]);
                };
            }
        }


        private void UseEffect(ParticleSystem particleSystem)
        {
            particleSystem.Play();

            Observable.Timer(TimeSpan.FromSeconds(2))
                .Subscribe(_ =>
                {
                    particleSystem.Stop();
                }).AddTo(_baseObject);
        }


        private void AddRewardEffectsToPlayerObject()
        {
            _rewardsEffects = new ParticleSystem[_levelRewardItemConfigs.Count];

            var rewardEffectsObject = Object.Instantiate(new GameObject("RewardEffects"), _baseObject.transform);

            for (int i = 0; i < _levelRewardItemConfigs.Count; i++)
            {
                var effectPrefrab = Object.Instantiate(_levelRewardItemConfigs[i].effectPrefrab, rewardEffectsObject.transform);
                _rewardsEffects[i] = effectPrefrab.GetComponent<ParticleSystem>();
                _rewardsEffects[i].Stop();
            }

        }


        private void SubscribeToExperienceChanges()
        {
            _disposables.Add(_experienceHandle.CurrentLevel.Subscribe(OnLevelChanged));
        }


        private void OnLevelChanged(int CurrentLvl)
        {
            if (_experienceHandle.CurrentLevel.Value > 1)
                ShowUpgradeMenu();
        }


        public void ShowUpgradeMenu()
        {
            if (!_isMenuActive)
            {
                _rewardMenuUI.Show();
                _isMenuActive = true;
                
                Observable.Timer(TimeSpan.FromSeconds(2))
                    .Subscribe(_ =>
                    {
                        _rewardMenuUI.Animation.Play("LevelRewardMenuAnimationOpen");
                    }).AddTo(_baseObject);

                Observable.Timer(TimeSpan.FromSeconds(3))
                    .Subscribe(_ =>
                    {
                        Time.timeScale = 0;
                        ShowMenuContent();
                    }).AddTo(_baseObject);
            }
        }


        private void ShowMenuContent()
        {
            while (_chosenRewards.Count < _numberOfActiveRewardItems)
            {
                var rewardButtonGO = _rewardButtonGameObjects[UnityEngine.Random.Range(0, _rewardButtonGameObjects.Length)];
                if (!_chosenRewards.Contains(rewardButtonGO))
                {
                    rewardButtonGO.SetActive(true);
                    _chosenRewards.Add(rewardButtonGO);
                }
            }
        }


        private void CustomizeRewardButtons()
        {
            _rewardButtonGameObjects = new GameObject[_levelRewardItemConfigs.Count];

            for (int indexRewards = 0; indexRewards < _levelRewardItemConfigs.Count; indexRewards++)
            {
                int rewardIndex = indexRewards;

                GameObject item = Object.Instantiate(_rewardMenuUI.LevelRewardItemPrefab, _rewardMenuUI.HorizontalLayoutGroup.transform);

                var levelRewardItemView = item.GetComponent<LevelRewardItemView>();
                levelRewardItemView.Button.onClick.AddListener(() => { SelectUpgrade(rewardIndex); });
                levelRewardItemView.Button.image.sprite = _levelRewardItemConfigs[indexRewards].Icon;
                levelRewardItemView.Description.text = _levelRewardItemConfigs[indexRewards].nameItem;
                levelRewardItemView.Characteristic.text = _levelRewardItemConfigs[indexRewards].upgradeCoefficient.ToString();
                levelRewardItemView.Dimension.text = _levelRewardItemConfigs[indexRewards].unitImprovementCoefficient;
                levelRewardItemView.TooltipText.text = _levelRewardItemConfigs[indexRewards].description;

                _rewardButtonGameObjects[indexRewards] = item;
            }
        }


        public void SelectUpgrade(int indexRewards)
        {
            if (_isMenuActive && (indexRewards >= 0) && (indexRewards < _availableRewards.Length))
            {
                CloseUpgradeMenu();
                _availableRewards[indexRewards].Invoke();
            }
        }


        public void CloseUpgradeMenu()
        {
            _rewardMenuUI.Animation.Rewind("LevelRewardMenuAnimationOpen");

            if (_isMenuActive)
            {
                _rewardMenuUI.Hide();
                _isMenuActive = false;
                Time.timeScale = 1;

                foreach (var reward in _chosenRewards)
                {
                    reward.SetActive(false);
                }

                _chosenRewards.Clear();
            }
        }


        private void ReplenishmentOfAmmunition(float upgradeCoefficient)
        {
            foreach (var weapon in _componentsStore.WeaponStorage.Weapons)
            {
                var rangeWeapon = weapon.Value as IRangeWeapon;

                // + bullet; 
            }
        }


        private void RechargeDashUpgrade(float upgradeCoefficient)
        {
            _componentsStore.Dash.RegenerationTime *= (1 - ConversionToDecimalFromPercentage(upgradeCoefficient));
        }


        private void DistanceDashUpgrade(float upgradeCoefficient)
        {
            _componentsStore.Dash.DashForce *= ConversionToDecimalFromPercentage(upgradeCoefficient);
        }
         

        private void ShieldStrengthUpgrade(float upgradeCoefficient)
        {
            _componentsStore.Shield.MaxProtection *= ConversionToDecimalFromPercentage(upgradeCoefficient);
        }


        private void GetBonusExperience(float upgradeCoefficient)
        {
            _componentsStore.ExperienceHandle.CurrentExperience.Value += upgradeCoefficient;
        }


        private void GetBonusGold(float upgradeCoefficient)
        {
            _componentsStore.GoldWallet.CurrentGold.Value += (int)upgradeCoefficient;
        }


        private void HealthUpgrade(float upgradeCoefficient)
        {
            _componentsStore.Attackable.Health.Value += upgradeCoefficient;
        }


        private void DamageUpgrade(float upgradeCoefficient)
        {
            foreach (var weapon in _componentsStore.WeaponStorage.Weapons)
            {
                weapon.Value.Damage *= ConversionToDecimalFromPercentage(upgradeCoefficient);
            }
        }

        private void SpeedUpgrade(float upgradeCoefficient)
        {
            _componentsStore.Movable.Speed.Value *= ConversionToDecimalFromPercentage(upgradeCoefficient);
        }


        private float ConversionToDecimalFromPercentage(float x)
        {
            return ((x / 100) + 1);
        }


        public void Dispose()
        {
            _disposables.ForEach(disposable => disposable.Dispose());
            _disposables.Clear();
        }


    }
}