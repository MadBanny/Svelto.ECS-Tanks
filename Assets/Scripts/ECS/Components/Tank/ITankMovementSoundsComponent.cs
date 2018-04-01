using UnityEngine;

namespace ECS.Tanks.Tank
{
    public interface ITankMovementSoundsComponent : IComponent
    {
        AudioClip IdleAudioClip { get; }
        AudioClip EngineDrivingAudioClip { get; }
    }
}
