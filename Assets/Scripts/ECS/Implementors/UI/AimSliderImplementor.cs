using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ECS.Tanks.UI
{
    [RequireComponent(typeof(Slider)), AddComponentMenu("ECS/Tanks/Aim Slider Implementor")]
    public class AimSliderImplementor : MonoBehaviour, IImplementor, IAimSliderComponent
    {
        public Slider Slider { get; private set; }

        private void Awake()
        {
            Slider = GetComponent<Slider>();
        }
    }
}