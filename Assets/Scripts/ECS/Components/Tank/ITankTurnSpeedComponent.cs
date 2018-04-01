using UnityEngine;
using System.Collections;

namespace ECS.Tanks.Tank
{
    public interface ITankTurnSpeedComponent : IComponent
    {
        float TurnSpeed { get; }
    }
}
