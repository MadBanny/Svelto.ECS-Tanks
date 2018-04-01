using Svelto.ECS;

namespace ECS.Tanks.Tank
{
    public class TankMovementSoundEntityView : EntityView
    {
        public IAudioSourceComponent AudioSourceComponent;
        public ITankMovementSoundsComponent TankMovementSoundsComponent;
        public ITankInputComponent TankInputComponent;
    }
}
