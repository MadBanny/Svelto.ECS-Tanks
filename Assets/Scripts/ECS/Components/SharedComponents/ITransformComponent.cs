using UnityEngine;
using System.Collections;

namespace ECS.Tanks
{
    public interface ITransformComponent : IPositionComponent
    {
        new Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 Forward { get; }
    }
}
