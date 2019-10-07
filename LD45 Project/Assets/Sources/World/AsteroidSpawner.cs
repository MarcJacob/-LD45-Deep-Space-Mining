using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    static AsteroidSpawner Instance;
    static public void SpawnAsteroids(float startX, float startY, float endX, float endY, int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Vector2 randPos = new Vector2(Random.Range(startX, endX), Random.Range(startY, endY)); // TODO use custom position system
            Instantiate(Instance.asteroidPrefab, randPos, Quaternion.identity).transform.parent = Instance.parent;
        }
    }

    [SerializeField]
    private GameObject asteroidPrefab;
    [SerializeField]
    private bool spawnAsteroidsOnStart = false;
    [SerializeField]
    private int amount;
    [SerializeField]
    private Transform parent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (spawnAsteroidsOnStart)
        {
            SpawnAsteroids(-200f, -200f, 200f, 200f, amount);
        }
    }
}
