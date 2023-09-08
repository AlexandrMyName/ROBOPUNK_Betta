using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;

public class ImproveItem : BaseImprovement, IItem 
{
    public void SetImprovement(ImprovementTime timeType, ImprovementType improveType, float value, float timer = 0.0f)
    {
        Type = improveType;
        Time = timeType;
        Value = value;
        Timer = timer;
        
    }
    private void Awake()
    {
        Type = ImprovementType.Attackable;
        Time = ImprovementTime.Temporary;
        Value = 20f;
        Timer = 10f;
    }
    public override void Improve(IImprovable improvable)
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        improvable.Apply(this);
    }

    public IItem RaiseItem() => this;
     
}
