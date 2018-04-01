using UnityEngine;
using System.Collections;
using Svelto.ECS;

namespace ECS.Tanks.Tank
{
    public class TankShootingEngine : IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        public TankShootingEngine()
        {

        }

        public void Ready()
        {
            Tick().Run();
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                var tankEntityViews = entityViewsDB.QueryEntityViews<TankEntityView>();

                if(tankEntityViews.Count > 0)
                {

                }

                yield return null;
            }
        }


    }
}
