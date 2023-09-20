using Abstracts;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(ChestDataConfig), menuName = "Config/" + nameof(ChestDataConfig))]
    public class ChestDataConfig : ScriptableObject
    {
        
        [field: SerializeField] public List<WeaponConfig> WeaponsPossibleGeneration { get; set; }
        [field: SerializeField] public List<ImprovableItemConfig> ImprovableItemsPossibleGeneration;
        [field: SerializeField] public List<int> MettaCoinsPossibleGeneration;
        [field: SerializeField] public List<float> HealthPossibleGeneration;


        public int COUNT_POSSIBLE_OBJECTS = 4; 


    }
}