using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float baseProjectileSpeed;
    [SerializeField]
    private float baseDamage;
    [SerializeField]
    private bool dontHitSameFaction = true;
    [SerializeField]
    private float hitDetectionRange = 0.1f;
    [SerializeField]
    private float lifeTime = 10f;

    public Action<GameObject> OnProjectileCollided = delegate { };

    private GameObject source;
    private float speedModifier;

    private float speed; // Current actually applied speed.
    private float damageOnImpact; // Current damage if the projectile hits something.

    public GameObject Source
    {
        get { return source; }
        set { if (source == null) source = value; else Debug.LogError("Error ! Attempted to set up projectile source more than once."); }
    }

    public float SpeedModifier
    {
        get { return speedModifier; }
        set
        {
            speedModifier = value;
            speed = speedModifier * baseProjectileSpeed;
            damageOnImpact = baseDamage * speedModifier;
        }
    }

    private void Awake()
    {
        SpeedModifier = 1f;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);

        CheckForCollision();
    }

    private void CheckForCollision()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, hitDetectionRange);

        foreach(var hit in hits)
        {
            if (hit.collider.gameObject != source)
            {
                var hitOwnershipComponent = hit.collider.GetComponent<Ownership>();
                if (hitOwnershipComponent == null || hitOwnershipComponent.OwnerFactionID == 0 || !dontHitSameFaction || hitOwnershipComponent.OwnerFactionID != source.GetComponent<Ownership>().OwnerFactionID)
                {
                    OnCollision(hit.collider.gameObject);
                }
            }
        }
    }

    private void OnCollision(GameObject go)
    {
        var damageable = go.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Damage(damageOnImpact);
        }
        OnProjectileCollided(go);
        Destroy(gameObject);
    }
}
