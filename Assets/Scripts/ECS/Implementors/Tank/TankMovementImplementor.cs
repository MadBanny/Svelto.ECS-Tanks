using UnityEngine;
using System.Collections;

using ECS.Tanks.Camera;

namespace ECS.Tanks.Tank
{
    [RequireComponent(typeof(Rigidbody))]
    public class TankMovementImplementor : MonoBehaviour,
        IImplementor,
        ITransformComponent,
        IPositionComponent,
        ICameraTargetComponent,
        ISpeedComponent,
        ITankTurnSpeedComponent
    {
        [SerializeField]
        private float _Speed = 12f;
        [SerializeField]
        private float _TurnSpeed = 180f;

        private Rigidbody _TankRigidbody;

        public Quaternion Rotation { get { return _TankRigidbody.rotation; } set { _TankRigidbody.MoveRotation(value); } }

        public float Speed { get { return _Speed; } }
        public float TurnSpeed {get{ return _TurnSpeed; } }

        public Vector3 Position { get { return _TankRigidbody.position; } set { _TankRigidbody.MovePosition(value); } }
        public Vector3 Forward { get { return _TankRigidbody.transform.forward; } }

        public bool ActiveSelf { get { return _TankRigidbody.gameObject.activeSelf; } }

        private void Awake()
        {
            _TankRigidbody = GetComponent<Rigidbody>();
        }
    }
}
