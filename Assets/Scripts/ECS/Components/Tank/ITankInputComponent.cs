namespace ECS.Tanks.Tank
{
    public interface ITankInputComponent : IComponent
    {
        UnityEngine.Vector3 Input { get; set; }
        bool GetFireButton { get; set; }
        bool GetFireButtonDown { get; set; }
        bool GetFireButtonUp { get; set; }
        bool Fired { get; set; }
    }
}