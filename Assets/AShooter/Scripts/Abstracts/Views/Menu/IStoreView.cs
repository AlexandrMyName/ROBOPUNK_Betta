using UnityEngine;
using UnityEngine.Events;


namespace Abstracts
{

    public interface IStoreView : IView
    {

        public GameObject PassiveSkillsGroupUI { get; }

        public GameObject AssistItemGroupUI { get; }

        public GameObject WeaponGroupUI { get; }

        public GameObject ArmorGroupUI { get; }

        public void SubscribeClickButtons(UnityAction onClickButtonBack);

    }

}