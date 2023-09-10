using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Core
{

    public class EnemyDistantMeleeAttackSystem : BaseSystem
    {

        private List<IDisposable> _disposables = new();


        protected override void Awake(IGameComponents components)
        {

        }


    }

}