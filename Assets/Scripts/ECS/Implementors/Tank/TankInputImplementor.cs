using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Tank
{
    public class TankInputImplementor : IImplementor, ITankInputComponent
    {
        public Vector3 Input { get; set; }
        public bool GetFireButton { get; set; }
        public bool GetFireButtonDown { get; set; }
        public bool GetFireButtonUp { get; set; }
        public bool Fired { get; set; }
    }
}