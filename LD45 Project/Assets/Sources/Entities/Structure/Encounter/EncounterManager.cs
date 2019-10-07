using System.Collections;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [SerializeField]
    private float encountersUpdatePeriod = 4f;
    [SerializeField]
    private int updateEncountersFramesCount = 10; // Number of frames over which all encounters will be updated.

    private float currentEncountersUpdatePeriod = 0f;

    private void Update()
    {
        currentEncountersUpdatePeriod -= Time.deltaTime;
        if (currentEncountersUpdatePeriod < 0f)
        {
            UpdateEncounters();
            currentEncountersUpdatePeriod = encountersUpdatePeriod;
        }
    }

    private void UpdateEncounters()
    {
        StartCoroutine(UpdateEncountersCoroutine());
    }

    IEnumerator UpdateEncountersCoroutine()
    {
        Encounter[] encounters = Encounter.GetAllEncounters();
        int encountersUpdatedPerFrame = encounters.Length / updateEncountersFramesCount;
        int extraEncountersToUpdate = encounters.Length % updateEncountersFramesCount;

        int encounterID = 0;
        for(int frame = 0; frame < updateEncountersFramesCount; frame++)
        {
            int encounter;
            for (encounter = 0; encounter < encountersUpdatedPerFrame; encounter++)
            {
                encounters[encounterID + encounter].Update();
            }
            encounterID += encounter;
            if (extraEncountersToUpdate > 0)
            {
                encounters[encounterID].Update();
                encounterID++;
                extraEncountersToUpdate--;
            }
            yield return null;
        }
    }
}
