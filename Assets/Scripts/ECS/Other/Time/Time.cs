namespace ECS.Tanks
{
    public class Time : ITime
    {
        public float DeltaTime { get { return UnityEngine.Time.deltaTime; } }
    }
}
