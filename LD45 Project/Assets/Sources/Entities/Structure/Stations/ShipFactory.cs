using UnityEngine;

[RequireComponent(typeof(Dock))]
public class ShipFactory : MonoBehaviour
{
    [SerializeField]
    private float shipSpawnCooldown;
    [SerializeField]
    private GameObject[] potentialShips;

    private float currentShipSpawnCooldown = 0f;
    private Dock dockComponent;

    private void Awake()
    {
        dockComponent = GetComponent<Dock>();
    }

    private void Update()
    {
        currentShipSpawnCooldown -= Time.deltaTime;
        if (currentShipSpawnCooldown < 0f)
        {
            SpawnShip();
            currentShipSpawnCooldown = shipSpawnCooldown;
        }
    }

    private void SpawnShip()
    {
        var shipPrefab = potentialShips[UnityEngine.Random.Range(0, potentialShips.Length)];
        var newShip = Instantiate(shipPrefab);
        newShip.name = shipPrefab.name;
        // TODO Set position of ship depending on custom grid based system.

        newShip.transform.position = transform.position;
        newShip.GetComponent<Dockable>().AskDock(dockComponent);
    }
}