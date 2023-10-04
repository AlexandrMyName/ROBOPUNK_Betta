using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Abstracts
{

    public interface IEnemyViews
    {

        IEnemyHealthView Health { get; set; }


        void InitViews();
    }
}