using UnityEngine;
using System.Collections;
using Svelto.ECS;

namespace ECS.Tanks
{
    public interface IColorComponent : IComponent
    {
        Color Color { set; }
    }
}