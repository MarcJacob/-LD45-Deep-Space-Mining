using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityInput))]
public class SoundOnAcceleration : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio;

    float startAudio;
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
        startAudio = audio.volume;
        audio.volume = 0f;
    }
    void Update()
    {
        if (entityInput != null)
            audio.volume = Mathf.Lerp(audio.volume, Mathf.Min(startAudio, Mathf.Max(entityInput.acceleration, 0f)), Time.deltaTime * 5f);
    }
}
