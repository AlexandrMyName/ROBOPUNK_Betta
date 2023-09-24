using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(StoreItemDataConfig), menuName = "Config/" + nameof(StoreItemDataConfig))]
    public class StoreItemDataConfig : ScriptableObject
    {
        public string description;
        public string nameItem;
        public Sprite Icon;
        public int price;
        public float improvementCoefficient;
        public string unitImprovementCoefficient;
    }
}