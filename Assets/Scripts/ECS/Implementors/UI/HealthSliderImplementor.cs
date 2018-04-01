using UnityEngine;
using UnityEngine.UI;

namespace ECS.Tanks.UI
{
    [RequireComponent(typeof(Slider)), AddComponentMenu("ECS/Tanks/Health Slider Implementor")]
    public class HealthSliderImplementor : MonoBehaviour, IImplementor, IHealthSliderComponent
    {
        public Slider HealthSlider { get; private set; }
        public Image FillImage;

        public int Value
        {
            set { HealthSlider.value = value; }
        }

        public Color FillImageColor { set { FillImage.color = value; } }

        private void Awake()
        {
            HealthSlider = GetComponent<Slider>();
        }
    }
}