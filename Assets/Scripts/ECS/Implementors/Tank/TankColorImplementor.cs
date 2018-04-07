using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Tank
{
    public class TankColorImplementor : MonoBehaviour, IImplementor, IColorComponent
    {
        public Color Color {
            set {
                //Not recommended
                foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
                {
                    mesh.material.color = value;
                }
            }
        }

    }
}
