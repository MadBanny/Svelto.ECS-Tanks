using System.Collections;
using Svelto.ECS;

namespace ECS.Tanks.Camera
{
    public class CameraEntityView : EntityView
    {
        public ITransformComponent TransformComponent;
        public IPositionComponent PositionComponent;
        public ICameraComponent CameraComponent;
        public ICameraRigComponent CameraRigComponent;
    }
}
