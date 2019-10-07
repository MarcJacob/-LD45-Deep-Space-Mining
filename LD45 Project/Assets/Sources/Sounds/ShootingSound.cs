using UnityEngine;

[RequireComponent(typeof(Autocannon))]
public class ShootingSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private Autocannon autoCannonComponent;
    private void Awake()
    {
        autoCannonComponent = GetComponent<Autocannon>();
        autoCannonComponent.OnRunning += AutocannonComponent_OnRunning;
    }

    private void AutocannonComponent_OnRunning(Vector2 arg1, GameObject arg2)
    {
        AudioSource.PlayClipAtPoint(clips[UnityEngine.Random.Range(0, clips.Length)], transform.position);
    }
}
