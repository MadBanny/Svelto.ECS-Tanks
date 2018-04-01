using UnityEngine;

namespace ECS.Tanks
{
    public interface IAudioSourceComponent : IComponent
    {
        AudioClip Clip { get; set; }
        float Pitch { get; set; }

        void PlayOneShot(AudioClip clip);
        void Play();
    }
}
