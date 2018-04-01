namespace ECS.Tanks.Tank
{
    public interface ITankInputComponent : IComponent
    {
        UnityEngine.Vector3 Input { get; set; }
        bool Fire { get; set; }
    }
}