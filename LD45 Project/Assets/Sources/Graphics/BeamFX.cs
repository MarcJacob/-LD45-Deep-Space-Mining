using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MiningBeam))]
public class BeamFX : MonoBehaviour
{
    private MiningBeam miningBeam;

    [SerializeField]
    private ParticleSystem targetParticleFX;
    [SerializeField]
    private LineRenderer beamFX;

    ParticleSystem.EmissionModule fxEmission;
    Vector2 beamCurrentPosition;

    private void Awake()
    {
        miningBeam = GetComponent<MiningBeam>();
        miningBeam.OnActivation += MiningBeam_OnActivation;
        miningBeam.OnDeactivation += MiningBeam_OnDeactivation;
        miningBeam.OnRunning += MiningBeam_OnRunning;

        targetParticleFX = Instantiate(targetParticleFX, transform);
        beamFX = Instantiate(beamFX, transform);
        fxEmission = targetParticleFX.emission;
        fxEmission.enabled = false;

        beamFX.sortingLayerName = "Ships";
        beamFX.sortingOrder = -1;
    }

    bool beamActivated = false;
    private void Update()
    {
        if (beamActivated == false)
        {
            beamCurrentPosition = transform.position;
        }
        beamFX.SetPosition(0, transform.position);
        beamFX.SetPosition(1, Vector2.Lerp(beamFX.GetPosition(1), beamCurrentPosition, Time.deltaTime * 10f));

    }

    // Update position
    private void MiningBeam_OnRunning(Vector2 pos, GameObject arg2)
    {
        targetParticleFX.transform.position = pos;
        beamCurrentPosition = pos;
    }

    // Stop emitting
    private void MiningBeam_OnDeactivation()
    {
        fxEmission.enabled = false;
        beamActivated = false;
    }

    // Start emitting
    private void MiningBeam_OnActivation(Vector2 pos, GameObject arg2)
    {
        fxEmission.enabled = true;
        targetParticleFX.transform.position = pos;
        beamCurrentPosition = pos;
        beamActivated = true;
    }
}
