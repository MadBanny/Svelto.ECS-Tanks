using UnityEngine;
using System.Collections;
using Svelto.ECS;
using Svelto.Tasks;

namespace ECS.Tanks.Tank
{
    public class TankShootingEffectsEngine : IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        public void Ready()
        {

        }
    }
}