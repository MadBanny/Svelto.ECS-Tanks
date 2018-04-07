using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ECS.Tanks.UI
{
    [RequireComponent(typeof(Slider)), AddComponentMenu("ECS/Tanks/Aim Slider Implementor")]
    public class AimSliderImplementor : MonoBehaviour, IImplementor, IAimSliderComponent
    {
        public Slider Slider { get; private set; }
        public float AimSliderValue { get { return Slider.value; } set { Slider.value = value; } }

        private void Awake()
        {
            Slider = GetComponent<Slider>();
        }
    }
}