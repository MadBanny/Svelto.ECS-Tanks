using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Tank
{
    public class TankInputImplementor : IImplementor, ITankInputComponent
    {
        public Vector3 Input { get; set; }
        public bool Fire { get; set; }
    }
}