using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Camera
{
    public class CameraImplementor : MonoBehaviour, IImplementor, ICameraComponent, IPositionComponent, ITransformComponent, ICameraRigComponent
    {

        [SerializeField]
        private UnityEngine.Camera _Camera;
        [SerializeField]
        private float _DumpTime;
        [SerializeField]
        private float _ScreenEdgeBuffer;
        [SerializeField]
        private float _MinSize;

        public float OrthographicSize {
            get { return _Camera.orthographicSize; }
            set { _Camera.orthographicSize = value; }
        }

        public bool Orthographic {
            get { return _Camera.orthographic; }
            set { _Camera.orthographic = value; }
        }

        public float Aspect {
            get { return _Camera.aspect; }
            set { _Camera.aspect = value; }
        }

        public Vector3 Position { get { return transform.position; } set { transform.position = value; } }
        public Quaternion Rotation { get { return transform.rotation; } set { transform.rotation = value; } }
        public Vector3 Forward { get { return transform.forward; } }

        public float DampTime { get { return _DumpTime; } set { _DumpTime = value; } }
        public float ScreenEdgeBuffer { get { return _ScreenEdgeBuffer; } set { _ScreenEdgeBuffer = value; } }
        public float MinSize { get { return _MinSize; } set { _MinSize = value; } }

        public Vector3 InverseTransformPoint(Vector3 position)
        {
            return transform.InverseTransformPoint(position);
        }
    }
}
