using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ItemConfigs), menuName = "Config/" + nameof(ItemConfigs))]
public class ItemConfigs : ScriptableObject
{
    public List<ImprovableItemConfig> ImprovableItems;


}
[Serializable]
public class ImprovableItemConfig
{
    public Sprite Icon;
    public ImprovementType ImprovableType;


}