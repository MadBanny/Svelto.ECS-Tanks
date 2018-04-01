using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Svelto.ECS;
using Svelto.Tasks;
using Svelto.Factories;
using ECS.Tanks.UI;

namespace ECS.Tanks.Tank
{
    public class TankSpawnerEngine : IEngine
    {
        private readonly IGameObjectFactory _GameObjectFactory;
        private readonly IEntityFactory _EntityFactory;
        private int _NumberOfTanksToSpawn = 2;

        public TankSpawnerEngine(IGameObjectFactory gameObjectFactory, IEntityFactory entityFactory)
        {
            _GameObjectFactory = gameObjectFactory;
            _EntityFactory = entityFactory;

            Spawn();
        }

        private void Spawn()
        {
            GameObject tankPrefab = Resources.Load<GameObject>("Tank");
            //Temp
            DataSources.SpawnDataSource spawnDataSource = Object.FindObjectOfType<DataSources.SpawnDataSource>();

            for (int i = 0; i < spawnDataSource.SpawnData.Length; i++)
            {
                //Build tank entity
                GameObject tank = _GameObjectFactory.Build(tankPrefab);

                List<IImplementor> implementors = new List<IImplementor>();
                //!!!
                tank.transform.position = spawnDataSource.SpawnData[i].SpawnPosition;
                tank.transform.rotation = spawnDataSource.SpawnData[i].SpawnRotation;
                //!!!
                TankColorImplementor colorImplementor = tank.GetComponentInChildren<TankColorImplementor>();
                colorImplementor.Color = spawnDataSource.SpawnData[i].TankColor;
                colorImplementor.SetColor();

                tank.GetComponents(implementors);
                implementors.Add(colorImplementor);
                implementors.Add(new TankInputImplementor());
                implementors.Add(new TankHealthImplementor(100));

                _EntityFactory.BuildEntity<TankEntityDescriptor>(tank.GetInstanceID(), implementors.ToArray());

                //Build tank weapon entity
                TankShootingImplementor weapon = tank.GetComponentInChildren<TankShootingImplementor>();
                _EntityFactory.BuildEntity<TankWeaponEntityDescriptor>(weapon.GetInstanceID(), new object[] { weapon });

                //Build tank HUD
                TankHudImplementor hudImplementor = tank.GetComponentInChildren<TankHudImplementor>();
                _EntityFactory.BuildEntity<TankHudEntityDescriptor>(hudImplementor.GetInstanceID(), new object[] { hudImplementor });
                /*TankHudEntityDescriptorHolder hudEntityHolder = tank.GetComponentInChildren<TankHudEntityDescriptorHolder>();
                IEntityDescriptorInfo hudEntityDescriptor = hudEntityHolder.RetrieveDescriptor();
                _EntityFactory.BuildEntity(hudEntityHolder.GetInstanceID(), hudEntityDescriptor, hudEntityHolder.GetComponentsInChildren<IImplementor>());*/

            }
        }
    }
}
