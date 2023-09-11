using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System;

public class ImproveItem : BaseImprovement, IItem 
{
    [SerializeField] private ImprovementTime _timeType;
    [SerializeField] private ImprovementType _improveType;
    [SerializeField] private float _valueMultiplier;
    [SerializeField] private float _timer = 0.0f;


    public void SetImprovement(ImprovementTime timeType, ImprovementType improveType, float value, float timer = 0.0f)
    {
        Type = improveType;
        Time = timeType;
        Value = value;
        Timer = timer;
    }

    
    public override void Improve(IImprovable improvable)
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        improvable.Apply(this);
    }

    public IItem RaiseItem() => this;


    private void Awake()
    => SetImprovement(_timeType, _improveType, _valueMultiplier, _timer);
    
}
