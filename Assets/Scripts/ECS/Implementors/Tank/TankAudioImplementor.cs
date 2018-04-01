using UnityEngine;

namespace ECS.Tanks.Tank
{
    [RequireComponent(typeof(AudioSource))]
    public class TankAudioImplementor : MonoBehaviour, IImplementor, IAudioSourceComponent, ITankMovementSoundsComponent
    {
        [SerializeField]
        private AudioClip _IdleAudioClip;
        [SerializeField]
        private AudioClip _EngineDrivingAudioClip;

        public AudioSource _AudioSource { get; private set; }

        public AudioClip Clip { get { return _AudioSource.clip; } set { _AudioSource.clip = value; } }

        public float Pitch { get { return _AudioSource.pitch; } set { _AudioSource.pitch = value; } }

        public AudioClip IdleAudioClip { get { return _IdleAudioClip; } }

        public AudioClip EngineDrivingAudioClip { get { return _EngineDrivingAudioClip; } }

        public void Play()
        {
            _AudioSource.Play();
        }

        public void PlayOneShot(AudioClip clip)
        {
            _AudioSource.PlayOneShot(clip);
        }

        private void Awake()
        {
            _AudioSource = GetComponent<AudioSource>();
        }
    }
}
