using System.Collections;
using Svelto.ECS;

namespace ECS.Tanks.Tank
{
    public class TankWeaponEntityView : EntityView
    {
        public ITransformComponent TransformComponent;
        public ILaunchForceComponent LaunchForceComponent;
    }
}
