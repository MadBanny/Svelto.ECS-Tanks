using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Tank
{
    public class TankShootingImplementor : MonoBehaviour, IImplementor, ITransformComponent, ILaunchForceComponent
    {
        public Vector3 Position { get { return transform.position; }  set { transform.position = value; } }
        public Quaternion Rotation { get { return transform.rotation; } set { transform.rotation = value; } }
        public Vector3 Forward { get { return transform.forward; } }

        public float CurrentLaunchForce { get; set; }

        public float MinLaunchForce { get; set; } = 15f;
        public float MaxLaunchForce { get; set; } = 30f;
        public float MaxChargeTime { get; set; } = 0.75f;
    }
}
