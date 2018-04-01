using System.Collections;
using Svelto.ECS;
namespace ECS.Tanks.Tank {
    public class TankEntityView : EntityView
    {
        public ITankInputComponent TankInputComponent;
        public ISpeedComponent SpeedComponent;
        public ITankTurnSpeedComponent TurnSpeedComponent;
        public IPositionComponent PositionComponent;
        public ITransformComponent TransformComponent;
    }
}
