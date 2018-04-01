using UnityEngine;
using System.Collections;

namespace ECS.Tanks
{
    public struct DamageInfo
    {
        public int DamagePerShot { get; private set; }
        public Vector3 DamagePoint { get; private set; }
        public int EntityDamagedID { get; private set; }

        public DamageInfo(int damage, Vector3 point, int entity) : this()
        {
            DamagePerShot = damage;
            DamagePoint = point;
            EntityDamagedID = entity;
        }
    }
}