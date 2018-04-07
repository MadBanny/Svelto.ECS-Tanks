using UnityEngine;
using System.Collections;
using Svelto.ECS;
using Svelto.Tasks;
using Svelto.DataStructures;

namespace ECS.Tanks.Tank
{
    public class TankInputEngine : IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        private readonly ITaskRoutine _TaskRoutine;

        public TankInputEngine()
        {
            //_TaskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Tick());
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
                        ITankInputComponent tankInput = tankEntityViews[i].TankInputComponent;
                        int tankNumber = 1;
                        tankNumber += i;
                        float horizontalAxis = Input.GetAxis("Horizontal" + tankNumber);
                        float verticalAxis = Input.GetAxis("Vertical" + tankNumber);

                        tankInput.Input = new Vector3(horizontalAxis, 0f, verticalAxis);

                        string fireButtonName = "Fire" + tankNumber;
                        tankInput.GetFireButton = Input.GetButton(fireButtonName);
                        tankInput.GetFireButtonDown = Input.GetButtonDown(fireButtonName);
                        tankInput.GetFireButtonUp = Input.GetButtonUp(fireButtonName);
                    }
                }

                yield return null;
            }
           
        }
    }
}
