using System;

namespace Battle
{
    public interface IDamageable
    {
        event Action<float> DamageTaken;
        void TakeDamage(float damage);
    }
}