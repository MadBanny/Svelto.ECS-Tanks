using Svelto.ECS;

namespace ECS.Tanks.UI
{
    public class TankHudEntityView : EntityView
    {
        public IHealthSliderComponent HealthSliderComponent;
        public IAimSliderComponent AimSliderComponent;
    }
}
