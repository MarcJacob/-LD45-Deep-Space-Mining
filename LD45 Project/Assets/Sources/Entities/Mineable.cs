using UnityEngine;

[RequireComponent(typeof(Cargo))]
public class Mineable : MonoBehaviour
{
    [SerializeField]
    private bool destroyOnMinedOut = true;

    private Cargo cargo;

    private void Awake()
    {
        cargo = GetComponent<Cargo>();
    }

    public uint Mine(uint resourceID, uint miningStrength)
    {
        uint withdrawn;
        cargo.WithdrawResourceToMax(resourceID, miningStrength, out withdrawn);
        if (cargo.IsEmpty() && destroyOnMinedOut)
        {
            Destroy(gameObject);
        }

        return withdrawn;
    }
}
