using System.Collections;
using Svelto.ECS;
using ECS.Tanks.Camera;
using ECS.Tanks.UI;
namespace ECS.Tanks.Tank
{
    public class TankEntityDescriptor : GenericEntityDescriptor<
        TankEntityView, 
        TankMovementSoundEntityView, 
        CameraTargetEntityView,
        HealthEntityView,
        HudDamageEntityView>
    {

    }
}
