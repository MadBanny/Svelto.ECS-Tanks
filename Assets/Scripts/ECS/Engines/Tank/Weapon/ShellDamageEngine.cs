using UnityEngine;
using System.Collections;
using Svelto.ECS;
namespace ECS.Tanks.Tank
{
    public class ShellDamageEngine : IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        private readonly ISequencer _DamageSequence;

        public ShellDamageEngine(ISequencer damageSequence)
        {
            _DamageSequence = damageSequence;
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
                var shellEntityViews = entityViewsDB.QueryEntityViews<ShellEntityView>();
                if(shellEntityViews.Count > 0)
                {
                    for(int i = 0; i < shellEntityViews.Count; i++)
                    {
                        DamageInfo damageInfo = new DamageInfo(10, new Vector3(), 0);
                        _DamageSequence.Next(this, ref damageInfo);
                    }
                }
                yield return null;
            }
        }
    }
}
