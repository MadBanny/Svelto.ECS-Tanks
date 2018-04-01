using UnityEngine;
using System.Collections;

namespace ECS.Tanks
{
    public interface IHealthComponent : IComponent
    {
        int CurrentHealth { get; set; }
    }
}
