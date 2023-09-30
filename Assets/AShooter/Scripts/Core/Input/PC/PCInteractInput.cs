using System;
using System.Collections.Generic;
using Abstracts;
using UniRx;
using Zenject;


namespace Core
{

    internal sealed class PCInteractInput : ISubjectInputProxy<Unit>
    {

        private IInteractable _interactable;

        public PCInteractInput(InputConfig config, IInteractable interactable)
        {
            _interactable = interactable;
            config.Interact.Key.performed += ctx => _interactable.Interact();
        }

        public Subject<Unit> AxisOnChange { get; }
    }
}