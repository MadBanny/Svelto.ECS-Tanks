using UnityEngine;
using System.Collections;

namespace ECS.Tanks.DataSources
{
    [System.Serializable]
    public class SpawnData
    {
        public Vector3 SpawnPosition { get { return _SpawnPoint.position; } }
        public Quaternion SpawnRotation { get { return _SpawnPoint.rotation; } }
        [SerializeField]
        private Transform _SpawnPoint;

        public Color TankColor;
    }
}
