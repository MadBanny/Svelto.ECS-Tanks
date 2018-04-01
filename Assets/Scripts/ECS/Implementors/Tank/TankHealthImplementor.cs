using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Tank
{
    public class TankHealthImplementor : IImplementor, IHealthComponent
    {
        public int CurrentHealth { get; set; }

        public TankHealthImplementor(int health)
        {
            CurrentHealth = health;
        }
    }
}
