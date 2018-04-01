using UnityEngine;
using System.Collections;
using Svelto.ECS;
using Svelto.Tasks;
using Svelto.DataStructures;

namespace ECS.Tanks.Tank
{
    public class TankMovementEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        private readonly ITime _Time;
        private readonly ITaskRoutine _TaskRoutine;

        public TankMovementEngine(ITime time)
        {
            _Time = time;
            //_TaskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Tick()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        public void Ready() {
            Tick().RunOnSchedule(StandardSchedulers.physicScheduler);
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                var tankEntityViews = entityViewsDB.QueryEntityViews<TankEntityView>();
                if (tankEntityViews.Count > 0)
                {
                    for (int i = 0; i < tankEntityViews.Count; i++)
                    {

                        Move(tankEntityViews[i]);
                        Turn(tankEntityViews[i]);
                    }
                    //_TankEntityView.TankInputComponent.Fire = Input.GetButton("Fire1");
                }
                yield return null;
            }
        }

        private void Move(TankEntityView tankEntityView)
        {
            Vector3 input = tankEntityView.TankInputComponent.Input;

            Vector3 movement = tankEntityView.TransformComponent.Forward * input.z * tankEntityView.SpeedComponent.Speed * _Time.DeltaTime;

            tankEntityView.TransformComponent.Position = tankEntityView.PositionComponent.Position + movement;
        }

        private void Turn(TankEntityView tankEntityView)
        {
            Vector3 input = tankEntityView.TankInputComponent.Input;

            float turn = input.x * tankEntityView.TurnSpeedComponent.TurnSpeed * _Time.DeltaTime;

            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            tankEntityView.TransformComponent.Rotation = tankEntityView.TransformComponent.Rotation * turnRotation;
        }

        public void Step(ref DamageInfo token, int condition)
        {

        }
    }
}
