using UnityEngine;

[RequireComponent(typeof(Dockable), typeof(SpriteRenderer))]
public class DisappearOnDocked : MonoBehaviour
{
    private Dockable dockableComponent;
    private SpriteRenderer spriteComponent;

    private void Awake()
    {
        dockableComponent = GetComponent<Dockable>();
        spriteComponent = GetComponent<SpriteRenderer>();

        dockableComponent.OnShipDocked += DockableComponent_OnShipDocked;
        dockableComponent.OnShipUndocked += DockableComponent_OnShipUndocked;
    }

    private void DockableComponent_OnShipUndocked(Dock obj)
    {
        spriteComponent.enabled = true;
    }

    private void DockableComponent_OnShipDocked(Dock obj)
    {
        spriteComponent.enabled = false;
    }


}