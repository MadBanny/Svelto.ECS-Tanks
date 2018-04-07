using Svelto.ECS;

namespace ECS.Tanks.Tank
{
    public class HealthEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        private readonly ISequencer _DamageSequence;

        public HealthEngine(ISequencer damageSequence)
        {
            _DamageSequence = damageSequence;
        }

        public void Ready()
        {

        }

        public void Step(ref DamageInfo damage, int condition)
        {
            var entityView = entityViewsDB.QueryEntityViews<HealthEntityView>();
            var healthComponent = entityView[0].HealthComponent;

            healthComponent.CurrentHealth -= 10;

            //the HealthEngine can branch the sequencer flow triggering two different
            //conditions
            if (healthComponent.CurrentHealth <= 0)
                _DamageSequence.Next(this, ref damage, (int)DamageCondition.DEAD);
            else
                _DamageSequence.Next(this, ref damage, (int)DamageCondition.DAMAGE);
        }
    }
}
