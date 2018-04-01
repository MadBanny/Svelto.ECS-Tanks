﻿using System;
using System.Collections.Generic;
using DBC;
using Svelto.DataStructures;
using Svelto.ECS.Internal;

#if ENGINE_PROFILER_ENABLED && UNITY_EDITOR
using Svelto.ECS.Profiler;
#endif

namespace Svelto.ECS
{
    public partial class EnginesRoot : IDisposable
    {
        public void Dispose()
        {
            foreach (var entity in _entityViewsDB)
                if (entity.Value.isQueryiableEntityView)
                    foreach (var entityView in entity.Value)
                        RemoveEntityViewFromEngines(_entityViewEngines, entityView as EntityView, entity.Key);

            foreach (var entity in _metaEntityViewsDB)
            {
                foreach (var entityView in entity.Value)
                    RemoveEntityViewFromEngines(_entityViewEngines, entityView as EntityView, entity.Key);
            }
        }

        /// <summary>
        ///     an EnginesRoot reference cannot be held by anything else than the Composition Root
        ///     where it has been created. IEntityFactory and IEntityFunctions allow a weakreference
        ///     of the EnginesRoot to be passed around.
        /// </summary>
        /// <returns></returns>
        public IEntityFactory GenerateEntityFactory()
        {
            return new GenericEntityFactory(new DataStructures.WeakReference<EnginesRoot>(this));
        }

        public IEntityFunctions GenerateEntityFunctions()
        {
            return new GenericEntityFunctions(new DataStructures.WeakReference<EnginesRoot>(this));
        }

        /// <summary>
        ///     The EntityDescriptor doesn't need to be ever instantiated. It just describes the Entity
        ///     itself in terms of EntityViews to build. The Implementors are passed to fill the
        ///     references of the EntityViews components. Please read the articles on my blog
        ///     to understand better the terminologies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityID"></param>
        /// <param name="implementors"></param>
        void BuildEntity<T>(int entityID, object[] implementors = null) where T : IEntityDescriptor, new()
        {
            EntityFactory.BuildEntityViews
                (entityID, _entityViewsToAdd.current, EntityDescriptorTemplate<T>.Default, implementors);
        }

        /// <summary>
        ///     When the type of the entity is not known (this is a special case!) an EntityDescriptorInfo
        ///     can be built in place of the generic parameter T.
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="entityDescriptor"></param>
        /// <param name="implementors"></param>
        void BuildEntity(int entityID, IEntityDescriptorInfo entityDescriptor, object[] implementors)
        {
            EntityFactory.BuildEntityViews
                (entityID, _entityViewsToAdd.current, entityDescriptor, implementors);
        }

        /// <summary>
        ///     A meta entity is a way to manage a set of entitites that are not easily
        ///     queriable otherwise. For example you may want to group existing entities
        ///     by size and type and then use the meta entity entityView to manage the data
        ///     shared among the single entities of the same type and size. This will
        ///     prevent the scenario where the coder is forced to parse all the entities to
        ///     find the ones of the same size and type.
        ///     Since the entities are managed through the shared entityView, the same
        ///     shared entityView must be found on the single entities of the same type and size.
        ///     The shared entityView of the meta entity is then used by engines that are meant
        ///     to manage a group of entities through a single entityView.
        ///     The same engine can manage several meta entities entityViews too.
        ///     The Engine manages the logic of the Meta EntityView data and other engines
        ///     can read back this data through the normal entity as the shared entityView
        ///     will be present in their descriptor too.
        ///     It's a way to control a group of Entities through a entityView only.
        ///     This set of entities can share exactly the same entityView reference if
        ///     built through this function. In this way, if you need to set a variable
        ///     on a group of entities, instead to inject N entityViews and iterate over
        ///     them to set the same value, you can inject just one entityView, set the value
        ///     and be sure that the value is shared between entities.
        /// </summary>
        /// <param name="metaEntityID"></param>
        /// <param name="ed"></param>
        /// <param name="implementors"></param>
        void BuildMetaEntity<T>(int metaEntityID, object[] implementors) where T : IEntityDescriptor, new()
        {
            EntityFactory.BuildEntityViews(metaEntityID, _metaEntityViewsToAdd.current,
                                           EntityDescriptorTemplate<T>.Default, implementors);
        }

        /// <summary>
        ///     Using this function is like building a normal entity, but the entityViews
        ///     are grouped by groupID to be more efficently processed inside engines and
        ///     improve cache locality. Either class entityViews and struct entityViews can be
        ///     grouped.
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="groupID"></param>
        /// <param name="ed"></param>
        /// <param name="implementors"></param>
        void BuildEntityInGroup<T>(int entityID, int groupID, object[] implementors = null)
            where T : IEntityDescriptor, new()
        {
            EntityFactory.BuildGroupedEntityViews(entityID, groupID,
                                                  _groupedEntityViewsToAdd.current,
                                                  _entityViewsToAdd.current,
                                                  EntityDescriptorTemplate<T>.Default,
                                                  implementors);
        }

        void BuildEntityInGroup(int      entityID, int groupID, IEntityDescriptorInfo entityDescriptor,
                                object[] implementors = null)
        {
            EntityFactory.BuildGroupedEntityViews(entityID, groupID,
                                                  _groupedEntityViewsToAdd.current,
                                                  _entityViewsToAdd.current,
                                                  entityDescriptor, implementors);
        }

        void Preallocate<T>(int size) where T : IEntityDescriptor, new()
        {
            var entityViewsToBuild = ((EntityDescriptorInfo) EntityDescriptorTemplate<T>.Default).entityViewsToBuild;
            var count              = entityViewsToBuild.Length;

            for (var index = 0; index < count; index++)
            {
                var entityViewBuilder = entityViewsToBuild[index];
                var entityViewType    = entityViewBuilder.GetEntityViewType();

                ITypeSafeList dbList;
                if (_entityViewsDB.TryGetValue(entityViewType, out dbList) == false)
                    _entityViewsDB[entityViewType] = entityViewBuilder.Preallocate(ref dbList, size);
                else
                    dbList.ReserveCapacity(size);

                if (_entityViewsToAdd.current.TryGetValue(entityViewType, out dbList) == false)
                    _entityViewsToAdd.current[entityViewType] = entityViewBuilder.Preallocate(ref dbList, size);
                else
                    dbList.ReserveCapacity(size);
            }
        }

        void RemoveEntity(ref EntityInfoView                    entityInfoView, Dictionary<Type, ITypeSafeList> viewsDB,
                          Dictionary<Type, ITypeSafeDictionary> entityViewsDBDic)
        {
            if (entityInfoView.isInAGroup)
                InternalRemoveFromGroupAndDBAndEngines(entityInfoView.entityViews, entityInfoView.ID,
                                                       entityInfoView.groupID, viewsDB, entityViewsDBDic);
            else
                InternalRemoveFromDBAndEngines(entityInfoView.entityViews, entityInfoView.ID, viewsDB,
                                               entityViewsDBDic);
        }

        void RemoveEntity(int entityID)
        {
            var entityInfoView = _DB.QueryEntityView<EntityInfoView>(entityID);

            RemoveEntity(ref entityInfoView, _entityViewsDB, _entityViewsDBDic);
        }

        void RemoveMetaEntity(int metaEntityID)
        {
            var entityInfoView = _DB.QueryMetaEntityView<EntityInfoView>(metaEntityID);

            RemoveEntity(ref entityInfoView, _metaEntityViewsDB, _metaEntityViewsDBDic);
        }

        void RemoveGroupAndEntitiesFromDB(int groupID)
        {
            foreach (var group in _groupEntityViewsDB[groupID])
            {
                var entityViewType = group.Key;

                int count;
                var entities = group.Value.ToArrayFast(out count);

                for (var i = 0; i < count; i++)
                {
                    var entityID = entities[i].ID;

                    InternalRemoveEntityViewFromDBAndEngines(_entityViewsDB, _entityViewsDBDic, entityViewType,
                                                             entityID);
                }
            }

            _groupEntityViewsDB.Remove(groupID);
        }

        void InternalRemoveEntityViewFromDBAndEngines(Dictionary<Type, ITypeSafeList>       entityViewsDB,
                                                      Dictionary<Type, ITypeSafeDictionary> entityViewsDBDic,
                                                      Type                                  entityViewType,
                                                      int                                   entityID)
        {
            var entityViews = entityViewsDB[entityViewType];
            if (entityViews.MappedRemove(entityID) == false)
                entityViewsDB.Remove(entityViewType);

            if (entityViews.isQueryiableEntityView)
            {
                var typeSafeDictionary = entityViewsDBDic[entityViewType];
                var entityView         = typeSafeDictionary.GetIndexedEntityView(entityID);

                if (typeSafeDictionary.Remove(entityID) == false)
                    entityViewsDBDic.Remove(entityViewType);

                for (var current = entityViewType; current != _entityViewType; current = current.BaseType)
                    RemoveEntityViewFromEngines(_entityViewEngines, entityView, current);
            }
        }

        void SwapEntityGroup(int entityID, int fromGroupID, int toGroupID)
        {
            Check.Require(fromGroupID != toGroupID,
                          "can't move an entity to the same group where it already belongs to");

            var entityViewBuilders      = _DB.QueryEntityView<EntityInfoView>(entityID).entityViews;
            var entityViewBuildersCount = entityViewBuilders.Length;

            var groupedEntities = _groupEntityViewsDB[fromGroupID];

            Dictionary<Type, ITypeSafeList> groupedEntityViewsTyped;
            if (_groupEntityViewsDB.TryGetValue(toGroupID, out groupedEntityViewsTyped) == false)
            {
                groupedEntityViewsTyped = new Dictionary<Type, ITypeSafeList>();

                _groupEntityViewsDB.Add(toGroupID, groupedEntityViewsTyped);
            }

            for (var i = 0; i < entityViewBuildersCount; i++)
            {
                var entityViewBuilder = entityViewBuilders[i];
                var entityViewType    = entityViewBuilder.GetEntityViewType();

                var           fromSafeList = groupedEntities[entityViewType];
                ITypeSafeList toSafeList;

                if (groupedEntityViewsTyped.TryGetValue(entityViewType, out toSafeList) == false)
                    groupedEntityViewsTyped[entityViewType] = toSafeList = fromSafeList.Create();

                entityViewBuilder.MoveEntityView(entityID, fromSafeList, toSafeList);

                fromSafeList.MappedRemove(entityID);
            }

            var entityInfoView = _DB.QueryEntityView<EntityInfoView>(entityID);
            entityInfoView.groupID = toGroupID;
        }

        void InternalRemoveFromDBAndEngines(IEntityViewBuilder[]                  entityViewBuilders, int entityID,
                                            Dictionary<Type, ITypeSafeList>       entityViewsDB,
                                            Dictionary<Type, ITypeSafeDictionary> entityViewsDBDic)
        {
            var entityViewBuildersCount = entityViewBuilders.Length;

            for (var i = 0; i < entityViewBuildersCount; i++)
            {
                var entityViewType = entityViewBuilders[i].GetEntityViewType();

                InternalRemoveEntityViewFromDBAndEngines(entityViewsDB, entityViewsDBDic, entityViewType, entityID);
            }
            
            InternalRemoveEntityViewFromDBAndEngines(entityViewsDB, entityViewsDBDic, typeof(EntityInfoView), entityID);
        }

        void InternalRemoveFromGroupAndDBAndEngines(IEntityViewBuilder[]                  entityViewBuilders,
                                                    int                                   entityID, int groupID,
                                                    Dictionary<Type, ITypeSafeList>       entityViewsDB,
                                                    Dictionary<Type, ITypeSafeDictionary> entityViewsDBDic)
        {
            InternalRemoveFromGroupDB(entityViewBuilders, entityID, groupID);

            InternalRemoveFromDBAndEngines(entityViewBuilders, entityID, entityViewsDB, entityViewsDBDic);
        }

        void InternalRemoveFromGroupDB(IEntityViewBuilder[] entityViewBuilders, int entityID, int groupID)
        {
            var entityViewBuildersCount = entityViewBuilders.Length;

            var dictionary = _groupEntityViewsDB[groupID];

            for (var i = 0; i < entityViewBuildersCount; i++)
            {
                var entityViewType = entityViewBuilders[i].GetEntityViewType();

                var typeSafeList = dictionary[entityViewType];
                typeSafeList.MappedRemove(entityID);
            }
        }

        static void RemoveEntityViewFromEngines(Dictionary<Type, FasterList<IHandleEntityViewEngine>> entityViewEngines,
                                                IEntityView                                           entityView,
                                                Type                                                  entityViewType)
        {
            FasterList<IHandleEntityViewEngine> enginesForEntityView;

            if (entityViewEngines.TryGetValue(entityViewType, out enginesForEntityView))
            {
                int count;
                var fastList = FasterList<IHandleEntityViewEngine>.NoVirt.ToArrayFast(enginesForEntityView, out count);

                for (var j = 0; j < count; j++)
                {
#if ENGINE_PROFILER_ENABLED && UNITY_EDITOR
                    EngineProfiler.MonitorRemoveDuration(fastList[j], entityView);
#else
                    fastList[j].Remove(entityView);
#endif
                }
            }
        }

        class GenericEntityFactory : IEntityFactory
        {
            readonly DataStructures.WeakReference<EnginesRoot> _weakEngine;

            public GenericEntityFactory(DataStructures.WeakReference<EnginesRoot> weakReference)
            {
                _weakEngine = weakReference;
            }

            public void BuildEntity<T>(int entityID, object[] implementors) where T : IEntityDescriptor, new()
            {
                _weakEngine.Target.BuildEntity<T>(entityID, implementors);
            }

            public void BuildEntity(int entityID, IEntityDescriptorInfo entityDescriptor, object[] implementors = null)
            {
                _weakEngine.Target.BuildEntity(entityID, entityDescriptor, implementors);
            }

            public void BuildMetaEntity<T>(int metaEntityID, object[] implementors) where T : IEntityDescriptor, new()
            {
                _weakEngine.Target.BuildMetaEntity<T>(metaEntityID, implementors);
            }

            public void BuildEntityInGroup<T>(int entityID, int groupID, object[] implementors)
                where T : IEntityDescriptor, new()
            {
                _weakEngine.Target.BuildEntityInGroup<T>(entityID, groupID, implementors);
            }

            public void BuildEntityInGroup(int      entityID, int groupID, IEntityDescriptorInfo entityDescriptor,
                                           object[] implementors)
            {
                _weakEngine.Target.BuildEntityInGroup(entityID, groupID, entityDescriptor, implementors);
            }

            public void PreallocateEntitySlots<T>(int size) where T : IEntityDescriptor, new()
            {
                _weakEngine.Target.Preallocate<T>(size);
            }
        }

        class GenericEntityFunctions : IEntityFunctions
        {
            readonly DataStructures.WeakReference<EnginesRoot> _weakReference;

            public GenericEntityFunctions(DataStructures.WeakReference<EnginesRoot> weakReference)
            {
                _weakReference = weakReference;
            }

            public void RemoveEntity(int entityID)
            {
                _weakReference.Target.RemoveEntity(entityID);
            }

            public void RemoveMetaEntity(int metaEntityID)
            {
                _weakReference.Target.RemoveEntity(metaEntityID);
            }

            public void RemoveGroupAndEntities(int groupID)
            {
                _weakReference.Target.RemoveGroupAndEntitiesFromDB(groupID);
            }

            public void SwapEntityGroup(int entityID, int fromGroupID, int toGroupID)
            {
                _weakReference.Target.SwapEntityGroup(entityID, fromGroupID, toGroupID);
            }
        }

        readonly EntityViewsDB _DB;

        readonly Dictionary<Type, ITypeSafeList> _entityViewsDB;

        readonly Dictionary<Type, ITypeSafeDictionary>            _entityViewsDBDic;
        readonly Dictionary<int, Dictionary<Type, ITypeSafeList>> _groupEntityViewsDB;
        readonly Dictionary<Type, ITypeSafeList>                  _metaEntityViewsDB;
        readonly Dictionary<Type, ITypeSafeDictionary>            _metaEntityViewsDBDic;

    }
}