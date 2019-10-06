using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipProperties : MonoBehaviour
{
    [SerializeField]
    private SHIP_SIZE size;
    [SerializeField]
    private float price;

    public SHIP_SIZE Size
    {
        get { return size; }
    }
    public float Price
    {
        get { return price; }
    }
}
