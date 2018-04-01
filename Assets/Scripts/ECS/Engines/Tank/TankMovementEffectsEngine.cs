using System.Collections;
using Svelto.ECS;
using Mathf = UnityEngine.Mathf;
using Random = UnityEngine.Random;

namespace ECS.Tanks.Tank
{
    public class TankMovementEffectsEngine : IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        public void Ready()
        {
            Tick().Run();
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                var tankMovementSoundEntityViews = entityViewsDB.QueryEntityViews<TankMovementSoundEntityView>();

                if(tankMovementSoundEntityViews.Count > 0)
                {
                    for (int i = 0; i < tankMovementSoundEntityViews.Count; i++)
                    {
                        IAudioSourceComponent audioSourceComponent = tankMovementSoundEntityViews[i].AudioSourceComponent;
                        ITankMovementSoundsComponent soundComponent = tankMovementSoundEntityViews[i].TankMovementSoundsComponent;
                        ITankInputComponent inputComponent = tankMovementSoundEntityViews[i].TankInputComponent;
                        if (Mathf.Abs(inputComponent.Input.z) < 0.1f && Mathf.Abs(inputComponent.Input.x) < 0.1f)
                        {
                            if (audioSourceComponent.Clip == soundComponent.EngineDrivingAudioClip)
                            {
                                audioSourceComponent.Clip = soundComponent.IdleAudioClip;
                                audioSourceComponent.Pitch = Random.Range(1 - 0.1f, 1 + 0.1f);
                                audioSourceComponent.Play();
                            }
                        }
                        else
                        {
                            if (audioSourceComponent.Clip == soundComponent.IdleAudioClip)
                            {
                                // ... change the clip to driving and play.
                                audioSourceComponent.Clip = soundComponent.EngineDrivingAudioClip;
                                audioSourceComponent.Pitch = Random.Range(1 - 0.1f, 1 + 0.1f);
                                audioSourceComponent.Play();
                            }
                        }
                    }
                }

                yield return null;
            }
        }
    }
}
