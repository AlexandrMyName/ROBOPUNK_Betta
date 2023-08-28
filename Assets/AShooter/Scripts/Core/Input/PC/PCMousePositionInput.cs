using System;
using Abstracts;
using UniRx;
using UnityEngine;


namespace Core
{
    
    internal sealed class PCMousePositionInput : IUserInputProxy<Vector3>
    {
        
        public IObservable<Vector3> AxisOnChange { get; }
        
        private Camera MainCamera = Camera.main;


        public PCMousePositionInput()
        {
            AxisOnChange = Observable
                .EveryUpdate()
                .Select(_ => Input.mousePosition)
                .Select(mousePosition =>
                    MainCamera.ScreenToWorldPoint(
                        new Vector3(mousePosition.x,
                            mousePosition.y,
                            MainCamera.nearClipPlane)));
        }
        
        
    }
}