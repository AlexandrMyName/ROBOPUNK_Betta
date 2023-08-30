using Abstracts;
using JetBrains.Annotations;
using UnityEngine;

namespace Core
{
    public sealed class PCInput : IInput
    {
        public IUserInputProxy<float> Horizontal { get; }
        public IUserInputProxy<float> Vertical { get; }
        public IUserInputProxy<bool> LeftClick { get; }
        public IUserInputProxy<Vector3> MousePosition { get; }

        public PCInput( [NotNull] InputConfig cnf )
        {

            Horizontal = new PCInputHorizontal(cnf);
            Vertical = new PCInputVertical(cnf);
            LeftClick = new PCAttackInput();
            MousePosition = new PCMousePositionInput();
        }
    }
}