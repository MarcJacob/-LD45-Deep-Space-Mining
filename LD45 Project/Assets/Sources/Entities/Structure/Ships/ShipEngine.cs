using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Determines how a ship moves both in the normal Unity coordinates system (when player or loaded) and in the custom grid-based coordinates system (when unloaded).
/// </summary>
public class ShipEngine : MonoBehaviour
{
    [SerializeField]
    private float dragRatio; // Direct multiplier to how much drag the ship experiences when moving in any direction. Effectively determines the maximum speed.
    [SerializeField]
    private float forwardAcceleration; // How much speed is added to the ship when going forward.
    [SerializeField]
    private float strafeAcceleration; // How much speed is added to the ship when strafing.
    [SerializeField]
    private float turnRate; // How quickly the ship turns.

    private Vector3 currentVelocity; // Current speed at which the ship is going.
    private ShipPiloting shipControllerInput;
    private Dockable dockableComponent;

    public Vector2 MovementVector { get; private set; }

    private void Awake()
    {
        shipControllerInput = GetComponent<ShipPiloting> ();
        dockableComponent = GetComponent<Dockable>();

        if (dockableComponent != null)
        {
            dockableComponent.OnShipDocked += DockableComponent_OnShipDocked;
            dockableComponent.OnShipUndocked += DockableComponent_OnShipUndocked;
        }
    }

    private void DockableComponent_OnShipUndocked(Dock dock)
    {
        enabled = true;
    }

    private void DockableComponent_OnShipDocked(Dock dock)
    {
        enabled = false;
    }

    private void FixedUpdate()
    {
        //TODO : Use "interface" class (not an actual interface, probably) to move either in Unity coordinates or custom grid based coordinates where appropriate.

        float acceleration = 0f;
        float strafe = 0f;
        float turn = 0f;
        if (shipControllerInput.CurrentPilot != null)
        {
            // Acceleration
            acceleration = forwardAcceleration * shipControllerInput.CurrentPilot.acceleration;

            float currentForwardAcceleration = Vector3.Dot(transform.up, currentVelocity) * currentVelocity.magnitude;

            if (acceleration < 0f && currentForwardAcceleration <= 0f)
            {
                acceleration = 0f;
            }
            currentVelocity += transform.up * acceleration * Time.fixedDeltaTime;
            // Turning

            turn = turnRate * shipControllerInput.CurrentPilot.turningDirection;
        }

        // Drag
        float speedDragRatio = 1 - (currentVelocity.magnitude * currentVelocity.magnitude * dragRatio);
        if (speedDragRatio > 0.98f) speedDragRatio = 0.98f;
        currentVelocity *= speedDragRatio >= 0f ? speedDragRatio : 0f;
        if (currentVelocity.magnitude < 0.005f && acceleration <= 0f && strafe != 0f) currentVelocity = Vector2.zero;

        Vector3 oldPos = transform.position;
        transform.Rotate(0f, 0f, turn);
        transform.Translate(currentVelocity, Space.World);
        MovementVector = currentVelocity;
    }

    private void OnDisable()
    {
        currentVelocity = Vector2.zero;
    }
}
