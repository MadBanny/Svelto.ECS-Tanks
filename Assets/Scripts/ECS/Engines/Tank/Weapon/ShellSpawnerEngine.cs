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
                        Charge(tankEntityView.TankInputComponent,tankWeaponEntityView.TransformComponent, tankWeaponEntityView.LaunchForceComponent, _Time);
                    }
                }
                yield return null;
            }
        }

        private void Charge(ITankInputComponent input,ITransformComponent transform, ILaunchForceComponent launchForce, ITime time)
        {
            float chargeSpeed = (launchForce.MaxLaunchForce - launchForce.MinLaunchForce) / launchForce.MaxChargeTime;

            if (launchForce.CurrentLaunchForce >= launchForce.MaxLaunchForce && !input.Fired)
            {
                launchForce.CurrentLaunchForce = launchForce.MaxLaunchForce;
                Fire(transform, launchForce.CurrentLaunchForce);
                input.Fired = true;
                launchForce.CurrentLaunchForce = launchForce.MinLaunchForce;
            }
            else if (input.GetFireButtonDown)
            {
                input.Fired = false;
                launchForce.CurrentLaunchForce = launchForce.MinLaunchForce;
            }
            else if (input.GetFireButton && !input.Fired)
            {
                launchForce.CurrentLaunchForce += chargeSpeed * time.DeltaTime;
            }
            else if (input.GetFireButtonUp && !input.Fired)
            {
                Fire(transform, launchForce.CurrentLaunchForce);
                input.Fired = true;
                launchForce.CurrentLaunchForce = launchForce.MinLaunchForce;
            }
        }

        private void Fire(ITransformComponent transform, float lauchForce)
        {
            GameObject go = _GameObjectFactory.Build(_ShellPrefab);
            go.transform.position = transform.Position;
            go.transform.rotation = transform.Rotation;
            go.GetComponent<Rigidbody>().velocity = lauchForce * transform.Forward;
            _EntityFactory.BuildEntity<ShellEntityDescriptor>(go.GetInstanceID(), new object[] { });
        }
    }
}
