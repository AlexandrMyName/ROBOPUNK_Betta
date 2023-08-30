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

        public PCInput( [NotNull] InputConfig config )
        {

            Horizontal = new PCInputHorizontal(config);
            Vertical = new PCInputVertical(config);
            LeftClick = new PCAttackInput();
            MousePosition = new PCMousePositionInput();
        }
    }
}