using UnityEngine;

public class Mineable : MonoBehaviour
{
    [SerializeField]
    private bool destroyOnMinedOut = true;
    [SerializeField]
    private uint amountLeft;
    [SerializeField]
    private RESOURCE_TYPE resourceType;

    public RESOURCE_TYPE ResourceType { get { return resourceType; } }

    public uint Mine(uint resourceID, uint miningStrength)
    {
        uint withdrawn = (uint)Mathf.Min(amountLeft, miningStrength);
        amountLeft -= withdrawn;
        if (amountLeft <= 0 && destroyOnMinedOut)
        {
            Destroy(gameObject);
        }

        return withdrawn;
    }
}
