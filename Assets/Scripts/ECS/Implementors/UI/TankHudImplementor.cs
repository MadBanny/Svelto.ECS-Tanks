using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ECS.Tanks.UI
{
    public class TankHudImplementor : MonoBehaviour, IImplementor, IHealthSliderComponent, IAimSliderComponent
    {
        public Slider HealthSlider;
        public Image HealthFillImage;

        public Slider AimSlider;

        public int Value { set { HealthSlider.value = value; } }
        public Color FillImageColor { set { HealthFillImage.color = value; } }
        
    }
}
