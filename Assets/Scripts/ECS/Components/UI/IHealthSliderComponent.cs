using UnityEngine;
using System.Collections;

namespace ECS.Tanks.UI
{
    public interface IHealthSliderComponent : IComponent
    {
        float HealthSliderValue { set; }
        Color FillImageColor { set; }
    }
}
