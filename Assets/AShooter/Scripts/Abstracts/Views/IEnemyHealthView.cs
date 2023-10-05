using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Abstracts
{

    public interface IEnemyHealthView : IView
    {

        void Deactivate();

        void RefreshHealth(float currentHealth, float maxHealth);

 
    }
}
