using UnityEngine;

public class ActivateOnPlayerShipDestroyed : MonoBehaviour
{
    [SerializeField]
    GameObject[] activated;

    GameObject lastPlayerShip;

    private void Awake()
    {
        PlayerInput.OnPlayerShipChanged += OnPlayerShipChanged;
    }

    private void OnPlayerShipChanged(GameObject obj)
    {
        if (lastPlayerShip != null) lastPlayerShip.GetComponent<Damageable>().OnDeath -= OnPlayerShipDeath;
        if (obj != null)
        {
            obj.GetComponent<Damageable>().OnDeath += OnPlayerShipDeath;
            lastPlayerShip = obj;
        }
    }

    private void OnPlayerShipDeath()
    {
        foreach (var go in activated)
        {
            go.SetActive(true);
        }
    }
}