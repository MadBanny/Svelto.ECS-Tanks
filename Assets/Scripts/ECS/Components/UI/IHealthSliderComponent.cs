using UnityEngine;
using System.Collections;

namespace ECS.Tanks.UI
{
    public interface IHealthSliderComponent : IComponent
    {
        int Value { set; }
        Color FillImageColor { set; }
    }
}
