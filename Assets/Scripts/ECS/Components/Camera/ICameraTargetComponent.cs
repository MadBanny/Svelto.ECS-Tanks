using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Camera
{
    public interface ICameraTargetComponent : IComponent
    {
        Vector3 Position { get; }
        bool ActiveSelf { get; }
    }
}
