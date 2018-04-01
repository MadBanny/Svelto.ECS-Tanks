using UnityEngine;

namespace ECS.Tanks
{
    public interface IPositionComponent : IComponent
    {
        Vector3 Position { get; }
    }
}