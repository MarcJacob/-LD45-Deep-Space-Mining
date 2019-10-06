using UnityEngine;
/// <summary>
/// Objects with a trading component will be able to conduct trade with Dockable objects docked onto them.
/// </summary>
[RequireComponent(typeof(Dock), typeof(Cargo), typeof(Ownership))]
public class Trading : MonoBehaviour
{
    public static bool RunItemTransaction(Cargo sellerCargo, Cargo buyerCargo, RESOURCE_TYPE resourceType, uint amount, float pricePerUnit)
    {
        // Get owning party of buyer
        uint buyerFactionID = 0;
        uint sellerFactionID = 0;
        var buyerOwnership = buyerCargo.GetComponent<Ownership>();
        var sellerOwnership = sellerCargo.GetComponent<Ownership>();
        if (buyerOwnership != null)
        {
            buyerFactionID = buyerOwnership.OwnerFactionID;
        }
        if (sellerOwnership != null)
        {
            sellerFactionID = sellerOwnership.OwnerFactionID;
        }

        // TODO add treasury for NPC factions. For now, only player has to pay.
        
        float totalPrice = pricePerUnit * amount;
        bool transactionSuccessful = true;
        if (buyerFactionID == 1 && GameManager.PlayerCash < totalPrice) // player faction
        {
            Debug.LogError("Error during trading - player doesn't have enough cash");
            return false;
        }
        // Do the transaction
        uint withdrawn;
                
        if (!sellerCargo.WithdrawResourceToMax((uint)resourceType, amount, out withdrawn))
        {
            // Cancel transaction
            Debug.LogError("Error during trading - the seller does not have the necessary resources in cargo");
            sellerCargo.AddResource((uint)resourceType, withdrawn);
            transactionSuccessful = false;
        }
        else
        {
            // Transfer to buyer
            uint added;
            if (!buyerCargo.AddResource((uint)resourceType, amount, out added))
            {
                // Cancel transaction
                Debug.LogError("Error during trading - the buyer does not have the necessary cargo space !");
                sellerCargo.AddResource((uint)resourceType, withdrawn);
                buyerCargo.WithdrawResourceToMax((uint)resourceType, added, out added);
                transactionSuccessful = false;
            }
        }
        if (transactionSuccessful && buyerFactionID == 1) GameManager.RemoveCash((int)totalPrice);
        else if (transactionSuccessful && sellerFactionID == 1) GameManager.AddCash((int)totalPrice);
        return transactionSuccessful;
    }

    Dock dockComponent;
    Cargo cargoComponent;
    Ownership ownershipComponent;

    private void Awake()
    {
        dockComponent = GetComponent<Dock>();
        cargoComponent = GetComponent<Cargo>();
        ownershipComponent = GetComponent<Ownership>();
    }

    public void BuyFrom(Cargo cargo, RESOURCE_TYPE resourceType, uint amount, float pricePerUnit)
    {
        RunItemTransaction(cargo, cargoComponent, resourceType, amount, pricePerUnit);
    }

    public void SellTo(Cargo cargo, RESOURCE_TYPE resourceType, uint amount, float pricePerUnit)
    {
        RunItemTransaction(cargoComponent, cargo, resourceType, amount, pricePerUnit);
    }
}