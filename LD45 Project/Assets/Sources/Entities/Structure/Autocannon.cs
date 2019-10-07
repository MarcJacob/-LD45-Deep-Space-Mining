using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autocannon : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeedModifier;
    [SerializeField]
    private float firingPeriod;
    [SerializeField]
    private Projectile projectilePrefab; // TODO pooling

    public event Action<Vector2, GameObject> OnActivation = delegate { };
    public event Action OnDeactivation = delegate { };
    public event Action<Vector2, GameObject> OnRunning = delegate { }; // Called each frame while the equipment is active.

    private ShipPiloting shipControllerInput;
    private float currentFiringCooldown = 0f;

    private void Awake()
    {
        shipControllerInput = GetComponent<ShipPiloting>();
    }

    private bool activatedLastFrame = false;
    private void Update()
    {
        currentFiringCooldown -= Time.deltaTime;
        if (shipControllerInput.CurrentPilot != null && shipControllerInput.CurrentPilot.usingActiveEquipment)
        {
            if (!activatedLastFrame)
            {
                activatedLastFrame = true;
                OnActivation(shipControllerInput.CurrentPilot.targetedPosition, null);
            }
            if (currentFiringCooldown < 0f)
            {
                currentFiringCooldown = firingPeriod;
                FireShot(shipControllerInput.CurrentPilot.targetedPosition);
            }
        }
        else if (activatedLastFrame)
        {
            DeactivateEquipment();
        }
    }

    private void FireShot(Vector2 dir)
    {
        var projectile = Instantiate(projectilePrefab.gameObject, transform.position, Quaternion.identity);
        projectile.transform.up = dir;
        projectile.GetComponent<Projectile>().Source = gameObject;
    }

    private void DeactivateEquipment()
    {
        OnDeactivation();
        activatedLastFrame = false;
    }
}
