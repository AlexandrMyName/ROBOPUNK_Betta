using Abstracts;
using System;
using UnityEngine;


namespace Core
{

    [Serializable]
    public class EnemyBossComponent : IEnemyBossComponent
    {

       [field: SerializeField] public TrailRenderer HealTrail { get; set; }
    }
}