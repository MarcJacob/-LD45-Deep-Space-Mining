using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private float HP;

    public event Action OnDeath = delegate { };
    public event Action<float> OnDamageReceived = delegate { };

    public void Damage(float amount)
    {
        HP -= amount;
        OnDamageReceived(amount);
        if (HP < 0)
        {
            OnDeath();
            Destroy(gameObject);
        }
    }

    public void Heal(float amount)
    {

    }
}
