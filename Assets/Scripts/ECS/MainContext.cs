using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Svelto.ECS;
using Svelto.Context;
using Svelto.Tasks;
using Svelto.ECS.Schedulers.Unity;

using ECS.Tanks.Tank;
using ECS.Tanks.Camera;
using ECS.Tanks.UI;

namespace ECS.Tanks
{
    [AddComponentMenu("ECS/Tanks/Main Context")]
    public class MainContext : UnityContext<Main> { }

    public class Main : ICompositionRoot
    {
        private EnginesRoot _EnginesRoot;
        private IEntityFactory _EntityFactory;

        public Main()
        {
            SetupEnginesAndEntities();
        }

        public void OnContextCreated(UnityContext contextHolder)
        {
            //BuildEntitiesFromScene(contextHolder);
            SetupCamera(contextHolder);
            SetupHud();
        }

        public void OnContextDestroyed()
        {
            _EnginesRoot.Dispose();
            TaskRunner.StopAndCleanupAllDefaultSchedulers();
        }

        public void OnContextInitialized()
        {

        }

        private void SetupEnginesAndEntities()
        {
            _EnginesRoot = new EnginesRoot(new UnitySumbmissionEntityViewScheduler());
            _EntityFactory = _EnginesRoot.GenerateEntityFactory();
            IEntityFunctions entityFunctions = _EnginesRoot.GenerateEntityFunctions();
            GameObjectFactory gameObjectFactory = new GameObjectFactory();

            Sequencer damageSequence = new Sequencer();
            Sequencer roundSequence = new Sequencer();

            ITime time = new Time();

            //Construct Tank(Player) engines
            TankSpawnerEngine tankSpawnerEngine = new TankSpawnerEngine(gameObjectFactory, _EntityFactory);
            TankMovementEngine tankMovementEngine = new TankMovementEngine(time);
            TankHudEngine tankHudEngine = new TankHudEngine();
            HealthEngine healthEngine = new HealthEngine(damageSequence);

            //Construct Weapon engines
            ShellSpawnerEngine shellSpawnerEngine = new ShellSpawnerEngine(gameObjectFactory, _EntityFactory, time);
            ShellDamageEngine shellDamageEngine = new ShellDamageEngine(damageSequence);

            //Construct Camera engines
            CameraEngine cameraEngine = new CameraEngine(time);

            //Add tanks engines
            _EnginesRoot.AddEngine(tankSpawnerEngine);
            _EnginesRoot.AddEngine(new TankInputEngine());
            _EnginesRoot.AddEngine(tankMovementEngine);
            _EnginesRoot.AddEngine(new TankMovementEffectsEngine());
            _EnginesRoot.AddEngine(tankHudEngine);
            _EnginesRoot.AddEngine(healthEngine);

            //Add weapon engines
            _EnginesRoot.AddEngine(new TankShootingEngine());
            _EnginesRoot.AddEngine(shellSpawnerEngine);
            _EnginesRoot.AddEngine(shellDamageEngine);

            //Add camera engines
            _EnginesRoot.AddEngine(cameraEngine);

            damageSequence.SetSequence(
                new Steps
                {
                    {shellDamageEngine,
                        new To{
                            new IStep[] { healthEngine }
                        }
                    },
                    {healthEngine,
                        new To{
                            { (int)DamageCondition.DAMAGE,
                                new IStep[] {tankHudEngine}
                            },
                            { (int)DamageCondition.DEAD,
                                new IStep[] {tankMovementEngine}
                            }
                        }
                    }
                }
            );
        }

        private void SetupCamera(UnityContext contextHolder)
        {
            CameraEntityDescriptorHolder cameraEntityDescriptor = contextHolder.GetComponentInChildren<CameraEntityDescriptorHolder>();
            EntityDescriptorInfo entityDescriptor = cameraEntityDescriptor.RetrieveDescriptor();
            _EntityFactory.BuildEntity(
                cameraEntityDescriptor.gameObject.GetInstanceID(),
                entityDescriptor,
                cameraEntityDescriptor.GetComponentsInChildren<IImplementor>()
            );
        }

        private void SetupHud()
        {

        }

        private void BuildEntitiesFromScene(UnityContext contextHolder)
        {
            /*IEntityDescriptorHolder[] entities = contextHolder.GetComponentsInChildren<IEntityDescriptorHolder>();

            for (int i = 0; i < entities.Length; i++)
            {
                var entityDescriptorHolder = entities[i];
                var entityDescriptor = entityDescriptorHolder.RetrieveDescriptor();
                _EntityFactory.BuildEntity
                (((MonoBehaviour)entityDescriptorHolder).gameObject.GetInstanceID(),
                    entityDescriptor,
                    (entityDescriptorHolder as MonoBehaviour).GetComponentsInChildren<IImplementor>());
            }*/
        }
    }
}
