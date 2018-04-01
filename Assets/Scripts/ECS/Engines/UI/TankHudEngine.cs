﻿using System.Collections;
using Svelto.ECS;
using Svelto.Tasks;

using ECS.Tanks.Tank;
using Color = UnityEngine.Color;

namespace ECS.Tanks.UI
{
    public class TankHudEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        public void Ready()
        {
            Tick().Run();
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                var tankHudEntityViews = entityViewsDB.QueryEntityViews<TankHudEntityView>();
                var hudDamageEntityViews = entityViewsDB.QueryEntityViews<HudDamageEntityView>();

                if(tankHudEntityViews.Count > 0)
                {
                    for(int i = 0; i < tankHudEntityViews.Count; i++)
                    {
                        HudDamageEntityView hudDamageEntityView = hudDamageEntityViews[i];
                        TankHudEntityView tankHudEntityView = tankHudEntityViews[i];
                        IHealthSliderComponent healthSlider = tankHudEntityView.HealthSliderComponent;

                        healthSlider.Value = hudDamageEntityView.HealthComponent.CurrentHealth;
                        healthSlider.FillImageColor = Color.Lerp(Color.red, Color.green, hudDamageEntityView.HealthComponent.CurrentHealth / 100);
                        //healthSlider.FillImageColor = Color.Lerp(Color.red, Color.green, m_CurrentHealth / m_StartingHealth);
                    }
                }
                yield return null;
            }
        }

        public void Step(ref DamageInfo token, int condition)
        {

        }
    }
}