using UnityEngine.UI;

namespace ECS.Tanks.UI
{
    public interface IAimSliderComponent : IComponent
    {
        float AimSliderValue { set; }
    }
}
