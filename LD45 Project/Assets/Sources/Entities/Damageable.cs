using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private float HP;

    public event Action OnDeath = delegate { };
    public event Action<float> OnDamageReceived = delegate { };

    private void Awake()
    {
        var dockable = GetComponent<Dockable>();
        dockable.OnShipDocked += Dockable_OnShipDocked;
        dockable.OnShipUndocked += Dockable_OnShipUndocked;
    }

    private void Dockable_OnShipUndocked(Dock obj)
    {
        enabled = true;
    }

    private void Dockable_OnShipDocked(Dock obj)
    {
        enabled = false;
    }

    public void Damage(float amount)
    {
        if (enabled)
        {
            HP -= amount;
            OnDamageReceived(amount);
            if (HP < 0)
            {
                OnDeath();
                Destroy(gameObject);
            }
        }
    }

    public void Heal(float amount)
    {

    }
}
