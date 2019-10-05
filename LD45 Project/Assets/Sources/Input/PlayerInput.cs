using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reads player input and sets variables which can be read by whatever else to react to player input.
/// Allows compartimentalization of player input and game systems.
/// </summary>
public class PlayerInput : EntityInput
{
    static public ShipPiloting CurrentShip;
    static public event Action<GameObject> OnPlayerShipChanged = delegate { };

    private Camera targetingCamera;

    private void Awake()
    {
        targetingCamera = Camera.main;    
    }
    private void Update()
    {
        acceleration = Input.GetAxis("Vertical");
        turningDirection = -Input.GetAxis("Horizontal");
        strafingDirection = Input.GetAxis("Strafe");

        var ray = targetingCamera.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.forward, 0f);
        float dist;
        if (plane.Raycast(ray, out dist))
        {
            Vector2 hitPoint = ray.GetPoint(dist);
            targetedPosition = hitPoint - (Vector2)transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetedPosition, targetedPosition.magnitude);
            if (hit.collider != null)
            {
                targetedObject = hit.collider.gameObject;
            }
            else
            {
                targetedObject = null;
            }
        }


        usingActiveEquipment = Input.GetMouseButton(0);
        interacting = Input.GetKeyDown(KeyCode.F);
    }

    private void OnEnable()
    {
        CurrentShip = GetComponent<ShipPiloting>();
        OnPlayerShipChanged(CurrentShip.gameObject);
    }

    private void OnDisable()
    {
        CurrentShip = null;
        OnPlayerShipChanged(null);
    }
}
