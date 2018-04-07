using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Tanks.Tank
{
    public interface ILaunchForceComponent : IComponent
    {
        float CurrentLaunchForce { get; set; }
        float MinLaunchForce { get; set; }
        float MaxLaunchForce { get; set; }
        float MaxChargeTime { get; set; }

    }
}
