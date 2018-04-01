using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Tank
{
    public class TankColorImplementor : MonoBehaviour, IImplementor, IColorComponent
    {
        public Color Color { get; set; }

        public void SetColor()
        {
            foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
            {
                mesh.material.color = Color;
            }
        }
    }
}
