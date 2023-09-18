using Core.DTO;
using System.Collections.Generic;
using UnityEngine;


namespace Abstracts
{


    public interface IEnemy
    {
        EnemyType EnemyType { get; set; }

        EnemyState EnemyState { get; set; }

        IEnemyComponentsStore ComponentsStore { get; }

        Transform PlayerTransform { get; }

        SphereCollider EnemyRadiusAttack { get; }

        void SetSystems(List<ISystem> systems);

        void SetComponents(IEnemyComponentsStore components, float RangeRadius);
    }
}
