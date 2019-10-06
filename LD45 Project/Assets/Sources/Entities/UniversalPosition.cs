using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalPosition : MonoBehaviour
{
    public const float SectorSize = 10000f;

    static UniversalPosition PlayerUniversalPosition;

    [SerializeField]
    private Vector2 sectorCoordinates;
    [SerializeField]
    private Vector2 localCoordinates;
    [SerializeField]
    private bool loadedIn; // When loaded in, use Unity coordinates for interaction with other loaded entities.
    // Entities get loaded when they are located in the same sector (or neighbouring sector) than the player.

}
