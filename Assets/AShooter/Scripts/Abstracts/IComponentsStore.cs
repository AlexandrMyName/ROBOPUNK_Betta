using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abstracts
{
    public interface IComponentsStore
    {
        IAttackable Attackable { get; }
        IMovable Movable { get; }
    }
}