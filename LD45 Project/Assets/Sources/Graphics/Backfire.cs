using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityInput))]
public class Backfire : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer backfireSprite;

    ShipPiloting piloting;
    EntityInput entityInput
    {
        get
        {
            return piloting.CurrentPilot;
        }
    }
    private void Awake()
    {
        piloting = GetComponent<ShipPiloting>();
        var col = backfireSprite.color;
        col.a = 0f;
        backfireSprite.color = col;
    }
    void Update()
    {
        if (entityInput != null)
        {
            var col = backfireSprite.color;
            col.a = Mathf.Lerp(col.a, Mathf.Max(entityInput.acceleration, 0f), Time.deltaTime * 5f);
            backfireSprite.color = col;
        }

    }
}
