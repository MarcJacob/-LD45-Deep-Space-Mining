using UnityEngine;

[RequireComponent(typeof(MiningBeam))]
public class BeamSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio;

    private MiningBeam beam;
    float startAudio;
    float goalAudio;
    private void Awake()
    {
        beam = GetComponent<MiningBeam>();
        beam.OnActivation += Beam_OnActivation;
        beam.OnDeactivation += Beam_OnDeactivation;
        startAudio = audio.volume;
        audio.volume = 0f;
    }

    private void Beam_OnDeactivation()
    {
        goalAudio = 0f;
    }

    private void Beam_OnActivation(Vector2 arg1, GameObject arg2)
    {
        goalAudio = 1f;
    }

    void Update()
    {
        audio.volume = Mathf.Lerp(audio.volume, goalAudio, Time.deltaTime * 5f);
    }
}