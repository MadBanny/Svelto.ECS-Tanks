using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Tank
{
    public class TankShootingImplementor : MonoBehaviour, IImplementor, IPositionComponent
    {
        public Vector3 Position { get { return transform.position; }  set { transform.position = value; } }
    }
}
