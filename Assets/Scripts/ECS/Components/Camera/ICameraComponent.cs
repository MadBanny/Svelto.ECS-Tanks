

namespace ECS.Tanks.Camera
{
    public interface ICameraComponent : IComponent
    {
        float OrthographicSize { get; set; }
        bool Orthographic { get; set; }
        float Aspect { get; set; }
    }
}