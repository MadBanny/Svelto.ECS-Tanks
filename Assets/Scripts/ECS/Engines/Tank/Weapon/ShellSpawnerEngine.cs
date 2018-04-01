using UnityEngine;
using System.Collections;
using Svelto.ECS;
using Svelto.Factories;

namespace ECS.Tanks.Tank
{
    public class ShellSpawnerEngine : IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        private readonly IEntityFactory _EntityFactory;
        private readonly IGameObjectFactory _GameObjectFactory;
        private readonly ITime _Time;

        private GameObject _ShellPrefab;

        public ShellSpawnerEngine(IGameObjectFactory gameObjectFactory, IEntityFactory entityFactory, ITime time)
        {
            _GameObjectFactory = gameObjectFactory;
            _EntityFactory = entityFactory;
            _Time = time;
        }

        public void Ready()
        {
            Tick().Run();
            _ShellPrefab = Resources.Load<GameObject>("Shell");
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                var tankEntityViews = entityViewsDB.QueryEntityViews<TankEntityView>();
                var tankWeaponEntityViews = entityViewsDB.QueryEntityViews<TankWeaponEntityView>();

                if (tankWeaponEntityViews.Count > 0)
                {
                    for(int i = 0; i < tankWeaponEntityViews.Count; i++)
                    {
                        TankWeaponEntityView tankWeaponEntityView = tankWeaponEntityViews[i];
                        TankEntityView tankEntityView = tankEntityViews[i];
                        if (tankEntityView.TankInputComponent.Fire)
                        {
                            GameObject go = _GameObjectFactory.Build(_ShellPrefab);
                            go.transform.position = tankWeaponEntityView.PositionComponent.Position;

                            _EntityFactory.BuildEntity<ShellEntityDescriptor>(go.GetInstanceID(), new object[] { });
                        }
                    }
                }
                yield return null;
            }
        }
    }
}
