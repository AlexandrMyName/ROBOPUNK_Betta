using UnityEngine;


namespace Abstracts
{

    public interface IMainStoreView : IView
    {

        public GameObject PassiveSkillsGroupUI { get; }
        
        public GameObject WeaponsGroupUI { get; }

        public GameObject SkinsGroupUI { get; }
        
        
    }
}