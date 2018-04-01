using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Camera
{
    public interface ICameraRigComponent : IComponent
    {
        float DampTime { get; set; }
        float ScreenEdgeBuffer { get; set; }
        float MinSize { get; set; }

        Vector3 InverseTransformPoint(Vector3 position);
    }
}
