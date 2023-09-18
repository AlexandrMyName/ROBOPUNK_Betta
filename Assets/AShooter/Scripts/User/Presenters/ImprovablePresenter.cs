using Abstracts;
using System.Collections.Generic;
using UnityEngine;
using User.View;
using Zenject;

namespace User.Presenters
{
    public class ImprovablePresenter : MonoBehaviour
    {
        [Inject] private ItemConfigs _itemConfigs;
        [SerializeField] private Transform _containerForView; 
        [Inject(Id = "ImprovableItemView")] private GameObject _itemViewPrefab;
        private List<ImprovableItemView> _itemViews;
        private IAttackable _attackable;
        private IMovable _movable;
        

        public void Init(IAttackable attackableModel, IMovable movableModel)
        {
            _attackable = attackableModel;
            _movable = movableModel;
            _itemViews = new();
        }


        public void ApplyImprove(IImprovement improvement)
        {
            CreateView(improvement);

            switch (improvement.GetImproveType())
            {
                case ImprovementType.Attackable:

                    Debug.Log($"Attack improve - accept! |{improvement.Value}|");

                    
                    break;
                case ImprovementType.Movable:
                    Debug.Log($"Speed improve - accept! |{improvement.Value}|");
                  
                    _movable.Speed.Value *= improvement.Value;

                    break;
            }

        }

        public void CanselImprove(IImprovement improvement)
        {
            switch (improvement.GetImproveType())
            {
                case ImprovementType.Attackable:

                    Debug.Log($"Attack improve - canseled! |{improvement.Value}|");
                    break;
                case ImprovementType.Movable:
                    Debug.Log($"Speed improve - canseled! |{improvement.Value}|");
                    _movable.Speed.Value /= improvement.Value;
                    break;

            }
            improvement.Dispose();
        }
        

        private void CreateView(IImprovement improvement)
        {
           GameObject itemObject = GameObject.Instantiate(_itemViewPrefab, _containerForView);
           ImprovableItemView itemView = itemObject.GetComponent<ImprovableItemView>();
            _itemViews.Add(itemView);

           itemView.InitView(
               _itemConfigs.ImprovableItems.Find(item => item.ImprovableType == improvement.GetImproveType()).Icon,
               improvement.Value,
               improvement.Timer, 
               improvement.GetImproveTime() == ImprovementTime.Temporary);
        }
    }
}