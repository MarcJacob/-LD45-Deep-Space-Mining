using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateSpawningManager : MonoBehaviour
{
    [SerializeField]
    GameObject pirateFrigatePrefab;
    [SerializeField]
    GameObject pirateCruiserPrefab;
    [SerializeField]
    private float manhattanSpawnDistance;
    [SerializeField]
    private float pirateSpawnPeriod;
    [SerializeField]
    private float minimumDifficultyNetworth;
    [SerializeField]
    private float maxDifficultyNetworth;
    [SerializeField]
    private int minDifficultyFrigatesAmount;
    [SerializeField]
    private int maxDifficultyFrigatesAmount;
    [SerializeField]
    private int minDifficultyCruiserAmount;
    [SerializeField]
    private int maxDifficultyCruiserAmount;

    private float currentSpawnCooldown = 0f;

    private void Update()
    {
        currentSpawnCooldown -= Time.deltaTime;
        if (currentSpawnCooldown < 0f)
        {
            SpawnWave();
            currentSpawnCooldown = pirateSpawnPeriod;
        }
    }

    private void SpawnWave()
    {
        float x = UnityEngine.Random.Range(-manhattanSpawnDistance, manhattanSpawnDistance);
        float y = manhattanSpawnDistance - Mathf.Abs(x);
        if (UnityEngine.Random.Range(0f, 1f) < 0.5f) y *= -1;

        Vector2 waveSpawn = new Vector2(x, y);
        // spawn frigates
        float difficulty = (GameManager.NetWorth - minimumDifficultyNetworth) / (maxDifficultyNetworth - minimumDifficultyNetworth);
        if (difficulty > 0f)
        {
            difficulty = Mathf.Min(difficulty, 1f);
            int frigateSpawnAmount = (int)Mathf.Lerp(minDifficultyFrigatesAmount, maxDifficultyFrigatesAmount, difficulty);
            for (int i = 0; i < frigateSpawnAmount; i++)
            {
                Vector2 offset = new Vector2(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
                Instantiate(pirateFrigatePrefab, offset + waveSpawn, Quaternion.identity);
            }
            int cruiserSpawnAmount = (int)Mathf.Lerp(minDifficultyCruiserAmount, maxDifficultyCruiserAmount, difficulty);
            for (int i = 0; i < cruiserSpawnAmount; i++)
            {
                Vector2 offset = new Vector2(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(-15f, 15f));
                Instantiate(pirateCruiserPrefab, offset + waveSpawn, Quaternion.identity);
            }
        }
        
    }
}
